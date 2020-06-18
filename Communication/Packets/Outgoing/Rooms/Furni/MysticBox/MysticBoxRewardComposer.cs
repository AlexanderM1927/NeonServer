using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Neon.HabboHotel.Items.Crafting;

namespace Neon.Communication.Packets.Outgoing.Rooms.Furni
{
    class MysticBoxRewardComposer : ServerPacket
    {
        public MysticBoxRewardComposer(string type, int itemID)
            : base(ServerPacketHeader.MysticBoxRewardComposer)
        {
            base.WriteString(type);
            base.WriteInteger(itemID);
        }
    }
}