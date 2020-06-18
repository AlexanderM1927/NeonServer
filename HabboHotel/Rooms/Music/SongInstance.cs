using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Users;
using Neon.Communication.Packets.Incoming;
using System.Collections.Concurrent;

using Neon.Database.Interfaces;
using log4net;
using Neon.HabboHotel.Items;

namespace Neon.HabboHotel.Rooms.Music
{
    public class SongInstance
    {
        private readonly SongItem mDiskItem;
        private readonly SongData mSongData;

        public SongInstance(SongItem Item, SongData SongData)
        {
            mDiskItem = Item;
            mSongData = SongData;
        }

        public SongData SongData
        {
            get { return mSongData; }
        }

        public SongItem DiskItem
        {
            get { return mDiskItem; }
        }
    }
}