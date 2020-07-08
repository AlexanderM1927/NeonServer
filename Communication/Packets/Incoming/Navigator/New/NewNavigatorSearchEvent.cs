using Neon.Communication.Packets.Outgoing.Navigator;
using Neon.HabboHotel.Navigator;
using System.Collections.Generic;

namespace Neon.Communication.Packets.Incoming.Navigator
{
    internal class NewNavigatorSearchEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient session, ClientPacket packet)
        {
            string Category = packet.PopString();
            string Search = packet.PopString();

            ICollection<SearchResultList> Categories = new List<SearchResultList>();

            if (!string.IsNullOrEmpty(Search))
            {
                if (NeonEnvironment.GetGame().GetNavigator().TryGetSearchResultList(0, out SearchResultList QueryResult))
                {
                    Categories.Add(QueryResult);
                }
            }
            else
            {
                Categories = NeonEnvironment.GetGame().GetNavigator().GetCategorysForSearch(Category);
                if (Categories.Count == 0)
                {
                    Categories = NeonEnvironment.GetGame().GetNavigator().GetResultByIdentifier(Category);
                    if (Categories.Count > 0)
                    {
                        session.SendMessage(new NavigatorSearchResultSetComposer(Category, Search, Categories, session, 2, 100));
                        return;
                    }
                }
            }

            session.SendMessage(new NavigatorSearchResultSetComposer(Category, Search, Categories, session));
        }
    }
}
