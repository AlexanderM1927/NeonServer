using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Rooms;

namespace Neon.Communication.Packets.Incoming.Moderation
{
    internal class ModerationKickEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().GetPermissions().HasRight("mod_kick"))
            {
                return;
            }

            int UserId = Packet.PopInt();
            string Message = Packet.PopString();

            GameClient Client = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
            if (Client == null || Client.GetHabbo() == null || Client.GetHabbo().CurrentRoomId < 1 || Client.GetHabbo().Id == Session.GetHabbo().Id)
            {
                return;
            }

            if (Client.GetHabbo().Rank >= Session.GetHabbo().Rank)
            {
                Session.SendNotification(NeonEnvironment.GetGame().GetLanguageLocale().TryGetValue("moderation_kick_permissions"));
                return;
            }

            if (!NeonEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room Room))
            {
                return;
            }

            Room.GetRoomUserManager().RemoveUserFromRoom(Client, true, false);
        }
    }
}
