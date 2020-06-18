using System;
using System.Linq;
using System.Text;

using Neon.Communication.Packets.Incoming;
using Neon.HabboHotel.GameClients;
using Neon.Communication.Packets.Outgoing.Inventory.Purse;

namespace Neon.Communication.Packets.Incoming.Inventory.Purse
{
    class GetCreditsInfoEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
            Session.SendMessage(new ActivityPointsComposer(Session.GetHabbo().Duckets, Session.GetHabbo().Diamonds, Session.GetHabbo().GOTWPoints));
        }
    }
}
