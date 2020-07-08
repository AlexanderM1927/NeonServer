using Neon.Communication.Packets.Outgoing.Navigator;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Navigator;
using System.Collections.Generic;

namespace Neon.Communication.Packets.Incoming.Navigator
{
    internal class GetNavigatorFlatsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            ICollection<SearchResultList> Categories = NeonEnvironment.GetGame().GetNavigator().GetEventCategories();

            Session.SendMessage(new NavigatorFlatCatsComposer(Categories, Session.GetHabbo().Rank));
        }
    }
}