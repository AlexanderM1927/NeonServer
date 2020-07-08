using Neon.Communication.Packets.Outgoing.Moderation;
using Neon.HabboHotel.Moderation;

namespace Neon.Communication.Packets.Incoming.Moderation
{
    internal class ReleaseTicketEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
            {
                return;
            }

            int Amount = Packet.PopInt();

            for (int i = 0; i < Amount; i++)
            {
                if (!NeonEnvironment.GetGame().GetModerationManager().TryGetTicket(Packet.PopInt(), out ModerationTicket Ticket))
                {
                    continue;
                }

                Ticket.Moderator = null;
                NeonEnvironment.GetGame().GetClientManager().SendMessage(new ModeratorSupportTicketComposer(Session.GetHabbo().Id, Ticket), "mod_tool");
            }
        }
    }
}