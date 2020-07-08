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

        public SongData SongData => mSongData;

        public SongItem DiskItem => mDiskItem;
    }
}