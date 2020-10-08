//
// Author: Robert Tizzard
//
// Library: C# class library to implement the BitTorrent protocol.
//
// Description:
//
// Copyright 2020.
//

using System;

namespace BitTorrentLibrary
{
  public interface IAnnouncer
    {
        AnnounceResponse Announce(Tracker tracker);
    }
}
