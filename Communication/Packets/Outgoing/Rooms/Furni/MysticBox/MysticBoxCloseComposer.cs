using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Neon.HabboHotel.Items.Crafting;

namespace Neon.Communication.Packets.Outgoing.Rooms.Furni
{
    class MysticBoxCloseComposer : ServerPacket
    {
        public MysticBoxCloseComposer()
            : base(ServerPacketHeader.MysticBoxCloseComposer)
        {
        }
    }
}