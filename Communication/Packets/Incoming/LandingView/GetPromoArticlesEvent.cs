using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.LandingView;
using Neon.HabboHotel.LandingView.Promotions;
using Neon.Communication.Packets.Outgoing.LandingView;

namespace Neon.Communication.Packets.Incoming.LandingView
{
    class GetPromoArticlesEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            ICollection<Promotion> LandingPromotions = NeonEnvironment.GetGame().GetLandingManager().GetPromotionItems();

            Session.SendMessage(new PromoArticlesComposer(LandingPromotions));
        }
    }
}
