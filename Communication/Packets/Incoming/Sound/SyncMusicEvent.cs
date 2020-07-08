using Neon.Communication.Packets.Outgoing.Rooms.Music;
using Neon.HabboHotel.Rooms;

namespace Neon.Communication.Packets.Incoming.Rooms.Music
{
    internal class SyncMusicEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Room Instance = Session.GetHabbo().CurrentRoom;

            if (Instance == null)
            {
                return;
            }

            if (Instance.GetRoomMusicManager().IsPlaying)
            {
                Session.SendMessage(new SyncMusicComposer(Instance.GetRoomMusicManager().CurrentSong.SongData.Id, Instance.GetRoomMusicManager().SongQueuePosition, Instance.GetRoomMusicManager().SongSyncTimestamp));
            }
        }
    }
}
