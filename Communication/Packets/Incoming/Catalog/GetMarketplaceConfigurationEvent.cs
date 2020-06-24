using Neon.Communication.Packets.Outgoing.Catalog;
using Neon.HabboHotel.GameClients;

namespace Neon.Communication.Packets.Incoming.Catalog
{
    public class GetMarketplaceConfigurationEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new MarketplaceConfigurationComposer());
        }
    }
}