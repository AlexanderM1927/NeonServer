﻿using Neon.Communication.Packets.Incoming;
using Neon.Communication.Packets.Outgoing.Rooms.Engine;
using Neon.HabboHotel.Rooms;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace Neon.HabboHotel.Items.Wired.Boxes.Effects
{
    internal class MoveFurniToUserBox : IWiredItem, IWiredCycle
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }

        public WiredBoxType Type => WiredBoxType.EffectMoveFurniToNearestUser;

        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }

        public int Delay
        {
            get => _delay;
            set
            {
                _delay = value;
                TickCount = value + 1;
            }
        }

        public int TickCount { get; set; }
        public string ItemsData { get; set; }
        private bool Requested;
        private int _delay = 0;
        private long _next = 0;

        public MoveFurniToUserBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            SetItems = new ConcurrentDictionary<int, Item>();
            TickCount = Delay;
            Requested = false;
        }

        public void HandleSave(ClientPacket Packet)
        {
            int Unknown = Packet.PopInt();
            string Unknown2 = Packet.PopString();

            if (SetItems.Count > 0)
            {
                SetItems.Clear();
            }

            int FurniCount = Packet.PopInt();
            for (int i = 0; i < FurniCount; i++)
            {
                Item SelectedItem = Instance.GetRoomItemHandler().GetItem(Packet.PopInt());

                if (SelectedItem != null && !Instance.GetWired().OtherBoxHasItem(this, SelectedItem.Id))
                {
                    SetItems.TryAdd(SelectedItem.Id, SelectedItem);
                }
            }

            int Delay = Packet.PopInt();
            this.Delay = Delay;
        }

        public bool Execute(params object[] Params)
        {
            if (SetItems.Count == 0)
            {
                return false;
            }

            if (_next == 0 || _next < NeonEnvironment.Now())
            {
                _next = NeonEnvironment.Now() + Delay;
            }

            if (!Requested)
            {
                TickCount = Delay;
                Requested = true;
            }
            return true;
        }

        public bool OnCycle()
        {
            if (Instance == null || !Requested || _next == 0)
            {
                return false;
            }

            long Now = NeonEnvironment.Now();
            if (_next < Now)
            {
                foreach (Item Item in SetItems.Values.ToList())
                {
                    if (Item == null)
                    {
                        continue;
                    }

                    if (!Instance.GetRoomItemHandler().GetFloor.Contains(Item))
                    {
                        continue;
                    }

                    Item toRemove = null;

                    if (Instance.GetWired().OtherBoxHasItem(this, Item.Id))
                    {
                        SetItems.TryRemove(Item.Id, out toRemove);
                    }

                    Point Point = Instance.GetGameMap().GetChaseMovement(Item);

                    Instance.GetWired().OnUserFurniCollision(Instance, Item);

                    if (!Instance.GetGameMap().ItemCanMove(Item, Point))
                    {
                        continue;
                    }

                    if (Instance.GetGameMap().CanRollItemHere(Point.X, Point.Y) && !Instance.GetGameMap().SquareHasUsers(Point.X, Point.Y))
                    {
                        double NewZ = Item.GetZ;
                        bool CanBePlaced = true;

                        List<Item> Items = Instance.GetGameMap().GetCoordinatedItems(Point);
                        foreach (Item IItem in Items.ToList())
                        {
                            if (IItem == null || IItem.Id == Item.Id)
                            {
                                continue;
                            }

                            if (!IItem.GetBaseItem().Walkable)
                            {
                                _next = 0;
                                CanBePlaced = false;
                                break;
                            }

                            if (IItem.TotalHeight > NewZ)
                            {
                                NewZ = IItem.TotalHeight;
                            }

                            if (CanBePlaced == true && !IItem.GetBaseItem().Stackable)
                            {
                                CanBePlaced = false;
                            }
                        }

                        if (CanBePlaced && Point != Item.Coordinate)
                        {
                            Instance.SendMessage(new SlideObjectBundleComposer(Item.GetX, Item.GetY, Item.GetZ, Point.X,
                                Point.Y, NewZ, 0, 0, Item.Id));
                            Instance.GetRoomItemHandler().SetFloorItem(Item, Point.X, Point.Y, NewZ);
                        }
                    }
                }

                _next = 0;
                return true;
            }
            return false;
        }
    }
}