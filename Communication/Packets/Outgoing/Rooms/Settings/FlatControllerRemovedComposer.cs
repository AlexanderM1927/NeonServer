using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.Rooms;

namespace Neon.Communication.Packets.Outgoing.Rooms.Settings
{
    class FlatControllerRemovedComposer : ServerPacket
    {
        public FlatControllerRemovedComposer(Room Instance, int UserId)
            : base(ServerPacketHeader.FlatControllerRemovedMessageComposer)
        {
            base.WriteInteger(Instance.Id);
            base.WriteInteger(UserId);
        }
    }
}
