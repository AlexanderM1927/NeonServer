using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.LandingView;
using Neon.HabboHotel.LandingView.Promotions;


namespace Neon.Communication.Packets.Outgoing.LandingView
{
    class PromoArticlesComposer : ServerPacket
    {
        public PromoArticlesComposer(ICollection<Promotion> LandingPromotions)
            : base(ServerPacketHeader.PromoArticlesMessageComposer)
        {
            base.WriteInteger(LandingPromotions.Count);//Count
            foreach (Promotion Promotion in LandingPromotions.ToList())
            {
                base.WriteInteger(Promotion.Id); //ID
                base.WriteString(Promotion.Title); //Title
                base.WriteString(Promotion.Text); //Text
                base.WriteString(Promotion.ButtonText); //Button text
                base.WriteInteger(Promotion.ButtonType); //Link type 0 and 3
                base.WriteString(Promotion.ButtonLink); //Link to article
                base.WriteString(Promotion.ImageLink); //Image link
            }
        }
    }
}