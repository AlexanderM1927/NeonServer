using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.Items;

namespace Neon.Communication.Packets.Outgoing.Rooms.Engine
{
    class RoomSpectatorComposer : ServerPacket
    {
        public RoomSpectatorComposer()
            : base(ServerPacketHeader.RoomSpectatorComposer)
        {
        }
    }
}
