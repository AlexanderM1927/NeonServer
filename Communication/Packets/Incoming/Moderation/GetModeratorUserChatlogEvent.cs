
using Neon.Communication.Packets.Outgoing.Moderation;
using Neon.HabboHotel.Users;

namespace Neon.Communication.Packets.Incoming.Moderation
{
    internal class GetModeratorUserChatlogEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
            {
                return;
            }

            if (!Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
            {
                return;
            }

            int UserId = Packet.PopInt();
            Habbo Habbo = NeonEnvironment.GetHabboById(UserId);

            if (Habbo == null)
            {
                Session.SendNotification("Oops, no se consigue este usuario");
                return;
            }

            try
            {
                Session.SendMessage(new ModeratorUserChatlogComposer(UserId));
            }
            catch { Session.SendNotification("Overflow :/"); }
        }
    }
}
