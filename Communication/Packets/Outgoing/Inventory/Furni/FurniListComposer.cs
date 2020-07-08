using Neon.HabboHotel.Catalog.Utilities;
using Neon.HabboHotel.Items;
using System.Collections.Generic;
using System.Linq;

namespace Neon.Communication.Packets.Outgoing.Inventory.Furni
{
    internal class FurniListComposer : ServerPacket
    {
        public FurniListComposer(List<Item> Items, ICollection<Item> Walls)
            : base(ServerPacketHeader.FurniListMessageComposer)
        {
            base.WriteInteger(1);
            base.WriteInteger(1);

            base.WriteInteger(Items.Count + Walls.Count);
            foreach (Item Item in Items.ToList())
            {
                WriteItem(Item);
            }

            foreach (Item Item in Walls.ToList())
            {
                WriteItem(Item);
            }
        }

        public FurniListComposer(List<Item> Items, int page, int pages)
            : base(ServerPacketHeader.FurniListMessageComposer)
        {
            WriteInteger(pages);
            WriteInteger(page);

            WriteInteger(Items.Count);
            foreach (Item Item in Items.ToList())
            {
                WriteItem(Item);
            }
        }

        public FurniListComposer()
           : base(ServerPacketHeader.FurniListMessageComposer)
        {
            WriteInteger(1);
            WriteInteger(1);

            WriteInteger(0);
        }

        private void WriteItem(Item Item)
        {
            WriteInteger(Item.Id);
            WriteString(Item.GetBaseItem().Type.ToString().ToUpper());
            WriteInteger(Item.Id);
            WriteInteger(Item.GetBaseItem().SpriteId);

            if (Item.LimitedNo > 0)
            {
                WriteInteger(1);
                WriteInteger(256);
                WriteString(Item.ExtraData);
                WriteInteger(Item.LimitedNo);
                WriteInteger(Item.LimitedTot);
            }

            else
            {
                ItemBehaviourUtility.GenerateExtradata(Item, this);
            }

            WriteBoolean(Item.GetBaseItem().AllowEcotronRecycle);
            WriteBoolean(Item.GetBaseItem().AllowTrade);
            WriteBoolean(Item.LimitedNo == 0 && Item.GetBaseItem().AllowInventoryStack);
            WriteBoolean(ItemUtility.IsRare(Item));
            WriteInteger(-1);//Seconds to expiration.
            WriteBoolean(true);
            WriteInteger(-1);//Item RoomId

            if (!Item.IsWallItem)
            {
                WriteString(string.Empty);
                WriteInteger(0);
            }
        }
    }
}