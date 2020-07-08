
using Neon.Communication.Packets.Outgoing.Rooms.Settings;
using Neon.HabboHotel.Rooms;

namespace Neon.Communication.Packets.Incoming.Rooms.Settings
{
    internal class GetRoomBannedUsersEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
            {
                return;
            }

            Room Instance = Session.GetHabbo().CurrentRoom;
            if (Instance == null || !Instance.CheckRights(Session, true))
            {
                return;
            }

            if (Instance.BannedUsers().Count > 0)
            {
                Session.SendMessage(new GetRoomBannedUsersComposer(Instance));
            }
        }
    }
}
