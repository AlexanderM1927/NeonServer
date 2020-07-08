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

        public double LengthSeconds => mLength;

        public int LengthMiliseconds => (int)(mLength * 1000.0);

        public string Name => mName;

        public string Artist => mArtist;

        public string Data => mData;

        public int Id => mID;
    }
}