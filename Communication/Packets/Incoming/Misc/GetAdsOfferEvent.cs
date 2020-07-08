using Neon.Communication.Packets.Outgoing.Handshake;
using Neon.HabboHotel.GameClients;

namespace Neon.Communication.Packets.Incoming.Misc
{
    internal class GetAdsOfferEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new VideoOffersRewardsComposer());
        }
    }
}
