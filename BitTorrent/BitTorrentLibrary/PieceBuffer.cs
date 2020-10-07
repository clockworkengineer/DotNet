﻿//
// Author: Robert Tizzard
//
// Library: C# class library to implement the BitTorrent protocol.
//
// Description: Buffer used to hold and assemble  pieces from
// blocks downloaded/uploaded.
//
// Copyright 2019.
//

using System;
using System.Linq;

namespace BitTorrentLibrary
{
    /// <summary>
    /// Assembled piece buffer.
    /// </summary>
    public class PieceBuffer
    {
        private bool[] _blockPresent;                   // == true then block present
        private uint _blockCount;                       // Unfilled block spaces in buffer
        public uint Length { get; set; }                // Piece Length
        public byte[] Buffer { get; set; }              // Piece Buffer
        public uint Number { get; set; }                // Piece Number
        public bool AllBlocksThere => _blockCount == 0;   // == true All blocks have been downloaded

        /// <summary>
        /// Create an empty piece buffer.
        /// </summary>
        /// <param name="length">Length.</param>
        public PieceBuffer(UInt32 length)
        {
            Number = 0;
            Length = length;
            Buffer = new byte[Length];
            _blockCount = length / Constants.BlockSize;
            _blockPresent = new bool[_blockCount];

        }

        /// <summary>
        /// Create a piece buffer from a copy of another.
        /// </summary>
        /// <param name="pieceBuffer">Piece buffer.</param>
        public PieceBuffer(PieceBuffer pieceBuffer)
        {
            Number = pieceBuffer.Number;
            Length = pieceBuffer.Length;
            Buffer = new byte[Length];
            pieceBuffer.Buffer.CopyTo(Buffer, 0);
            _blockCount = pieceBuffer._blockCount;
            _blockPresent = new bool[pieceBuffer._blockPresent.Length];
            pieceBuffer._blockPresent.CopyTo(_blockPresent, 0);
        }

        /// <summary>
        ///  Add piece message packet block to piece buffer
        /// </summary>
        /// <param name="packetBuffer"></param>
        /// <param name="blockNumber"></param>
        public void AddBlockFromPacket(byte[] packetBuffer, UInt32 blockNumber)
        {
            System.Buffer.BlockCopy(packetBuffer, 9, Buffer, (Int32)blockNumber * Constants.BlockSize, (Int32)packetBuffer.Length - 9);
            if (!_blockPresent[blockNumber])
            {
                _blockPresent[blockNumber] = true;
                _blockCount--;
            }

        }
        /// <summary>
        /// Reset piece so can be refilled
        /// </summary>
        public void Reset()
        {
            _blockCount = (uint)_blockPresent.Length;
            _blockPresent = Enumerable.Repeat(false, (int)_blockCount).ToArray();
        }

        public void SetBlocksPresent(UInt32 pieceLength) {
            _blockCount = pieceLength / Constants.BlockSize;
        }


    }
}
