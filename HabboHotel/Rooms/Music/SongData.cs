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

namespace Neon.HabboHotel.Rooms.Music
{
    public class SongData
    {
        private readonly string mArtist;
        private readonly string mData;
        private readonly int mID;
        private readonly double mLength;
        private readonly string mName;

        public SongData(int songid, string Name, string Artist, string Data, double Length)
        {
            mID = songid;
            mName = Name;
            mArtist = Artist;
            mData = Data;
            mLength = Length;
        }

        public double LengthSeconds
        {
            get { return mLength; }
        }

        public int LengthMiliseconds
        {
            get { return (int)(mLength * 1000.0); }
        }

        public string Name
        {
            get { return mName; }
        }

        public string Artist
        {
            get { return mArtist; }
        }

        public string Data
        {
            get { return mData; }
        }

        public int Id
        {
            get { return mID; }
        }
    }
}