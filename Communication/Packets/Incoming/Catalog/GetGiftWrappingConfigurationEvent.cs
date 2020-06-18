using Neon.Communication.Packets.Outgoing.Catalog;
using Neon.HabboHotel.GameClients;
using Neon.Communication.Packets.Incoming;

namespace Neon.Communication.Packets.Incoming.Catalog
{
    public class GetGiftWrappingConfigurationEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new GiftWrappingConfigurationComposer());
        }
    }
}