﻿using Neon.Communication.Packets.Outgoing.Catalog;
using Neon.Communication.Packets.Outgoing.Inventory.Furni;
using Neon.Communication.Packets.Outgoing.Inventory.Purse;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.Catalog;
using Neon.HabboHotel.Items;
using Neon.HabboHotel.Rooms;
using System;
using System.Collections.Generic;

namespace Neon.Communication.Packets.Incoming.Rooms.Engine
{
    internal class PlaceBuilderObjectEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().InRoom)
            {
                return;
            }

            if (!NeonEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room Room))
            {
                return;
            }

            bool HasRights = false;
            if (Room.CheckRights(Session, false, true))
            {
                HasRights = true;
            }

            if (!HasRights)
            {
                Session.SendMessage(new RoomNotificationComposer("furni_placement_error", "message", "${room.error.cant_set_not_owner}"));
                return;
            }


            int PageId = Packet.PopInt();
            int ItemId = Packet.PopInt();

            if (!NeonEnvironment.GetGame().GetCatalog().TryGetBCPage(PageId, out BCCatalogPage Page))
            {
                return;
            }

            if (Session.GetHabbo().Rank > 8 && !Session.GetHabbo().StaffOk || NeonStaticGameSettings.IsGoingToBeClose)
            {
                return;
            }

            if (!Page.Enabled || !Page.Visible || Page.MinimumRank > Session.GetHabbo().Rank || (Page.MinimumVIP > Session.GetHabbo().VIPRank && Session.GetHabbo().Rank == 1))
            {
                return;
            }

            if (!Page.Items.TryGetValue(ItemId, out BCCatalogItem Item))
            {
                if (Page.ItemOffers.ContainsKey(ItemId))
                {
                    Item = Page.ItemOffers[ItemId];
                    if (Item == null)
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            ItemData baseItem = Item.GetBaseItem(Item.ItemId);

            if (Item.CostCredits > 0)
            {
                Session.GetHabbo().Credits -= Item.CostCredits;
                Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
            }

            if (Item.CostPixels > 0)
            {
                Session.GetHabbo().Duckets -= Item.CostPixels;
                Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Duckets, Session.GetHabbo().Duckets));
            }

            if (Item.CostDiamonds > 0)
            {
                Session.GetHabbo().Diamonds -= Item.CostDiamonds;
                Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, 0, 5));
            }

            if (Item.CostGOTWPoints > 0)
            {
                Session.GetHabbo().GOTWPoints -= Item.CostGOTWPoints;
                Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().GOTWPoints, 0, 103));
            }

            List<Item> GeneratedGenericItems = new List<Item>();

            List<Item> Items = ItemFactory.CreateMultipleItems(Item.Data, Session.GetHabbo(), "", 1);

            if (Items != null)
            {
                GeneratedGenericItems.AddRange(Items);
                //Session.SendMessage(RoomNotificationComposer.SendBubble("icons/" + Item.Data.ItemName + "_icon", "Acabas de comprar un/una " + Item.Data.PublicName + "", "inventory/open/furni"));
            }
            int newID = 0;
            foreach (Item PurchasedItem in GeneratedGenericItems)
            {
                if (Session.GetHabbo().GetInventoryComponent().TryAddItem(PurchasedItem))
                {
                    // Session.SendMessage(new FurniListNotificationComposer(PurchasedItem.Id, 1));
                    newID = PurchasedItem.Id;
                }
            }

            Session.SendMessage(new PurchaseOKComposer(Item, Item.Data));
            Session.SendMessage(new FurniListUpdateComposer());

            string Unknown = Packet.PopString();
            int X = Packet.PopInt();
            int Y = Packet.PopInt();
            int Rot = Packet.PopInt();
            Console.WriteLine(Unknown);


            Item RoomItem = new Item(newID, Room.RoomId, baseItem.Id, Item.ExtraData, X, Y, 0, Rot, Session.GetHabbo().Id, 0, 0, 0, string.Empty, Room);
            if (Room.GetRoomItemHandler().SetFloorItem(Session, RoomItem, X, Y, Rot, true, false, true))
            {
                Session.GetHabbo().GetInventoryComponent().RemoveItem(newID);

                if (Session.GetHabbo().Id == Room.OwnerId)
                {
                    NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_RoomDecoFurniCount", 1, false);
                }
            }
            else
            {
                Session.SendMessage(RoomNotificationComposer.SendBubble("furni_placement_error", "Lo siento, no tienes permiso para colocar furnis aquí", ""));
                return;
            }


            Console.WriteLine("Unknown: " + X + "|" + Y + "|" + Rot + "|");


        }
    }
}