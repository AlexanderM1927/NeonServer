using Neon.HabboHotel.Items;
using Neon.HabboHotel.Rooms;
using Neon.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Neon.Communication.Packets.Outgoing.Rooms.Engine
{
    internal class ObjectsComposer : ServerPacket
    {
        public ObjectsComposer(Item[] Objects, Room Room)
            : base(ServerPacketHeader.ObjectsMessageComposer)
        {
            WriteInteger(1);
            WriteInteger(Room.OwnerId);
            WriteString(Room.OwnerName);

            List<Item> l = new List<Item>();
            if (Room.HideWired)
            {
                for (int i = 0; i < Objects.Count(); i++)
                {
                    Item it = Objects[i];
                    if (it == null)
                    {
                        continue;
                    }

                    if (it.IsWired)
                    {
                        continue;
                    }

                    l.Add(it);
                }
                Objects = l.ToArray();
                base.WriteInteger(Objects.Length);
                for (int i = 0; i < Objects.Count(); i++)
                {
                    Item Item = Objects[i];
                    WriteFloorItem(Item, Convert.ToInt32(Item.UserID));
                }
            }

            else
            {
                base.WriteInteger(Objects.Length);
                for (int i = 0; i < Objects.Count(); i++)
                {
                    Item Item = Objects[i];
                    WriteFloorItem(Item, Convert.ToInt32(Item.UserID));
                }
            }
        }


        private void WriteFloorItem(Item Item, int UserID)
        {

            WriteInteger(Item.Id);
            WriteInteger(Item.GetBaseItem().SpriteId);
            WriteInteger(Item.GetX);
            WriteInteger(Item.GetY);
            WriteInteger(Item.Rotation);
            WriteString(string.Format("{0:0.00}", TextHandling.GetString(Item.GetZ)));
            WriteString(string.Empty);

            if (Item.LimitedNo > 0)
            {
                WriteInteger(1);
                WriteInteger(256);
                WriteString(Item.ExtraData);
                WriteInteger(Item.LimitedNo);
                WriteInteger(Item.LimitedTot);
            }

            else if (Item.Data.InteractionType == InteractionType.INFO_TERMINAL || Item.Data.InteractionType == InteractionType.ROOM_PROVIDER)
            {
                WriteInteger(0);
                WriteInteger(1);
                WriteInteger(1);
                WriteString("internalLink");
                WriteString(Item.ExtraData);
            }

            else if (Item.Data.InteractionType == InteractionType.FX_PROVIDER)
            {
                WriteInteger(0);
                WriteInteger(1);
                WriteInteger(1);
                WriteString("effectId");
                WriteString(Item.ExtraData);
            }

            else if (Item.Data.InteractionType == InteractionType.PINATA)
            {
                WriteInteger(0);
                WriteInteger(7);
                WriteString("6");
                if (Item.ExtraData.Length <= 0)
                {
                    WriteInteger(0);
                }
                else
                {
                    WriteInteger(int.Parse(Item.ExtraData));
                }

                WriteInteger(100);
            }

            else if (Item.Data.InteractionType == InteractionType.PINATATRIGGERED)
            {
                WriteInteger(0);
                WriteInteger(7);
                WriteString("0");
                if (Item.ExtraData.Length <= 0)
                {
                    WriteInteger(0);
                }
                else
                {
                    WriteInteger(int.Parse(Item.ExtraData));
                }

                WriteInteger(1);
            }


            else if (Item.Data.InteractionType == InteractionType.PLANT_SEED)
            {
                base.WriteInteger(0);
                base.WriteInteger(7);
                base.WriteString(Item.ExtraData);
                if (Item.ExtraData.Length <= 0)
                {
                    base.WriteInteger(0);
                }
                else
                {
                    base.WriteInteger(int.Parse(Item.ExtraData));
                }
                base.WriteInteger(12);
            }

            else if (Item.Data.InteractionType == InteractionType.EASTEREGG)
            {
                base.WriteInteger(0);
                base.WriteInteger(7);
                base.WriteString(Item.ExtraData);
                if (Item.ExtraData.Length <= 0)
                {
                    base.WriteInteger(0);
                }
                else
                {
                    base.WriteInteger(int.Parse(Item.ExtraData));
                }
                base.WriteInteger(20);
            }

            else if (Item.Data.InteractionType == InteractionType.MAGICEGG)
            {
                WriteInteger(0);
                WriteInteger(7);
                WriteString(Item.ExtraData);
                if (Item.ExtraData.Length <= 0)
                {
                    WriteInteger(0);
                }
                else
                {
                    WriteInteger(int.Parse(Item.ExtraData));
                }
                WriteInteger(23);
            }

            else if (Item.Data.InteractionType == InteractionType.RPGNEON)
            {
                WriteInteger(0);
                WriteInteger(7);
                WriteString(Item.ExtraData);
                if (Item.ExtraData.Length <= 0)
                {
                    WriteInteger(0);
                }
                else
                {
                    WriteInteger(int.Parse(Item.ExtraData));
                }
                WriteInteger(23);
            }

            else if (Item.Data.InteractionType == InteractionType.MAGICCHEST)
            {
                WriteInteger(0);
                WriteInteger(7);
                WriteString(Item.ExtraData);
                if (Item.ExtraData.Length <= 0)
                {
                    WriteInteger(0);
                }
                else
                {
                    WriteInteger(int.Parse(Item.ExtraData));
                }
                WriteInteger(1);
            }

            else if (Item.Data.InteractionType == InteractionType.CAIXANEON)
            {
                base.WriteInteger(0);
                base.WriteInteger(7);
                base.WriteString(Item.ExtraData);
                if (Item.ExtraData.Length <= 0)
                {
                    base.WriteInteger(0);
                }
                else
                {
                    base.WriteInteger(int.Parse(Item.ExtraData));
                }
                base.WriteInteger(1);
            }
            else
            {
                ItemBehaviourUtility.GenerateExtradata(Item, this);
            }

            WriteInteger(-1); // to-do: check
            WriteInteger((Item.GetBaseItem().Modes > 1) ? 1 : 0);
            WriteInteger(UserID);
        }
    }
}