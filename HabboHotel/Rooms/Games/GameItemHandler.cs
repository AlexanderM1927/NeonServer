﻿using Neon.HabboHotel.Items;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Neon.HabboHotel.Rooms
{
    public class GameItemHandler
    {
        private Room room;
        private Random rnd;
        private ConcurrentDictionary<int, Item> _banzaiPyramids;
        private ConcurrentDictionary<int, Item> _banzaiTeleports;

        public GameItemHandler(Room room)
        {
            this.room = room;
            rnd = new Random();

            _banzaiPyramids = new ConcurrentDictionary<int, Item>();
            _banzaiTeleports = new ConcurrentDictionary<int, Item>();
        }

        public void OnCycle()
        {
            CyclePyramids();
        }

        private void CyclePyramids()
        {
            Random rnd = new Random();

            foreach (Item item in _banzaiPyramids.Values.ToList())
            {
                if (item == null)
                {
                    continue;
                }

                if (item.interactionCountHelper == 0 && item.ExtraData == "1")
                {
                    room.GetGameMap().RemoveFromMap(item, false);
                    item.interactionCountHelper = 1;
                }

                if (string.IsNullOrEmpty(item.ExtraData))
                {
                    item.ExtraData = "0";
                }

                int randomNumber = rnd.Next(0, 30);
                if (randomNumber == 15)
                {
                    if (item.ExtraData == "0")
                    {
                        item.ExtraData = "1";
                        item.UpdateState();
                        room.GetGameMap().RemoveFromMap(item, false);
                    }
                    else
                    {
                        if (room.GetGameMap().ItemCanBePlacedHere(item.GetX, item.GetY))
                        {
                            item.ExtraData = "0";
                            item.UpdateState();
                            room.GetGameMap().AddItemToMap(item);
                        }
                    }
                }
            }
        }

        public void AddPyramid(Item item, int itemID)
        {
            if (_banzaiPyramids.ContainsKey(itemID))
            {
                _banzaiPyramids[itemID] = item;
            }
            else
            {
                _banzaiPyramids.TryAdd(itemID, item);
            }
        }

        public void RemovePyramid(int itemID)
        {
            _banzaiPyramids.TryRemove(itemID, out Item Item);
        }

        public void AddTeleport(Item item, int itemID)
        {
            if (_banzaiTeleports.ContainsKey(itemID))
            {
                _banzaiTeleports[itemID] = item;
            }
            else
            {
                _banzaiTeleports.TryAdd(itemID, item);
            }
        }

        public void RemoveTeleport(int itemID)
        {
            _banzaiTeleports.TryRemove(itemID, out Item Item);
        }

        public void onTeleportRoomUserEnter(RoomUser User, Item Item)
        {
            IEnumerable<Item> items = _banzaiTeleports.Values.Where(p => p.Id != Item.Id);

            int count = items.Count();

            int countID = rnd.Next(0, count);
            int countAmount = 0;

            if (count == 0)
            {
                return;
            }

            foreach (Item item in items.ToList())
            {
                if (item == null)
                {
                    continue;
                }

                if (countAmount == countID)
                {
                    item.ExtraData = "1";
                    item.UpdateNeeded = true;

                    room.GetGameMap().TeleportToItem(User, item);

                    Item.ExtraData = "1";
                    Item.UpdateNeeded = true;
                    item.UpdateState();
                    Item.UpdateState();
                }

                countAmount++;
            }
        }

        public void Dispose()
        {
            if (_banzaiTeleports != null)
            {
                _banzaiTeleports.Clear();
            }

            if (_banzaiPyramids != null)
            {
                _banzaiPyramids.Clear();
            }

            _banzaiPyramids = null;
            _banzaiTeleports = null;
            room = null;
            rnd = null;
        }
    }
}