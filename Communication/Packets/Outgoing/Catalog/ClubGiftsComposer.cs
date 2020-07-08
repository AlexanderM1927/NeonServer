using Neon.HabboHotel.Club;
using Neon.HabboHotel.GameClients;
using System;

namespace Neon.Communication.Packets.Outgoing.Catalog
{
    internal class ClubGiftsComposer : ServerPacket
    {
        public ClubGiftsComposer(GameClient Session)
            : base(ServerPacketHeader.ClubGiftsMessageComposer)
        {
            Subscription Sub = Session.GetHabbo().GetClubManager().GetSubscription("club_vip");

            double TimeLeft = Sub.ExpireTime - NeonEnvironment.GetUnixTimestamp();
            int TotalDaysLeft = (int)Math.Ceiling(TimeLeft / 86400);
            base.WriteInteger(TotalDaysLeft); // Días hasta la siguiente recarga

            base.WriteInteger(Session.GetHabbo().GetStats().vipGifts); // Regalos Disponibles
            base.WriteInteger(2); // NOMBRE DE CADEAUX 
                                  //{
            base.WriteInteger(267);//SPRITE_ID DU MOBI QUE VOUS VOULEZ
            base.WriteString("club_sofa");//ITEM_NAME DU MOBI
            base.WriteBoolean(false);//
            base.WriteInteger(5);//credits
            base.WriteInteger(0);//nombre de jetons
            base.WriteInteger(0);//type de la monnaie
            base.WriteBoolean(false);//Pouvoir offrir?
            base.WriteInteger(1); // Nombre de cadeaux à remettre
                                  //{
            base.WriteString("s");//type du mobi, s;i;b;r etc...
            base.WriteInteger(267);//SPRITE ID du mobi
            base.WriteString("");//ExtraData du mobi si vous voulez customiser un peu tout ça
            base.WriteInteger(1);//Nombre de mobi dans l'offre!
            base.WriteBoolean(false);//
                                     //}
            base.WriteInteger(0);//0 = TOUT LE MONDE, 1= MEMBRE HABBO CLUB
            base.WriteBoolean(false);//Offer?
            base.WriteBoolean(false);
            base.WriteString(string.Empty);
            /////////////////////////////////////////////////////////////////////////////////////////
            base.WriteInteger(230);//SPRITE_ID DU MOBI QUE VOUS VOULEZ
            base.WriteString("a0 throne");//ITEM_NAME DU MOBI
            base.WriteBoolean(false);//
            base.WriteInteger(5);//credits
            base.WriteInteger(0);//nombre de jetons
            base.WriteInteger(0);//type de la monnaie
            base.WriteBoolean(false);//Pouvoir offrir?
            base.WriteInteger(1); // Nombre de cadeaux à remettre
                                  //{
            base.WriteString("s");//type du mobi, s;i;b;r etc...
            base.WriteInteger(230);//SPRITE ID du mobi
            base.WriteString("");//ExtraData du mobi si vous voulez customiser un peu tout ça
            base.WriteInteger(1);//Nombre de mobi dans l'offre!
            base.WriteBoolean(false);//
                                     //}
            base.WriteInteger(0);//0 = TOUT LE MONDE, 1= MEMBRE HABBO CLUB
            base.WriteBoolean(false);//Offer?
            base.WriteBoolean(false);
            base.WriteString(string.Empty);
            //}

            base.WriteInteger(2);//Nombre de cadeaux à remettre
                                 //{
                                 //int, bool, int, bool
            base.WriteInteger(267);//SPRITE ID du mobi
            base.WriteBoolean(true);//On peut prendre ?
            base.WriteInteger(-100);//idk
            base.WriteBoolean(true);//idk

            base.WriteInteger(230);//SPRITE ID du mobi
            base.WriteBoolean(true);//On peut prendre ?
            base.WriteInteger(-100);//idk
            base.WriteBoolean(true);//idk
            //}
        }
    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Neon.Communication.Packets.Outgoing.Catalog
//{
//    class ClubGiftsComposer : ServerPacket
//    {
//        public ClubGiftsComposer()
//            : base(ServerPacketHeader.ClubGiftsMessageComposer)
//        {
//            base.WriteInteger(231);//Days until next gift. base.WriteInteger(0); //Gifts available
//            base.WriteInteger(1);//Count?

//            base.WriteInteger(1);//Count?
//                                 /*
//                                  *
//                                  * this._-1L8 = _arg1._-1u3();
//                                 this._-1iD = _arg1.readString();
//                                 this._-2zc = _arg1.readBoolean();
//                                 this._-6i = _arg1._-1u3();
//                                 this._-4-j = _arg1._-1u3();
//                                 this._-3h3 = _arg1._-1u3();
//                                 this._-0jj = _arg1.readBoolean();
//                                 var k:int = _arg1._-1u3();
//                                 this._-0zK = new <_-0YF>[];
//                                 var k:int;
//                                 while (k < k)
//                                 {
//                                     this._-0zK.push(new _-0YF(_arg1));
//                                     k++;
//                                 };
//                                 this._-rM = _arg1._-1u3();
//                                 this._-5dA = _arg1.readBoolean();
//                                 this._-0jf = _arg1.readBoolean();
//                                 this._-5L5 = _arg1.readString();
//                                  */

//            base.WriteInteger(202); // OFFER ID
//            base.WriteString("PENES Y POLLAS"); // OFFER NAME
//            base.WriteBoolean(false); // OFFER RENT OR BUY
//            base.WriteInteger(0); // CREDIT COST $$
//            base.WriteInteger(0); // ACTIVITY POINTS XD
//            base.WriteInteger(0); // activityPointType
//            base.WriteBoolean(false);

//            base.WriteInteger(1);//Count for some reason
//            {
//                base.WriteString("b");
//                base.WriteString("ADM");
//                //base.WriteInteger(8228);
//                //base.WriteString("");
//                //base.WriteInteger(1);
//                //base.WriteBoolean(true);
//            }
//            base.WriteInteger(0);
//            base.WriteBoolean(false);
//            base.WriteBoolean(true);
//            base.WriteString("");


//            base.WriteInteger(1);

//            base.WriteInteger(202);//Maybe the item id?


//            base.WriteBoolean(true);//Can we get?
//            base.WriteInteger(256);//idk
//            base.WriteBoolean(true);//idk
//            base.WriteInteger(1);
//            base.WriteBoolean(true);


//            //{
//            //    base.WriteInteger(12701);
//            //    base.WriteString("hc16_1");
//            //    base.WriteBoolean(false);
//            //    base.WriteInteger(1);
//            //    base.WriteInteger(0);
//            //    base.WriteInteger(0);
//            //    base.WriteBoolean(true);
//            //    base.WriteInteger(1);//Count for some reason
//            //    {
//            //        base.WriteString("s");
//            //        base.WriteInteger(8228);
//            //        base.WriteString("");
//            //        base.WriteInteger(1);
//            //        base.WriteBoolean(false);
//            //    }

//            //    //base.WriteInteger(0);
//            //    //base.WriteBoolean(true);
//            //}


//            //base.WriteInteger(0);//Count
//            //{
//            //    //int, bool, int, bool
//            //    base.WriteInteger(3253248);//Maybe the item id?


//            //    base.WriteBoolean(false);//Can we get?
//            //    base.WriteInteger(256);//idk
//            //    base.WriteBoolean(false);//idk
//            //    base.WriteInteger(0);
//            //    base.WriteBoolean(true);


//            //}
//        }
//    }
//}


