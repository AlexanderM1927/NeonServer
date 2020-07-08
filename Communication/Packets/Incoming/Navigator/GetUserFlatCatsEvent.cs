using Neon.Communication.Packets.Outgoing.Navigator;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Navigator;
using System.Collections.Generic;

namespace Neon.Communication.Packets.Incoming.Navigator
{
    public class GetUserFlatCatsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null)
            {
                return;
            }

            ICollection<SearchResultList> Categories = NeonEnvironment.GetGame().GetNavigator().GetFlatCategories();

            Session.SendMessage(new UserFlatCatsComposer(Categories, Session.GetHabbo().Rank));
        }
    }
}