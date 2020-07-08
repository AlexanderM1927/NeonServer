using Neon.Communication.Packets.Outgoing.Moderation;
using Neon.HabboHotel.Moderation;

namespace Neon.Communication.Packets.Incoming.Moderation
{
    internal class PickTicketEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
            {
                return;
            }

            int Junk = Packet.PopInt();//??
            int TicketId = Packet.PopInt();

            if (!NeonEnvironment.GetGame().GetModerationManager().TryGetTicket(TicketId, out ModerationTicket Ticket))
            {
                return;
            }

            Ticket.Moderator = Session.GetHabbo();
            NeonEnvironment.GetGame().GetClientManager().SendMessage(new ModeratorSupportTicketComposer(Session.GetHabbo().Id, Ticket), "mod_tool");
        }
    }
}
