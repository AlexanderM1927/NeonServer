﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections.Concurrent;

using Neon.Communication.Packets.Incoming;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Pathfinding;

namespace Neon.HabboHotel.Items.Wired.Boxes.Conditions
{
    class FurniHasFurniBox : IWiredItem
    {
        public Room Instance { get; set; }

        public Item Item { get; set; }

        public WiredBoxType Type { get { return WiredBoxType.ConditionFurniHasFurni; } }

        public ConcurrentDictionary<int, Item> SetItems { get; set; }

        public string StringData { get; set; }

        public bool BoolData { get; set; }

        public string ItemsData { get; set; }

        public FurniHasFurniBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            _ = Packet.PopInt();
            int Option = Packet.PopInt();
            _ = Packet.PopString();

            BoolData = Option == 1;

            if (SetItems.Count > 0)
                SetItems.Clear();

            int FurniCount = Packet.PopInt();
            for (int i = 0; i < FurniCount; i++)
            {
                Item SelectedItem = Instance.GetRoomItemHandler().GetItem(Packet.PopInt());
                if (SelectedItem != null)
                    SetItems.TryAdd(SelectedItem.Id, SelectedItem);
            }
        }

        public bool Execute(params object[] Params)
        {
            return this.BoolData ? AllFurniHaveFurniOn() : SomeFurniHaveFurniOn();
        }
        public bool AllFurniHaveFurniOn()
        { 
            foreach (Item Item in SetItems.Values.ToList())
            {
                bool Furni = false;

                List<Item> Items = Instance.GetGameMap().GetAllRoomItemForSquare(Item.GetX, Item.GetY);
                if (Items.Where(x => x.GetZ >= Item.GetZ).Count() > 1)
                    Furni = true;

                if (!Furni)
                    return false;
            }
            return true;
        }


        public bool SomeFurniHaveFurniOn()
        {
            foreach (Item Item in SetItems.Values.ToList())
            {
                if (Item == null || !Instance.GetRoomItemHandler().GetFloor.Contains(Item))
                    continue;

                bool Furni = false;
                foreach (string I in ItemsData.Split(';'))
                {
                    if (string.IsNullOrEmpty(I))
                        continue;

                    Item II = Instance.GetRoomItemHandler().GetItem(Convert.ToInt32(I));

                    if (II == null)
                        continue;

                    List<Item> Items = Instance.GetGameMap().GetAllRoomItemForSquare(II.GetX, II.GetY);
                    if (Items.Where(x => x.GetZ >= Item.GetZ).Count() > 1)
                    {
                        Furni = true;
                        break;
                    }

                }
                if (!Furni)
                    return false;
            }
            return true;
        }
    }
}
