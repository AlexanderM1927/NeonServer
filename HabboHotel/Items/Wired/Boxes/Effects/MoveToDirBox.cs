﻿using Neon.Communication.Packets.Incoming;
using Neon.HabboHotel.Items.Wired.Util;
using Neon.HabboHotel.Rooms;
using System;
using System.Collections.Concurrent;
using System.Linq;


namespace Neon.HabboHotel.Items.Wired.Boxes.Effects
{
    internal class MoveToDirBox : IWiredItem, IWiredCycle
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        private bool _needChange;
        private MovementDirection _startDirection;
        private WhenMovementBlock _whenMoveIsBlocked;

        public WiredBoxType Type => WiredBoxType.EffectMoveToDir;

        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData
        {
            get => string.Format("{0};{1}", StartDirection, WhenMoveIsBlocked);
            set
            {
                string[] array = value.Split(';');
                if (array.Length != 2)
                {
                    _startDirection = MovementDirection.NONE;
                    _whenMoveIsBlocked = WhenMovementBlock.NONE;
                    return;
                }
                _startDirection = (MovementDirection)int.Parse(array[0]);
                _whenMoveIsBlocked = (WhenMovementBlock)int.Parse(array[1]);
            }
        }
        public bool BoolData { get; set; }

        public int StartDirection => (int)_startDirection;

        public int WhenMoveIsBlocked => (int)_whenMoveIsBlocked;

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

        public MoveToDirBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            SetItems = new ConcurrentDictionary<int, Item>();
            TickCount = Delay;
            Requested = false;
        }

        public void HandleSave(ClientPacket Packet)
        {
            if (SetItems.Count > 0)
            {
                SetItems.Clear();
            }

            int Unknown = Packet.PopInt();
            int Movement = Packet.PopInt();
            int Rotation = Packet.PopInt();

            string Unknown1 = Packet.PopString();

            int FurniCount = Packet.PopInt();
            for (int i = 0; i < FurniCount; i++)
            {
                Item SelectedItem = Instance.GetRoomItemHandler().GetItem(Packet.PopInt());

                if (SelectedItem != null && !Instance.GetWired().OtherBoxHasItem(this, SelectedItem.Id))
                {
                    SetItems.TryAdd(SelectedItem.Id, SelectedItem);
                }
            }

            StringData = Movement + ";" + Rotation;
            Delay = Packet.PopInt();
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

                    if (Item.MoveToDirMovement == MovementDirection.NONE || _needChange)
                    {
                        Item.MoveToDirMovement = _startDirection;
                        _needChange = false;
                    }

                    System.Drawing.Point newPoint = Movement.HandleMovementDir(Item.Coordinate, Item.MoveToDirMovement, Item.Rotation);
                    if (newPoint != Item.Coordinate)
                    {

                        if (Instance.GetGameMap().SquareIsOpen(newPoint.X, newPoint.Y, false))
                        {
                            Instance.GetRoomItemHandler()
                                .SetFloorItem(null, Item, newPoint.X, newPoint.Y, Item.Rotation, false, false, true, true);
                        }
                        else
                        {
                            switch (_whenMoveIsBlocked)
                            {
                                #region NONE

                                case WhenMovementBlock.NONE:
                                    {
                                        Item.MoveToDirMovement = MovementDirection.NONE;
                                        break;
                                    }

                                #endregion NONE

                                #region RIGHT45

                                case WhenMovementBlock.RIGHT45:
                                    {
                                        if (Item.MoveToDirMovement == MovementDirection.RIGHT)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.RIGHT;
                                            }
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.LEFT)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.LEFT;
                                            }
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.UP)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP;
                                            }
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.DOWN)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN;
                                            }
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.UP_LEFT)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_LEFT;
                                            }
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.UP_RIGHT)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.RIGHT;
                                                break;
                                            }
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_RIGHT;
                                                break;
                                            }
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN;
                                                break;
                                            }
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_LEFT;
                                                break;
                                            }
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.LEFT;
                                                break;
                                            }
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_LEFT;
                                                break;
                                            }
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP;
                                                break;
                                            }
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_RIGHT;
                                                break;
                                            }
                                            return false;
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.DOWN_RIGHT)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_RIGHT;
                                            }
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.DOWN_LEFT)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_LEFT;
                                            }
                                        }

                                        break;
                                    }

                                #endregion RIGHT45

                                #region RIGHT90

                                case WhenMovementBlock.RIGHT90:
                                    {
                                        if (Item.MoveToDirMovement == MovementDirection.RIGHT)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.RIGHT;
                                            }
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.LEFT)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.LEFT;
                                            }
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.UP)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP;
                                            }
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.DOWN)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN;
                                            }
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.UP_LEFT)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_LEFT;
                                            }
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.UP_RIGHT)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_RIGHT;
                                                break;
                                            }
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_LEFT;
                                                break;
                                            }
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_LEFT;
                                                break;
                                            }
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_RIGHT;
                                                break;
                                            }
                                            return false;
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.DOWN_RIGHT)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_RIGHT;
                                            }
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.DOWN_LEFT)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_LEFT;
                                            }
                                        }

                                        break;
                                    }

                                #endregion RIGHT90

                                #region LEFT45

                                case WhenMovementBlock.LEFT45:
                                    {
                                        if (Item.MoveToDirMovement == MovementDirection.RIGHT)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.RIGHT;
                                            }
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.LEFT)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.LEFT;
                                            }
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.UP)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP;
                                            }
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.DOWN)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN;
                                            }
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.UP_LEFT)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_LEFT;
                                            }
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.UP_RIGHT)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_RIGHT;
                                            }
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.DOWN_RIGHT)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_RIGHT;
                                            }
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.DOWN_LEFT)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_LEFT;
                                            }
                                        }

                                        break;
                                    }

                                #endregion LEFT45

                                #region LEFT90

                                case WhenMovementBlock.LEFT90:
                                    {
                                        if (Item.MoveToDirMovement == MovementDirection.RIGHT)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.RIGHT;
                                            }
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.LEFT)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.LEFT;
                                            }
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.UP)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP;
                                            }
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.DOWN)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN;
                                            }
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.UP_LEFT)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_LEFT;
                                            }
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.UP_RIGHT)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_RIGHT;
                                            }
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.DOWN_RIGHT)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_RIGHT;
                                            }
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.DOWN_LEFT)
                                        {
                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX + 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_RIGHT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY - 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.UP_LEFT;
                                            }

                                            if (Instance.GetGameMap().IsValidMovement(Item.GetX - 1, Item.GetY + 1))
                                            {
                                                Item.MoveToDirMovement = MovementDirection.DOWN_LEFT;
                                            }
                                        }

                                        break;
                                    }

                                #endregion LEFT90

                                #region Turn Back

                                case WhenMovementBlock.TURN_BACK:
                                    {
                                        if (Item.MoveToDirMovement == MovementDirection.RIGHT)
                                        {
                                            Item.MoveToDirMovement = MovementDirection.LEFT;
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.LEFT)
                                        {
                                            Item.MoveToDirMovement = MovementDirection.RIGHT;
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.UP)
                                        {
                                            Item.MoveToDirMovement = MovementDirection.DOWN;
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.DOWN)
                                        {
                                            Item.MoveToDirMovement = MovementDirection.UP;
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.UP_RIGHT)
                                        {
                                            Item.MoveToDirMovement = MovementDirection.DOWN_LEFT;
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.DOWN_LEFT)
                                        {
                                            Item.MoveToDirMovement = MovementDirection.UP_RIGHT;
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.UP_LEFT)
                                        {
                                            Item.MoveToDirMovement = MovementDirection.DOWN_RIGHT;
                                        }
                                        else if (Item.MoveToDirMovement == MovementDirection.DOWN_RIGHT)
                                        {
                                            Item.MoveToDirMovement = MovementDirection.UP_LEFT;
                                        }

                                        break;
                                    }

                                #endregion Turn Back

                                #region Random

                                case WhenMovementBlock.TURN_RANDOM:
                                    {
                                        Item.MoveToDirMovement = (MovementDirection)new Random().Next(1, 7);
                                        break;
                                    }

                                    #endregion Random
                            }

                            newPoint = Movement.HandleMovementDir(Item.Coordinate, Item.MoveToDirMovement, Item.Rotation);
                            Instance.GetRoomItemHandler()
                                .SetFloorItem(null, Item, newPoint.X, newPoint.Y, Item.Rotation, false, false, true, true);
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