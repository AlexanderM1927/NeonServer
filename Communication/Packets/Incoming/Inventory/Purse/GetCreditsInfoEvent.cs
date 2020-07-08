using Neon.Communication.Packets.Outgoing.Inventory.Purse;
using Neon.HabboHotel.GameClients;

namespace Neon.Communication.Packets.Incoming.Inventory.Purse
{
    internal class GetCreditsInfoEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
            Session.SendMessage(new ActivityPointsComposer(Session.GetHabbo().Duckets, Session.GetHabbo().Diamonds, Session.GetHabbo().GOTWPoints));
        }
    }
}
