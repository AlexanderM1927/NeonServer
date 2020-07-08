using Neon.Communication.Packets.Outgoing.Rooms.Music;

namespace Neon.Communication.Packets.Incoming.Rooms.Music
{
    internal class GetJukeboxDisksEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session != null && (Session.GetHabbo() != null))
            {
                Session.SendMessage(new GetJukeboxDisksComposer(Session.GetHabbo().GetInventoryComponent().songDisks));
            }
        }
    }
}
