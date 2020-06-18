using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Navigator;
using Neon.Communication.Packets.Outgoing.Navigator;

namespace Neon.Communication.Packets.Incoming.Navigator
{
    class InitializeNewNavigatorEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            ICollection<TopLevelItem> TopLevelItems = NeonEnvironment.GetGame().GetNavigator().GetTopLevelItems();
            ICollection<SearchResultList> SearchResultLists = NeonEnvironment.GetGame().GetNavigator().GetSearchResultLists();

            Session.SendMessage(new NavigatorMetaDataParserComposer(TopLevelItems));
            Session.SendMessage(new NavigatorLiftedRoomsComposer());
            Session.SendMessage(new NavigatorCollapsedCategoriesComposer());
            Session.SendMessage(new NavigatorPreferencesComposer());
        }
    }
}
