
using Neon.Communication.Packets.Outgoing.Inventory.Badges;

namespace Neon.Communication.Packets.Incoming.Inventory.Badges
{
    internal class GetBadgesEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new BadgesComposer(Session));
        }
    }
}
