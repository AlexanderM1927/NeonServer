using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Neon.HabboHotel.Items.Crafting;

namespace Neon.Communication.Packets.Outgoing.Rooms.Furni
{
    class MysticBoxOpenComposer : ServerPacket
    {
        public MysticBoxOpenComposer()
            : base(ServerPacketHeader.MysticBoxOpenComposer)
        {
        }
    }
}