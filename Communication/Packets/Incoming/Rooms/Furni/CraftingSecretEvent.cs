﻿using Neon.Communication.Packets.Outgoing.Inventory.Furni;
using Neon.Communication.Packets.Outgoing.Rooms.Furni;
using Neon.HabboHotel.Items;
using Neon.HabboHotel.Items.Crafting;
using Neon.HabboHotel.Rooms;
using System.Collections.Generic;
using System.Linq;

namespace Neon.Communication.Packets.Incoming.Rooms.Furni
{
    internal class CraftSecretEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int craftingTable = Packet.PopInt();
            //int itemCount = Packet.PopInt();

            //int[] myItems = new int[itemCount];
            //for (int i = 0; i < itemCount; itemCount++)
            //{
            //    int ItemID = Packet.PopInt();
            //    Item InventoryItem = Session.GetHabbo().GetInventoryComponent().GetItem(ItemID);
            //    if (InventoryItem == null)
            //        continue;

            //    myItems[i] = InventoryItem.BaseItem;
            //}

            List<Item> items = new List<Item>();

            int count = Packet.PopInt();
            for (int i = 1; i <= count; i++)
            {
                int id = Packet.PopInt();

                Item item = Session.GetHabbo().GetInventoryComponent().GetItem(id);
                if (item == null || items.Contains(item))
                {
                    return;
                }

                items.Add(item);
            }

            CraftingRecipe recipe = null;
            foreach (KeyValuePair<string, CraftingRecipe> Receta in NeonEnvironment.GetGame().GetCraftingManager().CraftingRecipes)
            {
                bool found = false;

                foreach (KeyValuePair<string, int> item in Receta.Value.ItemsNeeded)
                {
                    if (item.Value != items.Count(item2 => item2.GetBaseItem().ItemName == item.Key))
                    {
                        found = false;
                        break;
                    }

                    found = true;
                }

                if (found == false)
                {
                    continue;
                }

                recipe = Receta.Value;
                break;
            }

            if (recipe == null)
            {
                return;
            }

            ItemData resultItem = NeonEnvironment.GetGame().GetItemManager().GetItemByName(recipe.Result);
            if (resultItem == null)
            {
                return;
            }

            bool success = true;
            foreach (KeyValuePair<string, int> need in recipe.ItemsNeeded)
            {
                for (int i = 1; i <= need.Value; i++)
                {
                    ItemData item = NeonEnvironment.GetGame().GetItemManager().GetItemByName(need.Key);
                    if (item == null)
                    {
                        success = false;
                        continue;
                    }

                    Item inv = Session.GetHabbo().GetInventoryComponent().GetFirstItemByBaseId(item.Id);
                    if (inv == null)
                    {
                        success = false;
                        continue;
                    }

                    using (Database.Interfaces.IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("DELETE FROM `items` WHERE `id` = '" + inv.Id + "' AND `user_id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                    }

                    Session.GetHabbo().GetInventoryComponent().RemoveItem(inv.Id);
                }
            }

            Session.GetHabbo().GetInventoryComponent().UpdateItems(true);

            if (success)
            {
                Session.GetHabbo().GetInventoryComponent().AddNewItem(0, resultItem.Id, "", 0, true, false, 0, 0);
                Session.SendMessage(new FurniListUpdateComposer());
                Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                //Session.SendMessage(new CraftableProductsComposer());

                switch (recipe.Type)
                {
                    case 1:
                        NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_CrystalCracker", 1);
                        break;

                    case 2:
                        NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_PetLover", 1);
                        break;

                    case 3:
                        NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_PetLover", 1);
                        break;
                }
            }

            Session.SendMessage(new CraftingResultComposer(recipe, success));

            Room room = Session.GetHabbo().CurrentRoom;
            Item table = room.GetRoomItemHandler().GetItem(craftingTable);

            Session.SendMessage(new CraftableProductsComposer(table));
        }
    }
}