﻿using Neon.HabboHotel.Items;
using Neon.Utilities;


namespace Neon.Communication.Packets.Outgoing.Rooms.Engine
{
    internal class ObjectUpdateComposer : ServerPacket
    {
        public ObjectUpdateComposer(Item Item, int UserId)
            : base(ServerPacketHeader.ObjectUpdateMessageComposer)
        {
            base.WriteInteger(Item.Id);
            base.WriteInteger(Item.GetBaseItem().SpriteId);
            base.WriteInteger(Item.GetX);
            base.WriteInteger(Item.GetY);
            base.WriteInteger(Item.Rotation);
            base.WriteString(string.Format("{0:0.00}", TextHandling.GetString(Item.GetZ)));
            base.WriteString(string.Empty);

            if (Item.LimitedNo > 0)
            {
                base.WriteInteger(1);
                base.WriteInteger(256);
                base.WriteString(Item.ExtraData);
                base.WriteInteger(Item.LimitedNo);
                base.WriteInteger(Item.LimitedTot);
            }
            else if (Item.Data.InteractionType == InteractionType.INFO_TERMINAL || Item.Data.InteractionType == InteractionType.ROOM_PROVIDER)
            {
                base.WriteInteger(0);
                base.WriteInteger(1);
                base.WriteInteger(1);
                base.WriteString("internalLink");
                base.WriteString(Item.ExtraData);
            }
            else if (Item.Data.InteractionType == InteractionType.FX_PROVIDER)
            {
                base.WriteInteger(0);
                base.WriteInteger(1);
                base.WriteInteger(1);
                base.WriteString("effectId");
                base.WriteString(Item.ExtraData);
            }

            else if (Item.Data.InteractionType == InteractionType.PINATA)
            {
                base.WriteInteger(0);
                base.WriteInteger(7);
                if (Item.ExtraData.Length <= 0)
                {
                    base.WriteString("6");
                    base.WriteInteger(0);
                }
                else
                {
                    base.WriteString((int.Parse(Item.ExtraData) == 1) ? "8" : "6");
                    base.WriteInteger(int.Parse(Item.ExtraData));
                }
                base.WriteInteger(1);
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

            else if (Item.Data.InteractionType == InteractionType.PINATATRIGGERED)
            {
                base.WriteInteger(0);
                base.WriteInteger(7);
                base.WriteString((Item.ExtraData.Length <= 0) ? "0" : "2");
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
                base.WriteInteger(23);
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

            base.WriteInteger(-1); // to-do: check
            base.WriteInteger((Item.GetBaseItem().Modes > 1) ? 1 : 0);
            base.WriteInteger(UserId);
        }
    }

    internal class UpdateFootBallComposer : ServerPacket
    {
        public UpdateFootBallComposer(Item Item, int newX, int newY)
            : base(ServerPacketHeader.ObjectUpdateMessageComposer)
        {
            WriteInteger(Item.Id);
            WriteInteger(Item.GetBaseItem().SpriteId);
            WriteInteger(newX);
            WriteInteger(newY);
            WriteInteger(4); // rot;
            WriteString((string.Format("{0:0.00}", TextHandling.GetString(Item.GetZ))));
            WriteString((string.Format("{0:0.00}", TextHandling.GetString(Item.GetZ))));
            WriteInteger(0);
            WriteInteger(0);
            WriteString(Item.ExtraData);
            WriteInteger(-1);
            WriteInteger(0);
            WriteInteger(Item.UserID);
        }
    }
}