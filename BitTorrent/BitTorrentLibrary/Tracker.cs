﻿//
// Author: Robert Tizzard
//
// Library: C# class library to implement the BitTorrent protocol.
//
// Description: 
//
// Copyright 2019.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Timers;

namespace BitTorrent
{
    /// <summary>
    /// Tracker.
    /// </summary>
    public class Tracker
    {

        /// <summary>
        /// Tracker event.
        /// </summary>
        public enum TrackerEvent
        {
            started = 0,
            stopped,
            completed
        };

        private System.Timers.Timer _announceTimer;
        private string _trackerURL = String.Empty;
        private string _peerID = String.Empty;
        private FileAgent _torrentFileAgent;
        private AnnounceResponse _currentTrackerResponse;
        private UInt32 _port = 6681;
        private string _ip = String.Empty;
        private UInt32 _compact = 1;
        private UInt32 _noPeerID;
        private UInt64 _uploaded;
        private UInt64 _downloaded;
        private UInt64 _left;
        private TrackerEvent _event;
        private string _key = String.Empty;
        private string _trackerID = String.Empty;
        private UInt32 _numWanted = 5;
        private UInt32 _interval = 2000;

        public UInt64 Uploaded { get => _uploaded; set => _uploaded = value; }
        public UInt64 Downloaded { get => _downloaded; set => _downloaded = value; }
        public UInt64 Left { get => _left; set => _left = value; }
        public TrackerEvent Event { get => _event; set => _event = value; }

        /// <summary>
        /// Constructs the response.
        /// </summary>
        /// <returns>The response.</returns>
        /// <param name="announceResponse">Announce response.</param>
        /// <param name="response">Response.</param>
        private AnnounceResponse ConstructResponse(byte[] announceResponse, ref AnnounceResponse response)
        {

            response.statusCode = (int)HttpStatusCode.OK;

            BNodeBase decodedAnnounce = Bencoding.Decode(announceResponse);

            UInt32.TryParse(Bencoding.GetDictionaryEntryString(decodedAnnounce, "complete"), out response.complete);
            UInt32.TryParse(Bencoding.GetDictionaryEntryString(decodedAnnounce, "incomplete"), out response.incomplete);

            BNodeBase field = Bencoding.GetDictionaryEntry(decodedAnnounce, "peers");
            if (field != null)
            {
                response.peers = new List<PeerDetails>();
                if (field is BNodeString)
                {
                    byte[] peers = ((BNodeString)field).str;
                    UInt32 numberPeers = (UInt32)peers.Length / 6;
                    for (var num = 0; num < peers.Length; num += 6)
                    {
                        PeerDetails peer = new PeerDetails();
                        peer._peerID = String.Empty;
                        peer.ip = $"{peers[num]}.{peers[num + 1]}.{peers[num + 2]}.{peers[num + 3]}";
                        if (peer.ip.Contains(":"))
                        {
                            peer.ip = peer.ip.Substring(peer.ip.LastIndexOf(":", StringComparison.Ordinal) + 1);
                        }
                        peer.port = (UInt32)peers[num + 4] * 256 + peers[num + 5];
                        Log.Logger.Trace($"Peer {peer.ip} Port {peer.port} found.");
                        response.peers.Add(peer);
                    }
                }
                else if (field is BNodeList)
                {
                    foreach (var listItem in ((BNodeList)(field)).list)
                    {
                        if (listItem is BNodeDictionary)
                        {
                            PeerDetails peer = new PeerDetails();
                            BNodeBase peerDictionaryItem = ((BitTorrent.BNodeDictionary)listItem);
                            BNodeBase peerField = Bencoding.GetDictionaryEntry(peerDictionaryItem, "ip");
                            if (peerField != null)
                            {
                                string path = string.Empty;
                                peer.ip = Encoding.ASCII.GetString(((BitTorrent.BNodeString)peerField).str);
                            }
                            if (peer.ip.Contains(":"))
                            {
                                peer.ip = peer.ip.Substring(peer.ip.LastIndexOf(":", StringComparison.Ordinal) + 1);
                            }
                            peerField = Bencoding.GetDictionaryEntry(peerDictionaryItem, "port");
                            if (peerField != null)
                            {
                                string path = string.Empty;
                                peer.port = UInt32.Parse(Encoding.ASCII.GetString(((BitTorrent.BNodeNumber)peerField).number));
                            }
                            Log.Logger.Trace($"Peer {peer.ip} Port {peer.port} found.");
                            response.peers.Add(peer);
                        }
                    }
                }

            }

            UInt32.TryParse(Bencoding.GetDictionaryEntryString(decodedAnnounce, "interval"), out response.interval);
            UInt32.TryParse(Bencoding.GetDictionaryEntryString(decodedAnnounce, "min interval"), out response.minInterval);
            response.trackerID = Bencoding.GetDictionaryEntryString(decodedAnnounce, "tracker id");
            response.statusMessage = Bencoding.GetDictionaryEntryString(decodedAnnounce, "failure reason");
            response.statusMessage = Bencoding.GetDictionaryEntryString(decodedAnnounce, "warning message");

            response.announceCount++;

            return (response);

        }

        /// <summary>
        /// Encodes the bytes to URL.
        /// </summary>
        /// <returns>The bytes to URL.</returns>
        /// <param name="toEncode">To encode.</param>
        private byte[] EncodeBytesToURL(byte[] toEncode)
        {
            return (WebUtility.UrlEncodeToBytes(toEncode, 0, toEncode.Length));

        }

        /// <summary>
        /// Encodes the tracker URL.
        /// </summary>
        /// <returns>The tracker URL.</returns>
        private string EncodeTrackerURL()
        {
            string url = _trackerURL +
            "?info_hash=" + Encoding.ASCII.GetString(EncodeBytesToURL(_torrentFileAgent.TorrentMetaInfo.MetaInfoDict["info hash"])) +
            "&peer_id=" + _peerID +
            "&port=" + _port +
            "&compact=" + _compact +
            "&no_peer_id=" + _noPeerID +
            "&uploaded=" + Uploaded +
            "&downloaded=" + Downloaded +
            "&left=" + Left +
            "&event=" + Event +
            "&ip=" + _ip +
            "&key=" + _key +
            "&trackerid=" + _trackerID +
            "&numwanted=" + _numWanted;

            return (url);

        }

        /// <summary>
        /// Updates the peers status.
        /// </summary>
        private void UpdatePeersStatus()
        {
            int unChokedPeers = 0;
            int activePeers = 0;

            foreach (var peer in _torrentFileAgent.RemotePeers)
            {
                if (peer.PeerChoking.WaitOne(0))
                {
                    unChokedPeers++;
                }
                if (peer.Active)
                {
                    activePeers++;
                }
            }

            Log.Logger.Info($"Unchoked Peers {unChokedPeers}/{ _torrentFileAgent.RemotePeers.Count}");
            Log.Logger.Info($"Active Peers {activePeers}/{ _torrentFileAgent.RemotePeers.Count}");

        }

        /// <summary>
        /// Ons the announce event.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="e">E.</param>
        /// <param name="tracker">Tracker.</param>
        private static void OnAnnounceEvent(Object source, ElapsedEventArgs e, Tracker tracker)
        {
            tracker._currentTrackerResponse = tracker.Announce();
            tracker.UpdatePeersStatus();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BitTorrent.Tracker"/> class.
        /// </summary>
        /// <param name="torrentFileAgent">Torrent file agent.</param>
        public Tracker(FileAgent torrentFileAgent)
        {
            _trackerURL = Encoding.ASCII.GetString(torrentFileAgent.TorrentMetaInfo.MetaInfoDict["announce"]);
            _torrentFileAgent = torrentFileAgent;
            _peerID = PeerID.get();
            _ip = Peer.GetLocalHostIP();

        }

        /// <summary>
        /// Announce this instance.
        /// </summary>
        /// <returns>The announce.</returns>
        public AnnounceResponse Announce()
        {

            Log.Logger.Info($"Announce: info_hash={Encoding.ASCII.GetString(EncodeBytesToURL(_torrentFileAgent.TorrentMetaInfo.MetaInfoDict["info hash"]))} " +
                  $"peer_id={_peerID} port={_port} compact={_compact} no_peer_id={_noPeerID} uploaded={Uploaded}" +
                  $"downloaded={Downloaded} left={Left} event={Event} ip={_ip} key={_key} trackerid={_trackerID} numwanted={_numWanted}");

            AnnounceResponse response = new AnnounceResponse();

            try
            {
                HttpWebRequest httpGetRequest = WebRequest.Create(EncodeTrackerURL()) as HttpWebRequest;

                httpGetRequest.Method = "GET";
                httpGetRequest.ContentType = "text/xml";

                using (HttpWebResponse httpGetResponse = httpGetRequest.GetResponse() as HttpWebResponse)
                {
                    byte[] announceResponse;
                    StreamReader reader = new StreamReader(httpGetResponse.GetResponseStream());
                    using (var memstream = new MemoryStream())
                    {
                        reader.BaseStream.CopyTo(memstream);
                        announceResponse = memstream.ToArray();
                    }
                    response.statusCode = (UInt32)httpGetResponse.StatusCode;
                    if (httpGetResponse.StatusCode == HttpStatusCode.OK)
                    {
                        ConstructResponse(announceResponse, ref response);
                    }
                    else
                    {
                        response.statusMessage = httpGetResponse.StatusDescription;
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
                response.statusMessage = ex.Message;
            }

            return (response);

        }

        /// <summary>
        /// Starts the announcing.
        /// </summary>
        public void StartAnnouncing()
        {
            try
            {
                _announceTimer = new System.Timers.Timer(_interval);
                _announceTimer.Elapsed += (sender, e) => OnAnnounceEvent(sender, e, this);
                _announceTimer.AutoReset = true;
                _announceTimer.Enabled = true;
            }
            catch (Error)
            {
                throw;
            }
            catch (Exception ex)
            {
                Log.Logger.Debug(ex);
            }
        }

        /// <summary>
        /// Stops the annonncing.
        /// </summary>
        public void StopAnnonncing()
        {
            try
            {
                _announceTimer.Stop();
                _announceTimer.Dispose();
            }
            catch (Error)
            {
                throw;
            }
            catch (Exception ex)
            {
                Log.Logger.Debug(ex);
            }
        }

        /// <summary>
        /// Update the specified response.
        /// </summary>
        /// <param name="response">Response.</param>
        public void Update(AnnounceResponse response)
        {
            try
            {
                UInt32 oldInterval = _interval;
                if (response.minInterval != 0)
                {
                    _interval = response.minInterval;
                }
                else
                {
                    _interval = response.interval;
                }
                _trackerID = response.trackerID;
                if (oldInterval != _interval)
                {
                    StopAnnonncing();
                    StartAnnouncing();
                }
            }
            catch (Error)
            {
                throw;
            }
            catch (Exception ex)
            {
                Log.Logger.Debug(ex);
            }
        }
    }
}