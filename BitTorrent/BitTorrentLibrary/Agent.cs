﻿//
// Author: Robert Tizzard
//
// Library: C# class library to implement the BitTorrent protocol.
//
// Description: All the high level torrent processing including download and upload.
//
// Copyright 2019.
//

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BitTorrent
{
    /// <summary>
    /// File Agent class definition.
    /// </summary>
    public class Agent
    {
        public delegate void ProgessCallBack(Object progressData);
        private readonly HashSet<string> _deadPeersList;             // Peers that failed to connect
        private readonly ManualResetEvent _downloading;              // WaitOn when downloads == false
        private readonly Downloader _torrentDownloader;              // Downloader for torrent
        public Dictionary<string, Peer> RemotePeers { get; set; }    // Connected remote peers
        public byte[] InfoHash { get; }                              // Torrent info hash
        public string TrackerURL { get; }                            // Main Tracker URL
        public Tracker MainTracker { get; set; }                     // Main torrent tracker

        /// <summary>
        /// Assembles the pieces of a torrent block by block.A task is created using this method for each connected peer.
        /// </summary>
        /// <returns>Task reference on completion.</returns>
        /// <param name="remotePeer">Remote peer.</param>
        /// <param name="progressFunction">Progress function.</param>
        /// <param name="progressData">Progress data.</param>
        private void AssemblePieces(Peer remotePeer, ProgessCallBack progressFunction, Object progressData, CancellationTokenSource cancelAssemblerTaskSource)
        {
            try
            {
                Log.Logger.Debug($"Running block assembler for peer {Encoding.ASCII.GetString(remotePeer.RemotePeerID)}.");

                CancellationToken cancelAssemblerTask = cancelAssemblerTaskSource.Token;

                PWP.Unchoke(remotePeer);

                remotePeer.BitfieldReceived.WaitOne();

                remotePeer.PeerChoking.WaitOne();

                while (MainTracker.Left != 0)
                {
                    for (UInt32 nextPiece = 0; _torrentDownloader.SelectNextPiece(remotePeer, ref nextPiece);)
                    {
                        Log.Logger.Debug($"Assembling blocks for piece {nextPiece}.");

                        remotePeer.TransferingPiece = nextPiece;

                        UInt32 blockNumber = 0;
                        for (; !remotePeer.TorrentDownloader.Dc.IsBlockPieceLast(nextPiece, blockNumber); blockNumber++)
                        {
                            PWP.Request(remotePeer, nextPiece, blockNumber * Constants.BlockSize, Constants.BlockSize);
                        }

                        PWP.Request(remotePeer, nextPiece, blockNumber * Constants.BlockSize,
                                     _torrentDownloader.Dc.pieceMap[nextPiece].lastBlockLength);

                        remotePeer.WaitForPieceAssembly.WaitOne();
                        remotePeer.WaitForPieceAssembly.Reset();

                        if (remotePeer.TransferingPiece != -1)
                        {
                            remotePeer.TransferingPiece = -1;

                            Log.Logger.Debug($"All blocks for piece {nextPiece} received");

                            _torrentDownloader.Dc.pieceBufferWriteQueue.Add(new PieceBuffer(remotePeer.AssembledPiece));

                            MainTracker.Left = _torrentDownloader.Dc.totalBytesToDownload - _torrentDownloader.Dc.totalBytesDownloaded;
                            MainTracker.Downloaded = _torrentDownloader.Dc.totalBytesDownloaded;
                        }
                        else
                        {
                            Log.Logger.Error($"Error : Piece {nextPiece} lost.");
                        }

                        progressFunction?.Invoke(progressData);

                        if (cancelAssemblerTask.IsCancellationRequested)
                        {
                            foreach (var peer in RemotePeers.Values)
                            {
                                if (!peer.PeerChoking.WaitOne(0))
                                {
                                    peer.PeerChoking.Set();
                                }
                            }
                            return;
                        }

                        Log.Logger.Info((_torrentDownloader.Dc.totalBytesDownloaded / (double)_torrentDownloader.Dc.totalBytesToDownload).ToString("0.00%"));

                        _downloading.WaitOne();

                        remotePeer.PeerChoking.WaitOne();
                    }

                    foreach (var peer in RemotePeers.Values)
                    {
                        if (!peer.PeerChoking.WaitOne(0))
                        {
                            peer.PeerChoking.Set();
                        }
                    }
                }

                Log.Logger.Debug($"Exiting block assembler for peer {Encoding.ASCII.GetString(remotePeer.RemotePeerID)}.");
            }
            catch (Error ex)
            {
                Log.Logger.Error(ex.Message);
                cancelAssemblerTaskSource.Cancel();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                cancelAssemblerTaskSource.Cancel();
            }
        }
        /// <summary>
        /// Return the number of bytes left in a torrent to download.
        /// </summary>
        /// <returns></returns>
        public UInt64 BytesLeftToDownload()
        {
            return _torrentDownloader.Dc.totalBytesToDownload - _torrentDownloader.Dc.totalBytesDownloaded;
        }

        /// <summary>
        /// Initializes a new instance of the Agent class.
        /// </summary>
        /// <param name="torrentFileName">Torrent file name.</param>
        /// <param name="downloadPath">Download path.</param>
        public Agent(MetaInfoFile torrentFile, String downloadPath)
        {
            _torrentDownloader = new Downloader(torrentFile, downloadPath);
            _torrentDownloader.BuildDownloadedPiecesMap();
            RemotePeers = new Dictionary<string, Peer>();
            InfoHash = torrentFile.MetaInfoDict["info hash"];
            TrackerURL = Encoding.ASCII.GetString(torrentFile.MetaInfoDict["announce"]);
            _deadPeersList = new HashSet<string>();
            _downloading = new ManualResetEvent(true);
        }
        /// <summary>
        /// /// Connect peers and add to swarm on success.
        /// </summary>
        /// <param name="peers"></param>
        public void ConnectPeersAndAddToSwarm(List<PeerDetails> peers)
        {
            Log.Logger.Info("Connecting any new peers to swarm ....");

            foreach (var peer in peers)
            {
                try
                {
                    if (!RemotePeers.ContainsKey(peer.ip) && !_deadPeersList.Contains(peer.ip))
                    {
                        Peer remotePeer = new Peer(_torrentDownloader, peer.ip, peer.port, InfoHash);
                        remotePeer.Connect();
                        if (remotePeer.Connected)
                        {
                            RemotePeers.Add(remotePeer.Ip, remotePeer);
                            Log.Logger.Info($"BTP: Local Peer [{ PeerID.Get()}] to remote peer [{Encoding.ASCII.GetString(remotePeer.RemotePeerID)}].");
                        }
                    }
                }
                catch (Exception)
                {
                    Log.Logger.Info($"Failed to connect to {peer.ip}");
                    _deadPeersList.Add(peer.ip);
                }
            }

            Log.Logger.Info($"Number of peers in swarm  {RemotePeers.Count}");
        }

        /// <summary>
        /// Startup File Agent.This includes starting announces to main tracker, building pieces
        /// to download map and getting and building the initial peer swarm/dead list.
        /// </summary>
        ///
        public void Startup()
        {
            try
            {
                Log.Logger.Info("Initial main tracker announce ...");

                ConnectPeersAndAddToSwarm(MainTracker.Announce().peers); ;

                MainTracker.StartAnnouncing();
            }
            catch (Error)
            {
                throw;
            }
            catch (Exception ex)
            {
                Log.Logger.Debug(ex);
                throw new Error("BitTorrent Error (Agent): Failed to load File Agent.");
            }
        }

        /// <summary>
        /// Download a torrent using an piece assembler per connected peer.
        /// </summary>
        /// <param name="progressFunction">User defined grogress function.</param>
        /// <param name="progressData">User defined grogress function data.</param>
        public void Download(ProgessCallBack progressFunction = null, Object progressData = null)
        {
            try
            {
                if (MainTracker.Left > 0)
                {
                    List<Task> assembleTasks = new List<Task>();
                    CancellationTokenSource cancelAssemblerTaskSource = new CancellationTokenSource();

                    Log.Logger.Info("Starting torrent download for MetaInfo data ...");

                    MainTracker.Event = Tracker.TrackerEvent.started;

                    foreach (var peer in RemotePeers.Values)
                    {
                        assembleTasks.Add(Task.Run(() => AssemblePieces(peer, progressFunction, progressData, cancelAssemblerTaskSource)));
                    }

                    _torrentDownloader.Dc.CheckForMissingBlocksFromPeers();

                    Task.WaitAll(assembleTasks.ToArray());

                    if (!cancelAssemblerTaskSource.IsCancellationRequested)
                    {
                        MainTracker.Event = Tracker.TrackerEvent.completed;
                        Log.Logger.Info("Whole Torrent finished downloading.");
                    }
                    else
                    {
                        Log.Logger.Info("Download aborted.");
                        Close();
                    }
                }
            }
            catch (Error)
            {
                throw;
            }
            catch (Exception ex)
            {
                Log.Logger.Debug(ex);
                throw new Error("BitTorrent Error (Agent): Failed to download torrent file.");
            }
        }

        /// <summary>
        /// Load Agent asynchronously.
        /// </summary>
        /// <returns>The async.</returns>
        public async Task LoadAsync()
        {
            try
            {
                await Task.Run(() => Startup()).ConfigureAwait(false);
            }
            catch (Error)
            {
                throw;
            }
            catch (Exception ex)
            {
                Log.Logger.Debug(ex);
                throw new Error("BitTorrent Error (Agent): " + ex.Message);
            }
        }

        /// <summary>
        /// Download torrent asynchronously.
        /// </summary>
        /// <param name="progressFunction">User defined grogress function.</param>
        /// <param name="progressData">User defined grogress function data.</param>
        public async Task DownloadAsync(ProgessCallBack progressFunction = null, Object progressData = null)
        {
            try
            {
                await Task.Run(() => Download(progressFunction, progressData)).ConfigureAwait(false);
            }
            catch (Error)
            {
                throw;
            }
            catch (Exception ex)
            {
                Log.Logger.Debug(ex);
                throw new Error("BitTorrent Error (Agent): " + ex.Message);
            }
        }

        /// <summary>
        /// Closedown Agent
        /// </summary>
        public void Close()
        {
            try
            {
                MainTracker.StopAnnonncing();
                if (RemotePeers != null)
                {
                    Log.Logger.Info("Closing peer sockets.");
                    foreach (var peer in RemotePeers.Values)
                    {
                        peer.Close();
                    }
                }
            }
            catch (Error)
            {
                throw;
            }
            catch (Exception ex)
            {
                Log.Logger.Debug(ex);
                throw new Error("BitTorrent Error (Agent): " + ex.Message);
            }
        }

        /// <summary>
        /// Start downloading torrent.
        /// </summary>
        public void Start()
        {
            try
            {
                _downloading.Set();
            }
            catch (Error)
            {
                throw;
            }
            catch (Exception ex)
            {
                Log.Logger.Debug(ex);
                throw new Error("BitTorrent Error (Agent): " + ex.Message);
            }
        }

        /// <summary>
        /// Pause downloading torrent.
        /// </summary>
        public void Pause()
        {
            try
            {
                _downloading.Reset();
            }
            catch (Error)
            {
                throw;
            }
            catch (Exception ex)
            {
                Log.Logger.Debug(ex);
                throw new Error("BitTorrent Error (Agent): " + ex.Message);
            }
        }
    }
}
