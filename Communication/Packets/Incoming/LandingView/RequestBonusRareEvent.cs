
using Neon.Communication.Packets.Outgoing.LandingView;

namespace Neon.Communication.Packets.Incoming.LandingView
{
    internal class RequestBonusRareEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new BonusRareMessageComposer(Session));
        }
    }
}
