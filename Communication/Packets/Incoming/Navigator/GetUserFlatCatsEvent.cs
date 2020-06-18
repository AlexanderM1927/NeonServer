using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.Communication.Packets.Incoming;
using Neon.HabboHotel.Users;
using Neon.HabboHotel.Navigator;
using Neon.HabboHotel.GameClients;
using Neon.Communication.Packets.Outgoing.Navigator;

namespace Neon.Communication.Packets.Incoming.Navigator
{
    public class GetUserFlatCatsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null)
                return;

            ICollection<SearchResultList> Categories = NeonEnvironment.GetGame().GetNavigator().GetFlatCategories();

            Session.SendMessage(new UserFlatCatsComposer(Categories, Session.GetHabbo().Rank));
        }
    }
}