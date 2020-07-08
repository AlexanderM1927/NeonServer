using Neon.Communication.Packets.Outgoing.Catalog;
using Neon.Communication.Packets.Outgoing.Inventory.Furni;
using Neon.Communication.Packets.Outgoing.Inventory.Purse;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.Database.Interfaces;
using Neon.HabboHotel.Catalog;
using Neon.HabboHotel.Items;
using System;
using System.Data;

namespace Neon.Communication.Packets.Incoming.Catalog
{
    internal class BuyTargettedOfferMessage : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)

        {
            #region RETURN VALUES
            TargetedOffers offer = NeonEnvironment.GetGame().GetTargetedOffersManager().TargetedOffer;
            HabboHotel.Users.Habbo habbo = Session.GetHabbo();
            if (offer == null || habbo == null)
            {
                Session.SendMessage(new PurchaseErrorComposer(1));
                return;
            }
            #endregion

            #region FIELDS
            Packet.PopInt();
            int amount = Packet.PopInt();
            if (amount > offer.Limit)
            {
                Session.SendMessage(new PurchaseErrorComposer(1));
                return;
            }
            int creditsCost = int.Parse(offer.Price[0]) * amount;
            int extraMoneyCost = int.Parse(offer.Price[1]) * amount;
            #endregion

            //#region CREDITS COST
            //if (creditsCost > 0)
            //{
            //    if (habbo.Credits < creditsCost)
            //    {
            //        Session.SendMessage(new PurchaseErrorComposer(1));
            //        return;
            //    }

            //    habbo.Credits -= creditsCost;
            //    Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits - creditsCost));
            //}
            //#endregion

            //#region EXTRA MONEY COST
            //if (extraMoneyCost > 0)
            //{
            //    #region GET MONEY TYPE AND DISCOUNT
            //    switch (offer.MoneyType)
            //    {
            //        #region DUCKETS COST
            //        case "duckets":
            //            {
            //                if (habbo.Duckets < extraMoneyCost)
            //                {
            //                    Session.SendMessage(new PurchaseErrorComposer(1));
            //                    return;
            //                }

            //                //habbo.Duckets -= extraMoneyCost;
            //                Session.GetHabbo().Duckets -= extraMoneyCost;
            //                Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Duckets, Session.GetHabbo().Duckets));
            //                break;
            //            }
            //        #endregion

            //        #region DIAMONDS COST
            //        case "diamonds":
            //            {
            //                if (habbo.Diamonds < extraMoneyCost)
            //                {
            //                    Session.SendMessage(new PurchaseErrorComposer(1));
            //                    return;
            //                }

            //                //habbo.Diamonds -= extraMoneyCost;
            //                Session.GetHabbo().Diamonds -= extraMoneyCost;
            //                Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, 0, 5));
            //                break;
            //            }
            //            #endregion

            //            //#region OTHER COST
            //            //default:
            //            //    goto case "duckets";
            //            //    #endregion
            //    }
            //    #endregion

            //    //habbo.UpdateExtraMoneyBalance();
            //}
            //#endregion

            #region BUY AND CREATE ITEMS PROGRESS
            TargetedOffers TargetedOffer = NeonEnvironment.GetGame().GetTargetedOffersManager().TargetedOffer;
            using (IQueryAdapter dbQuery = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbQuery.SetQuery("SELECT targeted_buy FROM users where id = " + habbo.Id + " LIMIT 1");
                DataTable count = dbQuery.getTable();
                foreach (DataRow Row in count.Rows)
                {
                    int offer2 = Convert.ToInt32(Row["targeted_buy"]);


                    if (TargetedOffer.Limit == offer2)
                    {
                        Session.SendMessage(new RoomCustomizedAlertComposer("Ya has pasado el limite de compras de esta Oferta."));
                    }

                    else

                    {
                        using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.runFastQuery("UPDATE users SET targeted_buy = targeted_buy +1 WHERE id = " + Session.GetHabbo().Id + ";");
                        }

                        foreach (TargetedItems product in offer.Products)
                        {
                            #region CHECK PRODUCT TYPE
                            switch (product.ItemType)
                            {
                                #region NORMAL ITEMS CASE
                                case "item":
                                    {
                                        if (!NeonEnvironment.GetGame().GetItemManager().GetItem(int.Parse(product.Item), out ItemData item))
                                        {
                                            return;
                                        }

                                        if (item == null)
                                        {
                                            return;
                                        }

                                        Item cItem = ItemFactory.CreateSingleItemNullable(item, Session.GetHabbo(), string.Empty, string.Empty);
                                        if (cItem != null)
                                        {
                                            Session.GetHabbo().GetInventoryComponent().TryAddItem(cItem);

                                            Session.SendMessage(new FurniListAddComposer(cItem));
                                            Session.SendMessage(new FurniListUpdateComposer());

                                        }

                                        Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                                        break;
                                    }
                                #endregion

                                #region BADGE CASE
                                case "badge":
                                    {
                                        if (habbo.GetBadgeComponent().HasBadge(product.Item))
                                        {
                                            //Session.SendMessage(new RoomCustomizedAlertComposer("mira men ahi te pudras joder"));
                                            //break;
                                        }

                                        habbo.GetBadgeComponent().GiveBadge(product.Item, true, Session);
                                        break;
                                    }
                                    #endregion
                            }
                            #endregion
                        }
                    }
                }
            }
            #endregion

            #region CREDITS COST
            if (creditsCost > 0)
            {
                if (habbo.Credits < creditsCost)
                {
                    Session.SendMessage(new PurchaseErrorComposer(1));
                    return;
                }

                habbo.Credits -= creditsCost;
                Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits - creditsCost));
            }
            #endregion

            #region EXTRA MONEY COST
            if (extraMoneyCost > 0)
            {
                #region GET MONEY TYPE AND DISCOUNT
                switch (offer.MoneyType)
                {
                    #region DUCKETS COST
                    case "duckets":
                        {
                            if (habbo.Duckets < extraMoneyCost)
                            {
                                Session.SendMessage(new PurchaseErrorComposer(1));
                                return;
                            }

                            //habbo.Duckets -= extraMoneyCost;
                            Session.GetHabbo().Duckets -= extraMoneyCost;
                            Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Duckets, Session.GetHabbo().Duckets));
                            break;
                        }
                    #endregion

                    #region DIAMONDS COST
                    case "diamonds":
                        {
                            if (habbo.Diamonds < extraMoneyCost)
                            {
                                Session.SendMessage(new PurchaseErrorComposer(1));
                                return;
                            }

                            //habbo.Diamonds -= extraMoneyCost;
                            Session.GetHabbo().Diamonds -= extraMoneyCost;
                            Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, 0, 5));
                            break;
                        }
                        #endregion

                        //#region OTHER COST
                        //default:
                        //    goto case "duckets";
                        //    #endregion
                }
                #endregion

                //habbo.UpdateExtraMoneyBalance();
            }
            #endregion

            #region RE-OPEN TARGETED BOX
            TargetedOffers TargetedOffer2 = NeonEnvironment.GetGame().GetTargetedOffersManager().TargetedOffer;

            int offer22 = Session.GetHabbo()._TargetedBuy;


            if (TargetedOffer2.Limit > offer22)
            {
                Session.SendMessage(NeonEnvironment.GetGame().GetTargetedOffersManager().TargetedOffer.Serialize());
            }
        }
    }
    #endregion
}