using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Neon.Communication.Packets.Outgoing.Rooms.FloorPlan
{
    internal class FloorPlanFloorMapComposer : ServerPacket
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