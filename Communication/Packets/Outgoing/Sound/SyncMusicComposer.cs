namespace Neon.Communication.Packets.Outgoing.Rooms.Music
{
    internal class SyncMusicComposer : ServerPacket
    {
        public SyncMusicComposer(int SongId, int PlaylistItemNumber, int SyncTimestampMs)
            : base(ServerPacketHeader.SyncMusicMessageComposer)
        {
            if (SongId == 0)
            {
                base.WriteInteger(-1);
                base.WriteInteger(-1);
                base.WriteInteger(-1);
                base.WriteInteger(-1);
                base.WriteInteger(-1);
            }
            else
            {
                base.WriteInteger(SongId);
                base.WriteInteger(PlaylistItemNumber);
                base.WriteInteger(SongId);
                base.WriteInteger(1);
                base.WriteInteger(SyncTimestampMs);
            }
        }
    }
}
