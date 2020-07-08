using Neon.HabboHotel.Items;
using System.Collections.Generic;

namespace Neon.Communication.Packets.Outgoing.Inventory.Furni
{
    internal class FurniListNotificationComposer : ServerPacket
    {
        public FurniListNotificationComposer(List<Item> items, int Type)
            : base(ServerPacketHeader.FurniListNotificationMessageComposer)
        {
            WriteInteger(1);
            WriteInteger(Type);
            WriteInteger(items.Count);
            foreach (Item i in items)
            {
                WriteInteger(i.Id);
            }
        }

        public FurniListNotificationComposer(int Id, int Type)
            : base(ServerPacketHeader.FurniListNotificationMessageComposer)
        {
            WriteInteger(1);
            WriteInteger(Type);
            WriteInteger(1);
            WriteInteger(Id);
        }

    }
}