﻿using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.Items;
using Neon.HabboHotel.Items.Data.Moodlight;
using Neon.HabboHotel.Items.Data.Toner;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Users;
using System;
using System.Linq;

namespace Neon.Communication.Packets.Incoming.Rooms.Engine
{
    internal class PlaceObjectEvent : IPacketEvent
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

            string RawData = Packet.PopString();
            string[] Data = RawData.Split(' ');
            if (!int.TryParse(Data[0], out int ItemId))
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

            Item Item = Session.GetHabbo().GetInventoryComponent().GetItem(ItemId);
            if (Item == null)
            {
                return;
            }

            if (Room.ForSale)
            {
                Session.SendWhisper("No se puede editar la Sala mientras está a la venta.");
                Session.SendWhisper("Cancela la Venta de la Sala expulsando a todos escribe ':unload' (sin las '')");
                return;
            }

            if (Room.GetRoomItemHandler().GetWallAndFloor.Count() > NeonStaticGameSettings.RoomFurnitureLimit)
            {
                Session.SendNotification("no se puede tener mas de " + NeonStaticGameSettings.RoomFurnitureLimit + " furnis en una sala!");
                return;
            }
            else if (Item.GetBaseItem().ItemName.ToLower().Contains("cf") && Room.OwnerId != Session.GetHabbo().Id && !Session.GetHabbo().GetPermissions().HasRight("room_item_place_exchange_anywhere"))
            {
                Session.SendNotification("No se puede colocar monedas canjeables en esta sala!");
                return;
            }

            //TODO: Make neat.
            switch (Item.GetBaseItem().InteractionType)
            {
                #region Interaction Types
                case InteractionType.MOODLIGHT:
                    {
                        MoodlightData moodData = Room.MoodlightData;
                        if (moodData != null && Room.GetRoomItemHandler().GetItem(moodData.ItemId) != null)
                        {
                            Session.SendNotification("Solo puedes tener un (1) regulador por sala!");
                            return;
                        }
                        break;
                    }
                case InteractionType.TONER:
                    {
                        TonerData tonerData = Room.TonerData;
                        if (tonerData != null && Room.GetRoomItemHandler().GetItem(tonerData.ItemId) != null)
                        {
                            Session.SendNotification("Solo puedes tener un (1) pinta fondo por sala!");
                            return;
                        }
                        break;
                    }
                case InteractionType.HOPPER:
                    {
                        if (Room.GetRoomItemHandler().HopperCount > 0)
                        {
                            Session.SendNotification("Solo puedes tener un (1) SaltaSalas en esta habitacion!");
                            return;
                        }
                        break;
                    }

                case InteractionType.TENT:
                case InteractionType.TENT_SMALL:
                    {
                        Room.AddTent(Item.Id);
                        break;
                    }
                    #endregion
            }

            if (!Item.IsWallItem)
            {
                if (Data.Length < 4)
                {
                    return;
                }

                if (!int.TryParse(Data[1], out int X)) { return; }
                if (!int.TryParse(Data[2], out int Y)) { return; }
                if (!int.TryParse(Data[3], out int Rotation)) { return; }

                Item RoomItem = new Item(Item.Id, Room.RoomId, Item.BaseItem, Item.ExtraData, X, Y, 0, Rotation, Session.GetHabbo().Id, Item.GroupId, Item.LimitedNo, Item.LimitedTot, string.Empty, Room);
                if (Room.GetRoomItemHandler().SetFloorItem(Session, RoomItem, X, Y, Rotation, true, false, true))
                {
                    Session.GetHabbo().GetInventoryComponent().RemoveItem(ItemId);

                    if (Session.GetHabbo().Id == Room.OwnerId)
                    {
                        NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_RoomDecoFurniCount", 1, false);
                    }

                    if (RoomItem.IsWired)
                    {
                        try { Room.GetWired().LoadWiredBox(RoomItem); }
                        catch { Console.WriteLine(Item.GetBaseItem().InteractionType); }
                    }
                }
                else
                {
                    Session.SendMessage(new RoomNotificationComposer("furni_placement_error", "message", "${room.error.cant_set_item}"));
                    return;
                }
            }
            else if (Item.IsWallItem)
            {
                string[] CorrectedData = new string[Data.Length - 1];

                for (int i = 1; i < Data.Length; i++)
                {
                    CorrectedData[i - 1] = Data[i];
                }

                if (TrySetWallItem(Session.GetHabbo(), Item, CorrectedData, out string WallPos))
                {
                    try
                    {
                        Item RoomItem = new Item(Item.Id, Room.RoomId, Item.BaseItem, Item.ExtraData, 0, 0, 0, 0, Session.GetHabbo().Id, Item.GroupId, Item.LimitedNo, Item.LimitedTot, WallPos, Room);

                        if (Room.GetRoomItemHandler().SetWallItem(Session, RoomItem))
                        {
                            Session.GetHabbo().GetInventoryComponent().RemoveItem(ItemId);
                            if (Session.GetHabbo().Id == Room.OwnerId)
                            {
                                NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_RoomDecoFurniCount", 1, false);
                            }
                        }
                    }
                    catch
                    {
                        Session.SendMessage(new RoomNotificationComposer("furni_placement_error", "message", "${room.error.cant_set_item}"));
                        return;
                    }
                }
                else
                {
                    Session.SendMessage(new RoomNotificationComposer("furni_placement_error", "message", "${room.error.cant_set_item}"));
                    return;
                }
            }
        }

        private static bool TrySetWallItem(Habbo Habbo, Item item, string[] data, out string position)
        {
            if (data.Length != 3 || !data[0].StartsWith(":w=") || !data[1].StartsWith("l=") || (data[2] != "r" && data[2] != "l"))
            {
                position = null;
                return false;
            }

            string wBit = data[0].Substring(3, data[0].Length - 3);
            string lBit = data[1].Substring(2, data[1].Length - 2);

            if (!wBit.Contains(",") || !lBit.Contains(","))
            {
                position = null;
                return false;
            }

            int.TryParse(wBit.Split(',')[0], out int w1);
            int.TryParse(wBit.Split(',')[1], out int w2);
            int.TryParse(lBit.Split(',')[0], out int l1);
            int.TryParse(lBit.Split(',')[1], out int l2);

            /*if (!Habbo.HasFuse("super_admin") && (w1 < 0 || w2 < 0 || l1 < 0 || l2 < 0 || w1 > 200 || w2 > 200 || l1 > 200 || l2 > 200))
            {
                position = null;
                return false;
            }*/



            string WallPos = ":w=" + w1 + "," + w2 + " l=" + l1 + "," + l2 + " " + data[2];

            position = WallPositionCheck(WallPos);

            return (position != null);
        }

        public static string WallPositionCheck(string wallPosition)
        {
            //:w=3,2 l=9,63 l
            try
            {
                if (wallPosition.Contains(Convert.ToChar(13)))
                {
                    return null;
                }
                if (wallPosition.Contains(Convert.ToChar(9)))
                {
                    return null;
                }

                string[] posD = wallPosition.Split(' ');
                if (posD[2] != "l" && posD[2] != "r")
                {
                    return null;
                }

                string[] widD = posD[0].Substring(3).Split(',');
                int widthX = int.Parse(widD[0]);
                int widthY = int.Parse(widD[1]);
                if (widthX < -1000 || widthY < -1 || widthX > 700 || widthY > 700)
                {
                    return null;
                }

                string[] lenD = posD[1].Substring(2).Split(',');
                int lengthX = int.Parse(lenD[0]);
                int lengthY = int.Parse(lenD[1]);
                if (lengthX < -1 || lengthY < -1000 || lengthX > 700 || lengthY > 700)
                {
                    return null;
                }

                return ":w=" + widthX + "," + widthY + " " + "l=" + lengthX + "," + lengthY + " " + posD[2];
            }
            catch
            {
                return null;
            }
        }
    }
}