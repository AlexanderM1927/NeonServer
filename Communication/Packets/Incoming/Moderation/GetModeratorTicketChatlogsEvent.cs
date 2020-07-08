using Neon.Communication.Packets.Outgoing.Moderation;
using Neon.HabboHotel.Moderation;
using Neon.HabboHotel.Rooms;

namespace Neon.Communication.Packets.Incoming.Moderation
{
    internal class GetModeratorTicketChatlogsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().GetPermissions().HasRight("mod_tickets"))
            {
                return;
            }

            int TicketId = Packet.PopInt();

            if (!NeonEnvironment.GetGame().GetModerationManager().TryGetTicket(TicketId, out ModerationTicket Ticket) || Ticket.Room == null)
            {
                return;
            }

            RoomData Data = NeonEnvironment.GetGame().GetRoomManager().GenerateRoomData(Ticket.Room.Id);
            if (Data == null)
            {
                return;
            }

            Session.SendMessage(new ModeratorTicketChatlogComposer(Ticket, Data, Ticket.Timestamp));
        }
    }
}
