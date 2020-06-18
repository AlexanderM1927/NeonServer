using System.Linq;
using System.Collections.Generic;
using Neon.HabboHotel.Items;
using System.Drawing;

namespace Neon.Communication.Packets.Outgoing.Rooms.FloorPlan
{
    class FloorPlanFloorMapComposer : ServerPacket
    {
        public FloorPlanFloorMapComposer(List<Point> Items)
            : base(ServerPacketHeader.FloorPlanFloorMapMessageComposer)
        {
            base.WriteInteger(Items.Count);
            foreach (Point Item in Items.ToList())
            {
                base.WriteInteger(Item.X);
                base.WriteInteger(Item.Y);
            }
        }
    }
}