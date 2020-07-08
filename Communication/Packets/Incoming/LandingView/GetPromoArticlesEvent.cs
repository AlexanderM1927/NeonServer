using Neon.Communication.Packets.Outgoing.LandingView;
using Neon.HabboHotel.LandingView.Promotions;
using System.Collections.Generic;

namespace Neon.Communication.Packets.Incoming.LandingView
{
    internal class GetPromoArticlesEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            ICollection<Promotion> LandingPromotions = NeonEnvironment.GetGame().GetLandingManager().GetPromotionItems();

            Session.SendMessage(new PromoArticlesComposer(LandingPromotions));
        }
    }
}
