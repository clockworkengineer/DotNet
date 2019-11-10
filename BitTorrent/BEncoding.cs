﻿//
// Author: Robert Tizzard
//
// Library: C# class library to implement the BitTorrent protocol.
//
// Description: Bencodeing encoding/decoder support code for BitTorrent.
//
// Copyright 2019.
//

using System;
using System.Collections.Generic;
using System.Text;

namespace BitTorrent
{
    /// <summary>
    /// Base for BNode Tree.
    /// </summary>
    public class BNodeBase
    {

    }

    /// <summary>
    /// Dictionary BNode.
    /// </summary>
    public class BNodeDictionary : BNodeBase
    {
        public Dictionary<string, BNodeBase> dict;

        public BNodeDictionary()
        {
            dict = new Dictionary<string, BNodeBase>();
        }
    }

    /// <summary>
    /// List BNode.
    /// </summary>
    public class BNodeList : BNodeBase
    {

        public List<BNodeBase> list;

        public BNodeList()
        {
            list = new List<BNodeBase>();
        }
    }

    /// <summary>
    /// Number BNode.
    /// </summary>
    public class BNodeNumber : BNodeBase
    {
        public byte[] number;
    }

    /// <summary>
    /// String BNode.
    /// </summary>
    public class BNodeString : BNodeBase
    {
        public byte[] str;
    }

    /// <summary>
    /// Support methods for Bencoding.
    /// </summary>
    public static class Bencoding
    {
        /// <summary>
        /// Extracts a Bencoded string and returns its bytes.
        /// </summary>
        /// <returns>The bytes for the string.</returns>
        /// <param name="buffer">buffer - Bencoded string input buffer.</param>
        /// <param name="position">position - Position in buffer. Updated to character after string on exit.</param>
        static private byte[] extractString(byte[] buffer, ref int position)
        {

            int end = position;

            while (buffer[end] != ':')
            {
                end++;
            }

            byte[] lengthBytes = new byte[end - position];
            Buffer.BlockCopy(buffer, position, lengthBytes, 0, end - position);

            int lengthOfString = int.Parse(Encoding.ASCII.GetString(lengthBytes));

            if (end + lengthOfString > buffer.Length)
            {
                lengthOfString = buffer.Length - end - 1;
            }

            position = end + lengthOfString + 1;

            byte[] retBuffer = new byte[lengthOfString];
            Buffer.BlockCopy(buffer, end + 1, retBuffer, 0, lengthOfString);

            return (retBuffer);

        }

        /// <summary>
        /// Extracts a Bencoded number and returns its bytes.
        /// </summary>
        /// <returns>The bytes(characters) for the number.</returns>
        /// <param name="buffer">buffer - Bencoded number input buffer.</param>
        /// <param name="position">position - Position in buffer. Updated to character after number on exit.</param>
        static private BNodeBase decodeNumber(byte[] buffer, ref int position)
        {
            BNodeNumber bNode = new BNodeNumber();

            position++;

            int end = position;
            while (buffer[end] != (byte) 'e')
            {
                end++;
            }
            bNode.number = new byte[end - position];
            Buffer.BlockCopy(buffer, position, bNode.number, 0, end - position);

            position = end + 1;

            return (bNode);

        }

        /// <summary>
        /// Creates a BNode string node.
        /// </summary>
        /// <returns>A string BNode.</returns>
        /// <param name="buffer">buffer - Bencoded string input buffer.</param>
        /// <param name="position">position - Position in buffer. Updated to character after string on exit.</param>
        static private BNodeBase decodeString(byte[] buffer, ref int position)
        {
            BNodeString bNode = new BNodeString();

            bNode.str = extractString(buffer, ref position);

            return (bNode);

        }

        /// <summary>
        /// Decodes a dictionary string key.
        /// </summary>
        /// <returns>String value of the key.</returns>
        /// <param name="buffer">buffer - Bencoded string input buffer.</param>
        /// <param name="position">position - Position in buffer. Updated to character after string on exit.</param>
        static private string decodeKey(byte[] buffer, ref int position)
        {
            string key = Encoding.ASCII.GetString(extractString(buffer, ref position));

            return (key);

        }

        /// <summary>
        /// Create a list BNode.
        /// </summary>
        /// <returns>A list BNode.</returns>
        /// <param name="buffer">buffer - Bencoded list input buffer.</param>
        /// <param name="position">position - Position in buffer. Updated to character after list on exit.</param>
        static private BNodeBase decodeList(byte[] buffer, ref int position)
        {

            BNodeList bNode = new BNodeList();

            position++;

            while (buffer[position] != (byte)'e')
            {

                switch ((char)buffer[position])
                {
                    case 'd':
                        bNode.list.Add(decodeDictionary(buffer, ref position));
                        break;

                    case 'l':
                        bNode.list.Add(decodeList(buffer, ref position));
                        break;

                    case 'i':
                        bNode.list.Add(decodeNumber(buffer, ref position));
                        break;

                    default:
                        bNode.list.Add(decodeString(buffer, ref position));
                        break;

                }
                if (position == buffer.Length) break;

            }

            position++;

            return (bNode);

        }

        /// <summary>
        /// Create a dictionary BNode.
        /// </summary>
        /// <returns>A dictionary BNode.</returns>
        /// <param name="buffer">buffer - Bencoded dictionary input buffer.</param>
        /// <param name="position">position - Position in buffer. Updated to character after dictionary on exit.</param>
        static private BNodeBase decodeDictionary(byte[] buffer, ref int position)
        {

            BNodeDictionary bNode = new BNodeDictionary();

            position++;

            while (buffer[position] != (byte)'e')
            {
                string key = decodeKey(buffer, ref position);

                switch ((char)buffer[position])
                {
                    case 'd':
                        bNode.dict[key] = decodeDictionary(buffer, ref position);
                        break;

                    case 'l':
                        bNode.dict[key] = decodeList(buffer, ref position);
                        break;

                    case 'i':
                        bNode.dict[key] = decodeNumber(buffer, ref position);
                        break;

                    default:
                        bNode.dict[key] = decodeString(buffer, ref position);
                        break;
                }
                if (position == buffer.Length) break;

            }

            position++;

            return (bNode);

        }

        /// <summary>
        /// Recursively parse a Bencoded string and return its BNode tree.
        /// </summary>
        /// <returns>Root of Bnode tree.</returns>
        /// <param name="buffer">buffer - Bencoded input buffer.</param>
        /// <param name="position">position - Position in buffer. SHould be at end of buffer on exit.</param>
        static private BNodeBase decodeBNodes(byte[] buffer, ref int position)
        {
            BNodeBase bNode=null;

            while (buffer[position]!=(byte) 'e')
            {
                switch((char) buffer[position])
                {
                    case 'd':
                        bNode = decodeDictionary(buffer, ref position);
                        break;

                    case 'l':
                        bNode = decodeList(buffer, ref position);
                        break;

                    case 'i':
                        bNode = decodeNumber(buffer, ref position);
                        break;

                    default:
                        bNode = decodeString(buffer, ref position);
                        break;

                }
                if (position == buffer.Length) break;
            }

            return (bNode);

        }

        /// <summary>
        /// Decode the specified Bendcoded buffer.
        /// </summary>
        /// <returns>The root BNode.</returns>
        /// <param name="buffer">buffer - Bencoded input buffer.</param>
        static public BNodeBase decode(byte[] buffer)
        {
            int position = 0;

            BNodeBase bNodeRoot = decodeBNodes(buffer, ref position);

            return (bNodeRoot);

        }

        /// <summary>
        /// Produce Bencoded output given a root BNode.
        /// </summary>
        /// <returns>Bencoded representation of BNode tree.</returns>
        /// <param name="bNode">Root <paramref name="bNode"/>.</param>
        static public byte[] encode(BNodeBase bNode)
        {
            List<byte> result = new List<byte>();

            try
            {
                if (bNode is BNodeDictionary)
                {
                    result.Add((byte)'d');
                    foreach (var key in ((BNodeDictionary)bNode).dict.Keys)
                    {
                        result.AddRange(Encoding.ASCII.GetBytes($"{key.Length}:{key}"));
                        result.AddRange(encode(((BNodeDictionary)bNode).dict[key]));
                    }
                    result.Add((byte)'e');
                }
                else if (bNode is BNodeList)
                {
                    result.Add((byte)'l');
                    foreach (var node in ((BNodeList)bNode).list)
                    {
                        result.AddRange(encode(node));
                    }
                    result.Add((byte)'e');
                }
                else if (bNode is BNodeNumber)
                {
                    result.Add((byte)'i');
                    result.AddRange(((BNodeNumber)bNode).number);
                    result.Add((byte)'e');
                }
                else if (bNode is BNodeString)
                {
                    result.AddRange(Encoding.ASCII.GetBytes($"{((BNodeString)bNode).str.Length}:"));
                    result.AddRange(((BNodeString)bNode).str);

                }
            }
            catch (Exception ex)
            {
                Program.Logger.Debug(ex);
            }

            return (result.ToArray());
        }

        /// <summary>
        /// Get a BNode entry for a given dictionary key. Note that it recursively searches
        /// until the key is found in the BNode tree structure.
        /// </summary>
        /// <returns>BNode entry for dictionary key.</returns>
        /// <param name="bNode">bNode - Bnode root of dictionary.</param>
        /// <param name="entryKey">entryKey - Dictionary key of Bnode entry to return.</param>
        static public BNodeBase getDictionaryEntry(BNodeBase bNode, string entryKey)
        {
            BNodeBase bNodeEntry=null;

            try
            {
                if (bNode is BNodeDictionary)
                {
                    if (((BNodeDictionary)bNode).dict.ContainsKey(entryKey))
                    {
                        return (((BNodeDictionary)bNode).dict[entryKey]);
                    }
                    else
                    {
                        foreach (var key in ((BNodeDictionary)bNode).dict.Keys)
                        {
                            bNodeEntry = getDictionaryEntry(((BNodeDictionary)bNode).dict[key], entryKey);
                            if (bNodeEntry != null) break;
                        }
                    }

                }
                else if (bNode is BNodeList)
                {
                    foreach (var node in ((BNodeList)bNode).list)
                    {
                        bNodeEntry = getDictionaryEntry(node, entryKey);
                        if (bNodeEntry != null) break;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.Logger.Debug(ex);
            }

            return (bNodeEntry);

        }

        /// <summary>
        /// Return string representation of a given BNode dictionary entry.
        /// </summary>
        /// <returns>String value of a given byte encoded number or string.</returns>
        /// <param name="bNode">bNode - Entry to extract.</param>
        /// <param name="entryKey">entryKey - Key of dictionary entry to extract.</param>
        static public string getDictionaryEntryString(BNodeBase bNode, string entryKey)
        {
            BNodeBase entryNode = null;

            try
            {
                entryNode = getDictionaryEntry(bNode, entryKey);
                if (entryNode != null)
                {
                    if (entryNode is BNodeString)
                    {
                        return (Encoding.ASCII.GetString(((BNodeString)entryNode).str));
                    }
                    else if (entryNode is BNodeNumber)
                    {
                        return (Encoding.ASCII.GetString(((BNodeNumber)entryNode).number));
                    }
                }
            }
            catch (Exception ex)
            {
                Program.Logger.Debug(ex);
            }

            return ("");
        }

    }
}
