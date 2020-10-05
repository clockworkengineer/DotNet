//
// Author: Robert Tizzard
//
// Library: C# class library to implement the BitTorrent protocol.
//
// Description: Perform HTTP annouce requests in remote tracker.
//
// Copyright 2019.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace BitTorrentLibrary
{

    /// <summary>
    /// HTTP Announcer
    /// </summary>
    public class AnnouncerHTTP : IAnnouncer
    {
        /// <summary>
        /// Decodes the announce request response recieved from a tracker.
        /// </summary>
        /// <param name="announceResponse">Announce response.</param>
        /// <param name="decodedResponse">Response.</param>
        private void DecodeAnnounceResponse(Tracker tracker, byte[] announceResponse, ref AnnounceResponse decodedResponse)
        {
            if (announceResponse.Length != 0)
            {
                BNodeBase decodedAnnounce = Bencoding.Decode(announceResponse);
                decodedResponse.statusMessage = Bencoding.GetDictionaryEntryString(decodedAnnounce, "failure reason");
                if (decodedResponse.statusMessage != "")
                {
                    decodedResponse.failure = true;
                    return; // If failure present then ignore rest of reply.
                }

                UInt32.TryParse(Bencoding.GetDictionaryEntryString(decodedAnnounce, "complete"), out decodedResponse.complete);
                UInt32.TryParse(Bencoding.GetDictionaryEntryString(decodedAnnounce, "incomplete"), out decodedResponse.incomplete);

                BNodeBase field = Bencoding.GetDictionaryEntry(decodedAnnounce, "peers");
                if (field != null)
                {
                    decodedResponse.peers = new List<PeerDetails>();
                    if (field is BNodeString bNodeString)
                    {
                        byte[] peers = (bNodeString).str;
                        for (var num = 0; num < peers.Length; num += 6)
                        {
                            PeerDetails peer = new PeerDetails
                            {
                                _peerID = String.Empty,
                                ip = $"{peers[num]}.{peers[num + 1]}.{peers[num + 2]}.{peers[num + 3]}"
                            };
                            peer.port = ((UInt32)peers[num + 4] * 256) + peers[num + 5];
                            if (peer.ip != tracker.Ip) // Ignore self in peers list
                            {
                                Log.Logger.Trace($"Peer {peer.ip} Port {peer.port} found.");
                                decodedResponse.peers.Add(peer);
                            }
                        }
                    }
                    else if (field is BNodeList bNodeList)
                    {
                        foreach (var listItem in (bNodeList).list)
                        {
                            if (listItem is BNodeDictionary bNodeDictionary)
                            {
                                PeerDetails peer = new PeerDetails();
                                BNodeBase peerDictionaryItem = (bNodeDictionary);
                                BNodeBase peerField = Bencoding.GetDictionaryEntry(peerDictionaryItem, "ip");
                                if (peerField != null)
                                {
                                    peer.ip = Encoding.ASCII.GetString(((BitTorrentLibrary.BNodeString)peerField).str);
                                }
                                if (peer.ip.Contains(":"))
                                {
                                    peer.ip = peer.ip.Substring(peer.ip.LastIndexOf(":", StringComparison.Ordinal) + 1);
                                }
                                peerField = Bencoding.GetDictionaryEntry(peerDictionaryItem, "port");
                                if (peerField != null)
                                {
                                    peer.port = UInt32.Parse(Encoding.ASCII.GetString(((BitTorrentLibrary.BNodeNumber)peerField).number));
                                }
                                if (peer.ip != tracker.Ip) // Ignore self in peers list
                                {
                                    Log.Logger.Trace($"Peer {peer.ip} Port {peer.port} found.");
                                    decodedResponse.peers.Add(peer);
                                }
                            }
                        }
                    }
                }

                UInt32.TryParse(Bencoding.GetDictionaryEntryString(decodedAnnounce, "interval"), out decodedResponse.interval);
                UInt32.TryParse(Bencoding.GetDictionaryEntryString(decodedAnnounce, "min interval"), out decodedResponse.minInterval);
                decodedResponse.trackerID = Bencoding.GetDictionaryEntryString(decodedAnnounce, "tracker id");

                decodedResponse.statusMessage = Bencoding.GetDictionaryEntryString(decodedAnnounce, "warning message");

                decodedResponse.announceCount++;
            }
        }
        /// <summary>
        /// Intialise instance of HTTP tracker.
        /// </summary>
        /// <param name="trackerURL"></param>
        public AnnouncerHTTP(string trackerURL)
        {

        }
        /// <summary>
        /// Perform an announce request to tracker and return any response.
        /// </summary>
        /// <returns>Announce response.</returns>
        public AnnounceResponse Announce(Tracker tracker)
        {
            Log.Logger.Info($"Announce: info_hash={Encoding.ASCII.GetString(WebUtility.UrlEncodeToBytes(tracker.InfoHash, 0, tracker.InfoHash.Length))} " +
                   $"peer_id={tracker.PeerID} port={tracker.Port} compact={tracker.Compact} no_peer_id={tracker.NoPeerID} uploaded={tracker.Uploaded}" +
                   $"downloaded={tracker.Downloaded} left={tracker.Left} event={Tracker.EventString[(int)tracker.Event]} ip={tracker.Ip} key={tracker.Key}" +
                   $" trackerid={tracker.TrackerID} numwanted={tracker.NumWanted}");

            AnnounceResponse response = new AnnounceResponse();

            try
            {
                string announceURL = $"{tracker.TrackerURL}?info_hash={Encoding.ASCII.GetString(WebUtility.UrlEncodeToBytes(tracker.InfoHash, 0, tracker.InfoHash.Length))}" +
                    $"&peer_id={tracker.PeerID}&port={tracker.Port}&compact={tracker.Compact}" +
                    $"&no_peer_id={tracker.NoPeerID}&uploaded={tracker.Uploaded}&downloaded={tracker.Downloaded}" +
                    $"&left={tracker.Left}&event={Tracker.EventString[(int)tracker.Event]}" +
                    $"&ip={tracker.Ip}&key={tracker.Key}&trackerid={tracker.TrackerID}&numwanted={tracker.NumWanted}";

                HttpWebRequest httpGetRequest = WebRequest.Create(announceURL) as HttpWebRequest;

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
                    if (httpGetResponse.StatusCode == HttpStatusCode.OK)
                    {
                        DecodeAnnounceResponse(tracker, announceResponse, ref response);
                    }
                    else
                    {
                        Log.Logger.Debug("Bittorrent (Tracker) Error: " + httpGetResponse.StatusDescription);
                        throw new Error("Bittorrent (Tracker) Error: " + httpGetResponse.StatusDescription);
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
                // Display first error as message concatination can be unreadable if too deep
                throw new Error("BitTorrent Error (Tracker): " + ex.GetBaseException().Message);
            }

            return response;
        }
    }
}