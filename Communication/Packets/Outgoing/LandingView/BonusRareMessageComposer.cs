using Neon.Communication.Packets.Outgoing.Inventory.Furni;
using Neon.Communication.Packets.Outgoing.Inventory.Purse;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Items;
using System;


namespace Neon.Communication.Packets.Outgoing.LandingView
{
    internal class BonusRareMessageComposer : ServerPacket
    {
        public BonusRareMessageComposer(GameClient Session)
            : base(ServerPacketHeader.BonusRareMessageComposer)
        {

            string product = NeonEnvironment.GetDBConfig().DBData["bonus_rare_productdata_name"];
            int baseid = int.Parse(NeonEnvironment.GetDBConfig().DBData["bonus_rare_item_baseid"]);
            int score = Convert.ToInt32(NeonEnvironment.GetDBConfig().DBData["bonus_rare_total_score"]);

            base.WriteString(product);
            base.WriteInteger(baseid);
            base.WriteInteger(score);
            base.WriteInteger(Session.GetHabbo().BonusPoints >= score ? score : score - Session.GetHabbo().BonusPoints); //Total To Gain
            if (Session.GetHabbo().BonusPoints >= score)
            {
                Session.GetHabbo().BonusPoints -= score;
                Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().BonusPoints, score, 101));
                Session.SendMessage(new RoomCustomizedAlertComposer("Has completado tu Bonus Rare ¡ya tienes tu premio en el inventario! Recibirás otro cuando vuelvas a acumular 120 puntos."));
                if (!NeonEnvironment.GetGame().GetItemManager().GetItem((baseid), out ItemData Item))
                {
                    // No existe este ItemId.
                    return;
                }

                Item GiveItem = ItemFactory.CreateSingleItemNullable(Item, Session.GetHabbo(), "", "");
                if (GiveItem != null)
                {
                    Session.GetHabbo().GetInventoryComponent().TryAddItem(GiveItem);

                    Session.SendMessage(new FurniListNotificationComposer(GiveItem.Id, 1));
                    Session.SendMessage(new FurniListUpdateComposer());
                }

                Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
            }
        }
    }
}
