﻿//
// Author: Robert Tizzard
//
// Library: C# class library to implement the BitTorrent protocol.
//
// Description: Class encaspulating peer data resources and functionlaity.
//
// Copyright 2020.
//
using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace BitTorrentLibrary
{
    /// <summary>
    /// Peer.
    /// </summary>
    internal class Peer
    {
        private readonly Mutex _closeMutex;                              // Close mutex
        private readonly PeerNetwork _network;                           // Network layer
        public bool Connected { get; set; }                              // == true connected to remote peer
        public byte[] RemotePeerID { get; set; }                         // Id of remote peer
        public TorrentContext Tc { get; set; }                           // Torrent torrent context
        public byte[] RemotePieceBitfield { get; set; }                  // Remote peer piece map
        public PieceBuffer AssembledPiece { get; set; }                  // Assembled pieces buffer
        public string Ip { get; }                                        // Remote peer ip
        public uint Port { get; }                                        // peer Port
        public Task AssemblerTask { get; set; }                          // Peer piece assembly task
        public bool AmInterested { get; set; } = false;                  // == true then client interested in remote peer
        public bool AmChoking { get; set; } = true;                      // == true then client is choking remote peer.
        public ManualResetEvent PeerChoking { get; }                     // == true (set) then remote peer is choking client (local host)
        public bool PeerInterested { get; set; } = false;                // == true then remote peer interested in client (local host)
        public CancellationTokenSource CancelTaskSource { get; set; }    // Cancelation token source for cancel task request token
        public ManualResetEvent WaitForPieceAssembly { get; }            // When event set then piece has been fully assembled
        public ManualResetEvent BitfieldReceived { get; }                // When event set then peer has recieved bitfield from remote peer
        public UInt32 NumberOfMissingPieces { get; set; }                // Number of missing pieces from a remote peers torrent
        public byte[] ReadBuffer => _network.ReadBuffer;                 // Network read buffer
        public UInt32 PacketLength => _network.PacketLength;             // Current read packet length

        /// <summary>
        /// Setup data and resources needed by peer.
        /// </summary>
        /// <param name="ip">Ip.</param>
        /// <param name="port">Port.</param>
        /// <param name="infoHash">Info hash.</param>
        /// <param name="tc">torrent context.</param>
        /// 
        public Peer(string ip, UInt32 port, TorrentContext tc, Socket socket)
        {
            Ip = ip;
            Port = port;
            _network = new PeerNetwork(socket);
            WaitForPieceAssembly = new ManualResetEvent(false);
            PeerChoking = new ManualResetEvent(false);
            BitfieldReceived = new ManualResetEvent(false);
            CancelTaskSource = new CancellationTokenSource();
            if (tc != null)
            {
                SetTorrentContext(tc);
            }
            _closeMutex = new Mutex();
        }
        /// <summary>
        /// Set torrent context and dependant fields.
        /// </summary>
        /// <param name="tc"></param>
        public void SetTorrentContext(TorrentContext tc)
        {
            Tc = tc;
            NumberOfMissingPieces = Tc.NumberOfPieces;
            AssembledPiece = new PieceBuffer(tc, Tc.PieceLength);
            RemotePieceBitfield = new byte[tc.Bitfield.Length];
        }
        /// <summary>
        /// Send packet to remote peer.
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        public void PeerWrite(byte[] buffer)
        {
            _network.Write(buffer);
        }

        /// <summary>
        /// Read packet from remote peer.
        /// </summary>
        /// <returns>The read.</returns>
        /// <param name="buffer">Buffer.</param>
        /// <param name="length">Length.</param>
        public int PeerRead(byte[] buffer, int length)
        {
            return _network.Read(buffer, length);
        }
        /// <summary>
        ///  Connect to remote peer.
        /// </summary>
        /// <param name="manager"></param>
        public void Connect(Manager manager)
        {
            ValueTuple<bool, byte[]> peerResponse;

            // No socket passed to constructor so need to connect to get it
            if (_network.PeerSocket == null)
            {
                _network.Connect(Ip, Port);
                peerResponse = PWP.ConnectToIntialHandshake(this);
            }
            else
            {
                peerResponse = PWP.ConnectFromIntialHandshake(this, manager);
            }
            if (peerResponse.Item1)
            {
                RemotePeerID = peerResponse.Item2;
                PWP.Bitfield(this, Tc.Bitfield);
                Connected = true;
                _network.StartReads(this);
            }

        }
        /// <summary>
        /// Release  any peer class resources. Iam sure there is  better way than using a mutex to
        /// solve the mutual exclusion issue with
        /// </summary>
        public void Close()
        {
            _closeMutex.WaitOne();

            if (Connected)
            {
                Log.Logger.Info($"Closing down Peer {Encoding.ASCII.GetString(RemotePeerID)}...");
                Connected = false;
                CancelTaskSource.Cancel();
                if (Tc.PeerSwarm.ContainsKey(Ip))
                {
                    if (Tc.PeerSwarm.TryRemove(Ip, out Peer deadPeer))
                    {
                        Log.Logger.Info($"Dead Peer {Ip} removed from swarm.");
                    }
                }
                _network.Close();
                Log.Logger.Info($"Closed down {Encoding.ASCII.GetString(RemotePeerID)}.");
            }

            _closeMutex.ReleaseMutex();
        }
        /// <summary>
        /// Check downloaded bitfield to see if a piece is present on a remote peer.
        /// </summary>
        /// <returns><c>true</c>, if piece on remote peer was ised, <c>false</c> otherwise.</returns>
        /// <param name="pieceNumber">Piece number.</param>
        public bool IsPieceOnRemotePeer(UInt32 pieceNumber)
        {
            return (RemotePieceBitfield[pieceNumber >> 3] & 0x80 >> (Int32)(pieceNumber & 0x7)) != 0;
        }
        /// <summary>
        /// Sets piece bit in local bitfield to signify its presence.
        /// </summary>
        /// <param name="pieceNumber">Piece number.</param>
        public void SetPieceOnRemotePeer(UInt32 pieceNumber)
        {

            if (!IsPieceOnRemotePeer(pieceNumber))
            {
                RemotePieceBitfield[pieceNumber >> 3] |= (byte)(0x80 >> (Int32)(pieceNumber & 0x7));
                NumberOfMissingPieces--;
            }

        }
        /// <summary>
        /// Place a block into the current piece being assembled.
        /// </summary>
        /// <param name="pieceNumber">Piece number.</param>
        /// <param name="blockOffset">Block offset.</param>
        public void PlaceBlockIntoPiece(UInt32 pieceNumber, UInt32 blockOffset)
        {

            UInt32 blockNumber = blockOffset / Constants.BlockSize;

            Log.Logger.Trace($"PlaceBlockIntoPiece({pieceNumber},{blockOffset},{_network.PacketLength - 9})");

            AssembledPiece.AddBlockFromPacket(_network.ReadBuffer, blockNumber);

            if (AssembledPiece.AllBlocksThere)
            {
                AssembledPiece.Number = pieceNumber;
                WaitForPieceAssembly.Set();
            }
        }

    }
}
