﻿using Neon.Communication.Packets.Outgoing;
using Neon.Communication.Packets.Outgoing.Inventory.Furni;
using Neon.Communication.Packets.Outgoing.Rooms.Engine;
using Neon.Communication.Packets.Outgoing.Rooms.Furni;
using Neon.Core;
using Neon.Database.Interfaces;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Items;
using Neon.HabboHotel.Items.Data.Moodlight;
using Neon.HabboHotel.Items.Data.Toner;
using Neon.HabboHotel.Items.Wired;
using Neon.HabboHotel.Pathfinding;
using Neon.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace Neon.HabboHotel.Rooms
{
    public class RoomItemHandling
    {
        private Room _room;

        public int HopperCount;
        private int mRollerSpeed;
        private int mRollerCycle;

        private ConcurrentDictionary<int, Item> _movedItems;

        private readonly ConcurrentDictionary<int, Item> _rollers;
        private ConcurrentDictionary<int, Item> _wallItems = null;
        private ConcurrentDictionary<int, Item> _floorItems = null;

        private readonly List<int> rollerItemsMoved;
        private readonly List<int> rollerUsersMoved;
        private readonly List<ServerPacket> rollerMessages;

        private ConcurrentQueue<Item> _roomItemUpdateQueue;
        public bool usedwiredscorebord;

        public RoomItemHandling(Room Room)
        {
            _room = Room;

            HopperCount = 0;
            GotRollers = false;
            mRollerSpeed = Room.RollerSpeed;
            mRollerCycle = 0;

            _movedItems = new ConcurrentDictionary<int, Item>();

            _rollers = new ConcurrentDictionary<int, Item>();
            _wallItems = new ConcurrentDictionary<int, Item>();
            _floorItems = new ConcurrentDictionary<int, Item>();

            rollerItemsMoved = new List<int>();
            rollerUsersMoved = new List<int>();
            rollerMessages = new List<ServerPacket>();

            _roomItemUpdateQueue = new ConcurrentQueue<Item>();
            usedwiredscorebord = false;
        }

        public void TryAddRoller(int ItemId, Item Roller)
        {
            _rollers.TryAdd(ItemId, Roller);
        }

        public bool GotRollers { get; set; }

        public void QueueRoomItemUpdate(Item item)
        {
            _roomItemUpdateQueue.Enqueue(item);
        }

        public void SetSpeed(int p)
        {
            mRollerSpeed = p;
        }

        public void UpdateWiredScoreBord()
        {
            List<ServerPacket> messages = new List<ServerPacket>();
            foreach (Item scoreitem in _floorItems.Values)
            {
                if (scoreitem.GetBaseItem().InteractionType == InteractionType.wired_score_board || scoreitem.GetBaseItem().InteractionType == InteractionType.wired_casino)
                {
                    ObjectUpdateComposer Message = new ObjectUpdateComposer(scoreitem, _room.OwnerId);
                    messages.Add(Message);
                }
            }
            _room.SendMessage(messages);
        }

        internal void ScorebordChangeCheck()
        {
            if (_room.WiredScoreFirstBordInformation.Count == 3)
            {
                DateTime now = DateTime.Now;
                int getdaytoday = Convert.ToInt32(now.ToString("MMddyyyy"));
                int getmonthtoday = Convert.ToInt32(DateTime.Now.ToString("MM"));
                int getweektoday = CultureInfo.GetCultureInfo("Nl-nl").Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                List<bool> SuperCheck = new List<bool>()
                {
                    getdaytoday != _room.WiredScoreFirstBordInformation[0],
                    getmonthtoday != _room.WiredScoreFirstBordInformation[1],
                    getweektoday != _room.WiredScoreFirstBordInformation[2]
                };

                _room.WiredScoreFirstBordInformation[0] = getdaytoday;
                _room.WiredScoreFirstBordInformation[1] = getmonthtoday;
                _room.WiredScoreFirstBordInformation[2] = getweektoday;

                if (SuperCheck[0])
                {
                    _room.WiredScoreBordDay.Clear();
                }

                if (SuperCheck[1])
                {
                    _room.WiredScoreBordMonth.Clear();
                }

                if (SuperCheck[2])
                {
                    _room.WiredScoreBordWeek.Clear();
                }
            }
        }

        public string WallPositionCheck(string wallPosition)
        {
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

        public void LoadFurniture()
        {
            if (_floorItems.Count > 0)
            {
                _floorItems.Clear();
            }

            if (_wallItems.Count > 0)
            {
                _wallItems.Clear();
            }

            List<Item> Items = ItemLoader.GetItemsForRoom(_room.Id, _room);
            foreach (Item Item in Items)
            {
                if (Item == null)
                {
                    continue;
                }

                if (Item.UserID == 0)
                {
                    using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("UPDATE `items` SET `user_id` = @UserId WHERE `id` = @ItemId LIMIT 1");
                        dbClient.AddParameter("ItemId", Item.UserID);
                        dbClient.AddParameter("UserId", _room.OwnerId);
                        dbClient.RunQuery();
                    }
                }

                if (Item.IsFloorItem)
                {
                    if (!_room.GetGameMap().ValidTile(Item.GetX, Item.GetY))
                    {
                        using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.RunQuery("UPDATE `items` SET `room_id` = '0' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                        }

                        GameClient Client = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(Item.UserID);
                        if (Client != null)
                        {
                            Client.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, Item.BaseItem, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                            Client.GetHabbo().GetInventoryComponent().UpdateItems(false);
                        }
                        continue;
                    }

                    if (!_floorItems.ContainsKey(Item.Id))
                    {
                        _floorItems.TryAdd(Item.Id, Item);
                    }
                }
                else if (Item.IsWallItem)
                {
                    if (string.IsNullOrWhiteSpace(Item.wallCoord))
                    {
                        using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `items` SET `wall_pos` = @WallPosition WHERE `id` = '" + Item.Id + "' LIMIT 1");
                            dbClient.AddParameter("WallPosition", ":w=0,2 l=11,53 l");
                            dbClient.RunQuery();
                        }

                        Item.wallCoord = ":w=0,2 l=11,53 l";
                    }

                    try
                    {
                        Item.wallCoord = WallPositionCheck(":" + Item.wallCoord.Split(':')[1]);
                    }
                    catch
                    {
                        using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `items` SET `wall_pos` = @WallPosition WHERE `id` = '" + Item.Id + "' LIMIT 1");
                            dbClient.AddParameter("WallPosition", ":w=0,2 l=11,53 l");
                            dbClient.RunQuery();
                        }

                        Item.wallCoord = ":w=0,2 l=11,53 l";
                    }

                    if (!_wallItems.ContainsKey(Item.Id))
                    {
                        _wallItems.TryAdd(Item.Id, Item);
                    }
                }
            }


            foreach (Item Item in _floorItems.Values)
            {
                if (Item.IsRoller)
                {
                    GotRollers = true;
                }
                else if (Item.GetBaseItem().InteractionType == InteractionType.MOODLIGHT)
                {
                    if (_room.MoodlightData == null)
                    {
                        _room.MoodlightData = new MoodlightData(Item.Id);
                    }
                }
                else if (Item.GetBaseItem().InteractionType == InteractionType.TONER)
                {
                    if (_room.TonerData == null)
                    {
                        _room.TonerData = new TonerData(Item.Id);
                    }
                }
                else if (Item.IsWired)
                {
                    if (_room == null)
                    {
                        continue;
                    }

                    if (_room.GetWired() == null)
                    {
                        continue;
                    }

                    _room.GetWired().LoadWiredBox(Item);
                }
                else if (Item.GetBaseItem().InteractionType == InteractionType.HOPPER)
                {
                    HopperCount++;
                }
            }
        }

        public Item GetItem(int pId)
        {
            if (_floorItems != null && _floorItems.ContainsKey(pId))
            {
                if (_floorItems.TryGetValue(pId, out Item Item))
                {
                    return Item;
                }
            }
            else if (_wallItems != null && _wallItems.ContainsKey(pId))
            {
                if (_wallItems.TryGetValue(pId, out Item Item))
                {
                    return Item;
                }
            }

            return null;
        }

        public void RemoveFurniture(GameClient Session, int pId, bool WasPicked = true)
        {
            Item Item = GetItem(pId);
            if (Item == null)
            {
                return;
            }

            if (Item.GetBaseItem().InteractionType == InteractionType.FOOTBALL_GATE)
            {
                _room.GetSoccer().UnRegisterGate(Item);
            }

            if (Item.GetBaseItem().InteractionType != InteractionType.GIFT)
            {
                Item.Interactor.OnRemove(Session, Item);
            }

            if (Item.GetBaseItem().InteractionType == InteractionType.GUILD_GATE)
            {
                Item.UpdateCounter = 0;
                Item.UpdateNeeded = false;
            }

            if (Item.GetBaseItem().InteractionType == InteractionType.HCGATE)
            {
                Item.UpdateCounter = 0;
                Item.UpdateNeeded = false;
            }

            if (Item.GetBaseItem().InteractionType == InteractionType.VIPGATE)
            {
                Item.UpdateCounter = 0;
                Item.UpdateNeeded = false;
            }

            RemoveRoomItem(Item);
        }

        private void RemoveRoomItem(Item Item)
        {
            if (Item.IsFloorItem)
            {
                _room.SendMessage(new ObjectRemoveComposer(Item, Item.UserID));
            }
            else if (Item.IsWallItem)
            {
                _room.SendMessage(new ItemRemoveComposer(Item, Item.UserID));
            }

            if (Item.IsWallItem)
            {
                _wallItems.TryRemove(Item.Id, out Item);
            }
            else
            {
                _floorItems.TryRemove(Item.Id, out Item);
                //mFloorItems.OnCycle();
                _room.GetGameMap().RemoveFromMap(Item);
            }

            RemoveItem(Item);
            _room.GetGameMap().GenerateMaps();
            _room.GetRoomUserManager().UpdateUserStatusses();
        }

        private List<ServerPacket> CycleRollers()
        {
            if (!GotRollers)
            {
                return new List<ServerPacket>();
            }

            if (mRollerCycle >= mRollerSpeed || mRollerSpeed == 0)
            {
                rollerItemsMoved.Clear();
                rollerUsersMoved.Clear();
                rollerMessages.Clear();

                List<Item> ItemsOnRoller;
                List<Item> ItemsOnNext;

                foreach (Item Roller in _rollers.Values)
                {
                    if (Roller == null)
                    {
                        continue;
                    }

                    Point NextSquare = Roller.SquareInFront;

                    ItemsOnRoller = _room.GetGameMap().GetRoomItemForSquare(Roller.GetX, Roller.GetY, Roller.GetZ);
                    ItemsOnNext = _room.GetGameMap().GetAllRoomItemForSquare(NextSquare.X, NextSquare.Y).ToList();

                    if (ItemsOnRoller.Count > 10)
                    {
                        ItemsOnRoller = _room.GetGameMap().GetRoomItemForSquare(Roller.GetX, Roller.GetY, Roller.GetZ).Take(10).ToList();
                    }

                    bool NextSquareIsRoller = (ItemsOnNext.Where(x => x.GetBaseItem().InteractionType == InteractionType.ROLLER).Count() > 0);
                    bool NextRollerClear = true;

                    double NextZ = 0.0;
                    bool NextRoller = false;

                    foreach (Item Item in ItemsOnNext)
                    {
                        if (Item.IsRoller)
                        {
                            if (Item.TotalHeight > NextZ)
                            {
                                NextZ = Item.TotalHeight;
                            }

                            NextRoller = true;
                        }
                    }

                    if (NextRoller)
                    {
                        foreach (Item Item in ItemsOnNext)
                        {
                            if (Item.TotalHeight > NextZ)
                            {
                                NextRollerClear = false;
                            }
                        }
                    }

                    if (ItemsOnRoller.Count > 0)
                    {
                        foreach (Item rItem in ItemsOnRoller)
                        {
                            if (rItem == null)
                            {
                                continue;
                            }

                            if (!rollerItemsMoved.Contains(rItem.Id) && _room.GetGameMap().CanRollItemHere(NextSquare.X, NextSquare.Y) && NextRollerClear && Roller.GetZ < rItem.GetZ && _room.GetRoomUserManager().GetUserForSquare(NextSquare.X, NextSquare.Y) == null)
                            {
                                if (!NextSquareIsRoller)
                                {
                                    NextZ = rItem.GetZ - Roller.GetBaseItem().Height;
                                }
                                else
                                {
                                    NextZ = rItem.GetZ;
                                }

                                rollerMessages.Add(UpdateItemOnRoller(rItem, NextSquare, Roller.Id, NextZ));
                                rollerItemsMoved.Add(rItem.Id);
                            }
                        }
                    }

                    RoomUser RollerUser = _room.GetGameMap().GetRoomUsers(Roller.Coordinate).FirstOrDefault();

                    if (RollerUser != null && !RollerUser.IsWalking && NextRollerClear && _room.GetGameMap().IsValidStep(new Vector2D(Roller.GetX, Roller.GetY), new Vector2D(NextSquare.X, NextSquare.Y), true, false, false, true) && _room.GetGameMap().CanRollItemHere(NextSquare.X, NextSquare.Y) && _room.GetGameMap().GetFloorStatus(NextSquare) != 0)
                    {
                        if (!rollerUsersMoved.Contains(RollerUser.HabboId))
                        {
                            if (!NextSquareIsRoller)
                            {
                                NextZ = RollerUser.Z - Roller.GetBaseItem().Height;
                            }
                            else
                            {
                                NextZ = RollerUser.Z;
                            }

                            RollerUser.isRolling = true;
                            RollerUser.rollerDelay = 1;

                            rollerMessages.Add(UpdateUserOnRoller(RollerUser, NextSquare, Roller.Id, NextZ));
                            rollerUsersMoved.Add(RollerUser.HabboId);
                        }
                    }
                }

                mRollerCycle = 0;
                return rollerMessages;
            }
            else
            {
                mRollerCycle++;
            }

            return new List<ServerPacket>();
        }

        public ServerPacket UpdateItemOnRoller(Item pItem, Point NextCoord, int pRolledID, double NextZ)
        {
            ServerPacket mMessage = new ServerPacket(ServerPacketHeader.SlideObjectBundleMessageComposer);
            mMessage.WriteInteger(pItem.GetX);
            mMessage.WriteInteger(pItem.GetY);

            mMessage.WriteInteger(NextCoord.X);
            mMessage.WriteInteger(NextCoord.Y);

            mMessage.WriteInteger(1);

            mMessage.WriteInteger(pItem.Id);

            mMessage.WriteString(TextHandling.GetString(pItem.GetZ));
            mMessage.WriteString(TextHandling.GetString(NextZ));

            mMessage.WriteInteger(pRolledID);

            SetFloorItem(pItem, NextCoord.X, NextCoord.Y, NextZ);

            return mMessage;
        }

        public ServerPacket UpdateUserOnRoller(RoomUser pUser, Point pNextCoord, int pRollerID, double NextZ)
        {
            ServerPacket mMessage = new ServerPacket(ServerPacketHeader.SlideObjectBundleMessageComposer);
            mMessage.WriteInteger(pUser.X);
            mMessage.WriteInteger(pUser.Y);

            mMessage.WriteInteger(pNextCoord.X);
            mMessage.WriteInteger(pNextCoord.Y);

            mMessage.WriteInteger(0);
            mMessage.WriteInteger(pRollerID);
            mMessage.WriteInteger(2);
            mMessage.WriteInteger(pUser.VirtualId);
            mMessage.WriteString(TextHandling.GetString(pUser.Z));
            mMessage.WriteString(TextHandling.GetString(NextZ));

            _room.GetGameMap().UpdateUserMovement(new Point(pUser.X, pUser.Y), new Point(pNextCoord.X, pNextCoord.Y), pUser);
            _room.GetGameMap().GameMap[pUser.X, pUser.Y] = 1;
            pUser.X = pNextCoord.X;
            pUser.Y = pNextCoord.Y;
            pUser.Z = NextZ;

            _room.GetGameMap().GameMap[pUser.X, pUser.Y] = 0;

            if (pUser != null && pUser.GetClient() != null && pUser.GetClient().GetHabbo() != null)
            {
                List<Item> Items = _room.GetGameMap().GetRoomItemForSquare(pNextCoord.X, pNextCoord.Y);
                foreach (Item IItem in Items)
                {
                    if (IItem == null)
                    {
                        continue;
                    }

                    _room.GetWired().TriggerEvent(WiredBoxType.TriggerWalkOnFurni, pUser.GetClient().GetHabbo(), IItem);
                }

                Item Item = _room.GetRoomItemHandler().GetItem(pRollerID);
                if (Item != null)
                {
                    _room.GetWired().TriggerEvent(WiredBoxType.TriggerWalkOffFurni, pUser.GetClient().GetHabbo(), Item);
                }
            }

            return mMessage;
        }

        public void SaveFurniture()
        {
            try
            {
                if (_movedItems.Count > 0)
                {
                    foreach (Item Item in _movedItems.Values)
                    {
                        using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            GameClient Client = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(Item.UserID);
                            if (Item.IsWallItem && (!Item.GetBaseItem().ItemName.Contains("wallpaper_single") || !Item.GetBaseItem().ItemName.Contains("floor_single") || !Item.GetBaseItem().ItemName.Contains("landscape_single")))
                            {
                                dbClient.SetQuery("UPDATE `items` SET `wall_pos` = @wallPos WHERE `id` = '" + Item.Id + "' LIMIT 1");
                                dbClient.AddParameter("wallPos", Item.wallCoord);
                                dbClient.RunQuery();
                            }
                            if (Item.GetBaseItem().ItemName.Contains("wallpaper_single"))
                            {
                                NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Client, "ACH_RoomDecoFloor", 1);
                            }
                            if (Item.GetBaseItem().ItemName.Contains("floor_single"))
                            {
                                NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Client, "ACH_RoomDecoWallpaper", 1);
                            }
                            if (Item.GetBaseItem().ItemName.Contains("landscape_single"))
                            {
                                NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Client, "ACH_RoomDecoLandscape", 1);
                            }
                            // if (Item.GetBaseItem().ItemName == "es_skating_ice")
                            // {
                            //     NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Client, "ACH_TagA", 1);
                            // }
                            if (Item.GetBaseItem().ItemName.Contains("val11_floor"))
                            {
                                NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Client, "ACH_RbTagA", 1);
                            }
                            /* if (Item.GetBaseItem().ItemName == "easter11_grasspatc")
                             {
                                 NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Client, "ACH_RbBunnyTa", 1);
                             }
                             if (Item.GetBaseItem().ItemName == "hole" || Item.GetBaseItem().ItemName == "hole2")
                             {
                                 NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Client, "ACH_RoomDecoHoleFurniCount", 1);
                             }
                             if (Item.GetBaseItem().ItemName == "snowb_slope")
                             {
                                 NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Client, "ACH_snowBoardBuild", 1);
                             }  */
                            dbClient.SetQuery("UPDATE `items` SET `base_item` = " + Item.BaseItem + ", `x` = '" + Item.GetX + "', `y` = '" + Item.GetY + "', `z` = '" + Item.GetZ + "', `rot` = '" + Item.Rotation + "', `extra_data` = @edata" + Item.Id + " WHERE `id` = " + Item.Id);
                            dbClient.AddParameter("edata" + Item.Id, Item.ExtraData);
                            dbClient.RunQuery();
                        }
                    }
                }


                KeyValuePair<int, string> data;
                DateTime now = DateTime.Now;
                int getdaytoday = Convert.ToInt32(now.ToString("MMddyyyy"));
                int getmonthtoday = Convert.ToInt32(now.ToString("MM"));
                int getweektoday = CultureInfo.GetCultureInfo("Nl-nl").Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    if (usedwiredscorebord)
                    {
                        ScorebordChangeCheck();
                        dbClient.RunQuery(string.Concat("DELETE FROM `wired_scorebord`  WHERE roomid = ", _room.RoomId, " "));

                        lock (_room.WiredScoreBordDay)
                        {
                            foreach (int mdayuserids in _room.WiredScoreBordDay.Keys)
                            {
                                if (_room.WiredScoreBordDay.ContainsKey(mdayuserids))
                                {
                                    data = _room.WiredScoreBordDay[mdayuserids];

                                    dbClient.SetQuery("INSERT INTO `wired_scorebord` (`roomid`, `userid`, `username`, `punten`, `soort`, `timestamp`) VALUES ('" + _room.RoomId + "', '" + mdayuserids + "', @dusername" + mdayuserids + ", '" + data.Key + "', 'day', '" + getdaytoday + "')");
                                    dbClient.AddParameter(string.Concat("dusername", mdayuserids), data.Value);
                                    dbClient.RunQuery();
                                }
                            }
                        }

                        lock (_room.WiredScoreBordMonth)
                        {
                            foreach (int mmonthuserids in _room.WiredScoreBordMonth.Keys)
                            {
                                if (_room.WiredScoreBordMonth.ContainsKey(mmonthuserids))
                                {
                                    data = _room.WiredScoreBordMonth[mmonthuserids];

                                    dbClient.SetQuery("INSERT INTO `wired_scorebord` (`roomid`, `userid`, `username`, `punten`, `soort`, `timestamp`) VALUES ('" + _room.RoomId + "', '" + mmonthuserids + "', @musername" + mmonthuserids + ", '" + data.Key + "', 'month', '" + getmonthtoday + "')");
                                    dbClient.AddParameter(string.Concat("musername", mmonthuserids), data.Value);
                                    dbClient.RunQuery();
                                }
                            }
                        }
                        lock (_room.WiredScoreBordWeek)
                        {
                            foreach (int weekuserids in _room.WiredScoreBordWeek.Keys)
                            {
                                if (_room.WiredScoreBordDay.ContainsKey(weekuserids))
                                {
                                    data = _room.WiredScoreBordDay[weekuserids];

                                    dbClient.SetQuery("INSERT INTO `wired_scorebord` (`roomid`, `userid`, `username`, `punten`, `soort`, `timestamp`) VALUES ('" + _room.RoomId + "', '" + weekuserids + "', @wusername" + weekuserids + ", '" + data.Key + "', 'week', '" + getweektoday + "')");
                                    dbClient.AddParameter(string.Concat("wusername", weekuserids), data.Value);
                                    dbClient.RunQuery();
                                }
                            }
                        }
                    }
                }
                usedwiredscorebord = false;
            }

            catch (Exception e)
            {
                Logging.LogCriticalException("Error during saving furniture for room " + _room.RoomId + ". Stack: " + e);
            }
        }
        public bool SetFloorItem(GameClient Session, Item Item, int newX, int newY, int newRot, bool newItem, bool OnRoller, bool sendMessage, bool updateRoomUserStatuses = false, bool ball = false)
        {
            bool NeedsReAdd = false;

            if (newItem)
            {
                if (Item.IsWired)
                {
                    if (Item.GetBaseItem().WiredType == WiredBoxType.EffectRegenerateMaps && _room.GetRoomItemHandler().GetFloor.Where(x => x.GetBaseItem().WiredType == WiredBoxType.EffectRegenerateMaps).Count() > 0)
                    {
                        return false;
                    }
                }
            }

            List<Item> ItemsOnTile = GetFurniObjects(newX, newY);
            if (Item.GetBaseItem().InteractionType == InteractionType.ROLLER && ItemsOnTile.Where(x => x.GetBaseItem().InteractionType == InteractionType.ROLLER && x.Id != Item.Id).Count() > 0)
            {
                return false;
            }

            {
                for (int i = 0; i < ItemsOnTile.Count(); i++)
                {
                    Item it = ItemsOnTile[i];
                    if (it.Id == Item.Id)
                    {
                        continue;
                    }

                    if (it.GetBaseItem().InteractionType == InteractionType.ROLLER)
                    {
                        return false;
                    }
                }
            }
            if (Item.GetBaseItem().InteractionType == InteractionType.DICE && ItemsOnTile.Count(x => x.GetBaseItem().InteractionType == InteractionType.DICE && x.Id != Item.Id) > 0)
            {
                return false;
            }

            if (!newItem)
            {
                NeedsReAdd = _room.GetGameMap().RemoveFromMap(Item);
            }

            Dictionary<int, ThreeDCoord> AffectedTiles = Gamemap.GetAffectedTiles(Item.GetBaseItem().Length, Item.GetBaseItem().Width, newX, newY, newRot);

            if (!_room.GetGameMap().ValidTile(newX, newY) || _room.GetGameMap().SquareHasUsers(newX, newY) && !Item.GetBaseItem().IsSeat)
            {
                if (NeedsReAdd && !ball)
                {
                    _room.GetGameMap().AddToMap(Item);
                }

                if (!ball)
                {
                    return false;
                }
            }

            foreach (ThreeDCoord Tile in AffectedTiles.Values)
            {
                if (!_room.GetGameMap().ValidTile(Tile.X, Tile.Y) ||
                    (_room.GetGameMap().SquareHasUsers(Tile.X, Tile.Y) && !Item.GetBaseItem().IsSeat))
                {
                    if (NeedsReAdd)
                    {
                        _room.GetGameMap().AddToMap(Item);
                    }
                    return false;
                }
            }

            double newZ = _room.GetGameMap().Model.SqFloorHeight[newX, newY];

            if (!OnRoller)
            {
                if (_room.GetGameMap().Model.SqState[newX, newY] != SquareState.OPEN && !Item.GetBaseItem().IsSeat)
                {
                    return false;
                }

                foreach (ThreeDCoord Tile in AffectedTiles.Values)
                {
                    if (_room.GetGameMap().Model.SqState[Tile.X, Tile.Y] != SquareState.OPEN &&
                        !Item.GetBaseItem().IsSeat)
                    {
                        if (NeedsReAdd)
                        {
                            //AddItem(Item);
                            _room.GetGameMap().AddToMap(Item);
                        }
                        return false;
                    }
                }

                if (!Item.GetBaseItem().IsSeat && !Item.IsRoller)
                {
                    foreach (ThreeDCoord Tile in AffectedTiles.Values)
                    {
                        if (_room.GetGameMap().GetRoomUsers(new Point(Tile.X, Tile.Y)).Count > 0)
                        {
                            if (NeedsReAdd)
                            {
                                _room.GetGameMap().AddToMap(Item);
                            }

                            return false;
                        }
                    }
                }
            }

            List<Item> ItemsAffected = new List<Item>();
            List<Item> ItemsComplete = new List<Item>();

            foreach (ThreeDCoord Tile in AffectedTiles.Values)
            {
                List<Item> Temp = GetFurniObjects(Tile.X, Tile.Y);

                if (Temp != null)
                {
                    ItemsAffected.AddRange(Temp);
                }
            }


            ItemsComplete.AddRange(ItemsOnTile);
            ItemsComplete.AddRange(ItemsAffected);

            if (!OnRoller)
            {
                foreach (Item I in ItemsComplete)
                {
                    if (I == null)
                    {
                        continue;
                    }

                    if (I.Id == Item.Id)
                    {
                        continue;
                    }

                    if (I.GetBaseItem() == null)
                    {
                        continue;
                    }

                    if (!I.GetBaseItem().Stackable && Item.GetBaseItem().InteractionType != InteractionType.STACKTOOL && !_room.GetGameMap().HasStackTool(I.GetX, I.GetY))
                    {
                        if (NeedsReAdd)
                        {
                            //AddItem(Item);
                            _room.GetGameMap().AddToMap(Item);
                        }
                        return false;
                    }
                }
            }

            {
                if (Item.Rotation != newRot && Item.GetX == newX && Item.GetY == newY)
                {
                    newZ = Item.GetZ;
                }

                foreach (Item I in ItemsComplete)
                {
                    if (I == null)
                    {
                        continue;
                    }

                    if (I.Id == Item.Id)
                    {
                        continue;
                    }

                    if (I.GetBaseItem().InteractionType == InteractionType.STACKTOOL)
                    {
                        newZ = I.GetZ;
                        break;
                    }


                    if (I.TotalHeight > newZ)
                    {
                        newZ = I.TotalHeight;
                    }


                }
            }

            if (newRot != 0 && newRot != 2 && newRot != 4 && newRot != 6 && newRot != 8 && !Item.GetBaseItem().ExtraRot)
            {
                newRot = 0;
            }

            Item.Rotation = newRot;
            int oldX = Item.GetX;
            int oldY = Item.GetY;

            if (Item.GetBaseItem().InteractionType == InteractionType.STACKTOOL)
            {
                newZ = 0;
                _room.SendMessage(new UpdateMagicTileComposer(Item.Id, 0));
            }


            Item.SetState(newX, newY, newZ, AffectedTiles);

            if (!OnRoller && Session != null)
            {
                Item.Interactor.OnPlace(Session, Item);
            }

            if (newItem)
            {
                if (_floorItems.ContainsKey(Item.Id))
                {
                    if (Session != null)
                    {
                        Session.SendNotification(NeonEnvironment.GetGame().GetLanguageLocale().TryGetValue("room_item_placed"));
                    }

                    _room.GetGameMap().RemoveFromMap(Item);
                    return true;
                }

                if (Item.IsFloorItem && !_floorItems.ContainsKey(Item.Id))
                {
                    _floorItems.TryAdd(Item.Id, Item);
                }
                else if (Item.IsWallItem && !_wallItems.ContainsKey(Item.Id))
                {
                    _wallItems.TryAdd(Item.Id, Item);
                }

                if (sendMessage)
                {
                    _room.SendMessage(new ObjectAddComposer(Item, _room));
                }
            }
            else
            {
                UpdateItem(Item);
                if (!OnRoller && sendMessage)
                {
                    _room.SendMessage(new ObjectUpdateComposer(Item, _room.OwnerId));
                }
            }
            _room.GetGameMap().AddToMap(Item);

            if (Item.GetBaseItem().IsSeat)
            {
                updateRoomUserStatuses = true;
            }

            if (updateRoomUserStatuses)
            {
                _room.GetRoomUserManager().UpdateUserStatusses();
            }

            if (Item.GetBaseItem().InteractionType == InteractionType.TENT || Item.GetBaseItem().InteractionType == InteractionType.TENT_SMALL)
            {
                _room.RemoveTent(Item.Id, Item);
                _room.AddTent(Item.Id);
            }

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE `items` SET `room_id` = '" + _room.RoomId + "', `x` = '" + Item.GetX + "', `y` = '" + Item.GetY + "', `z` = '" + Item.GetZ + "', `rot` = '" + Item.Rotation + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
            }
            return true;
        }

        public List<Item> GetFurniObjects(int X, int Y)
        {
            return _room.GetGameMap().GetCoordinatedItems(new Point(X, Y));
        }

        public bool SetFloorItem(Item Item, int newX, int newY, double newZ)
        {
            if (_room == null)
            {
                return false;
            }

            _room.GetGameMap().RemoveFromMap(Item);

            Item.SetState(newX, newY, newZ, Gamemap.GetAffectedTiles(Item.GetBaseItem().Length, Item.GetBaseItem().Width, newX, newY, Item.Rotation));
            if (Item.GetBaseItem().InteractionType == InteractionType.TONER)
            {
                if (_room.TonerData == null)
                {
                    _room.TonerData = new TonerData(Item.Id);
                }
            }
            UpdateItem(Item);
            _room.GetGameMap().AddItemToMap(Item);
            return true;
        }

        public bool SetWallItem(GameClient Session, Item Item)
        {
            if (!Item.IsWallItem || _wallItems.ContainsKey(Item.Id))
            {
                return false;
            }

            if (_floorItems.ContainsKey(Item.Id))
            {
                Session.SendNotification(NeonEnvironment.GetGame().GetLanguageLocale().TryGetValue("room_item_placed"));
                return true;
            }


            Item.Interactor.OnPlace(Session, Item);
            if (Item.GetBaseItem().InteractionType == InteractionType.MOODLIGHT)
            {
                if (_room.MoodlightData != null)
                {
                    _room.MoodlightData = new MoodlightData(Item.Id);
                    Item.ExtraData = _room.MoodlightData.GenerateExtraData();
                }
            }

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `items` SET `room_id` = '" + _room.RoomId + "', `x` = '" + Item.GetX + "', `y` = '" + Item.GetY + "', `z` = '" + Item.GetZ + "', `rot` = '" + Item.Rotation + "', `wall_pos` = @WallPos WHERE `id` = '" + Item.Id + "' LIMIT 1");
                dbClient.AddParameter("WallPos", Item.wallCoord);
                dbClient.RunQuery();
            }

            _wallItems.TryAdd(Item.Id, Item);

            _room.SendMessage(new ItemAddComposer(Item));

            return true;
        }

        public void UpdateItem(Item item)
        {
            if (item == null)
            {
                return;
            }

            if (!_movedItems.ContainsKey(item.Id))
            {
                _movedItems.TryAdd(item.Id, item);
            }
        }


        public void RemoveItem(Item item)
        {
            if (item == null)
            {
                return;
            }

            if (_movedItems.ContainsKey(item.Id))
            {
                _movedItems.TryRemove(item.Id, out item);
            }

            if (_rollers.ContainsKey(item.Id))
            {
                _ = _rollers.TryRemove(item.Id, out _);
            }
        }

        public void OnCycle()
        {
            if (GotRollers)
            {
                try
                {
                    _room.SendMessage(CycleRollers());
                }
                catch //(Exception e)
                {
                    // Logging.LogThreadException(e.ToString(), "rollers for room with ID " + room.RoomId);
                    GotRollers = false;
                }
            }

            if (_roomItemUpdateQueue.Count > 0)
            {
                List<Item> addItems = new List<Item>();
                while (_roomItemUpdateQueue.Count > 0)
                {
                    if (_roomItemUpdateQueue.TryDequeue(out Item item))
                    {
                        item.ProcessUpdates();

                        if (item.UpdateCounter > 0)
                        {
                            addItems.Add(item);
                        }
                    }
                }

                foreach (Item item in addItems)
                {
                    if (item == null)
                    {
                        continue;
                    }

                    _roomItemUpdateQueue.Enqueue(item);
                }
            }

            //mFloorItems.OnCycle();
            //mWallItems.OnCycle();
        }

        public List<Item> RemoveItems(GameClient Session)
        {
            List<Item> Items = new List<Item>();

            foreach (Item Item in GetWallAndFloor)
            {
                if (Item == null || Item.UserID != Session.GetHabbo().Id)
                {
                    continue;
                }

                if (Item.IsFloorItem)
                {
                    _floorItems.TryRemove(Item.Id, out Item I);
                    Session.GetHabbo().GetInventoryComponent()._floorItems.TryAdd(Item.Id, I);
                    _room.SendMessage(new ObjectRemoveComposer(Item, Item.UserID));
                }
                else if (Item.IsWallItem)
                {
                    _wallItems.TryRemove(Item.Id, out Item I);
                    Session.GetHabbo().GetInventoryComponent()._wallItems.TryAdd(Item.Id, I);
                    _room.SendMessage(new ItemRemoveComposer(Item, Item.UserID));
                }

                _room.GetGameMap().GenerateMaps();
                Session.SendMessage(new FurniListAddComposer(Item));
            }

            _rollers.Clear();
            return Items;
        }

        public ICollection<Item> GetFloor => _floorItems.Values;

        public ICollection<Item> GetWall => _wallItems.Values;

        public IEnumerable<Item> GetWallAndFloor => _floorItems.Values.Concat(_wallItems.Values);


        public bool CheckPosItem(GameClient Session, Item Item, int newX, int newY, int newRot, bool newItem, bool SendNotify = true)
        {
            try
            {
                Dictionary<int, ThreeDCoord> dictionary = Gamemap.GetAffectedTiles(Item.GetBaseItem().Length, Item.GetBaseItem().Width, newX, newY, newRot);
                if (!_room.GetGameMap().ValidTile(newX, newY))
                {
                    return false;
                }

                foreach (ThreeDCoord coord in dictionary.Values)
                {
                    if ((_room.GetGameMap().Model.DoorX == coord.X) && (_room.GetGameMap().Model.DoorY == coord.Y))
                    {
                        return false;
                    }
                }

                if ((_room.GetGameMap().Model.DoorX == newX) && (_room.GetGameMap().Model.DoorY == newY))
                {
                    return false;
                }

                foreach (ThreeDCoord coord in dictionary.Values)
                {
                    if (!_room.GetGameMap().ValidTile(coord.X, coord.Y))
                    {
                        return false;
                    }
                }

                double num = _room.GetGameMap().Model.SqFloorHeight[newX, newY];
                if ((((Item.Rotation == newRot) && (Item.GetX == newX)) && (Item.GetY == newY)) && (Item.GetZ != num))
                {
                    return false;
                }

                if (_room.GetGameMap().Model.SqState[newX, newY] != SquareState.OPEN)
                {
                    return false;
                }

                foreach (ThreeDCoord coord in dictionary.Values)
                {
                    if (_room.GetGameMap().Model.SqState[coord.X, coord.Y] != SquareState.OPEN)
                    {
                        return false;
                    }
                }
                if (!Item.GetBaseItem().IsSeat)
                {
                    if (_room.GetGameMap().SquareHasUsers(newX, newY))
                    {
                        return false;
                    }

                    foreach (ThreeDCoord coord in dictionary.Values)
                    {
                        if (_room.GetGameMap().SquareHasUsers(coord.X, coord.Y))
                        {
                            return false;
                        }
                    }
                }

                List<Item> furniObjects = GetFurniObjects(newX, newY);
                List<Item> collection = new List<Item>();
                List<Item> list3 = new List<Item>();
                foreach (ThreeDCoord coord in dictionary.Values)
                {
                    List<Item> list4 = GetFurniObjects(coord.X, coord.Y);
                    if (list4 != null)
                    {
                        collection.AddRange(list4);
                    }
                }

                if (furniObjects == null)
                {
                    furniObjects = new List<Item>();
                }

                list3.AddRange(furniObjects);
                list3.AddRange(collection);
                foreach (Item item in list3)
                {
                    if ((item.Id != Item.Id) && !item.GetBaseItem().Stackable)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


        public ICollection<Item> GetRollers()
        {
            return _rollers.Values;
        }

        public void Dispose()
        {
            foreach (Item Item in GetWallAndFloor)
            {
                if (Item == null)
                {
                    continue;
                }

                Item.Destroy();
            }

            _floorItems.Clear();
            _wallItems.Clear();
            _movedItems.Clear();
            _rollers.Clear();
            _roomItemUpdateQueue = null;

            _room = null;
            _floorItems = null;
            _wallItems = null;
            _movedItems = null;
            _wallItems = null;
            _roomItemUpdateQueue = null;
        }
    }
}
