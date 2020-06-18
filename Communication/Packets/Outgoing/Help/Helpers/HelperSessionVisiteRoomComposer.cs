using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neon.Communication.Packets.Outgoing.Help.Helpers
{
    class HelperSessionVisiteRoomComposer : ServerPacket
    {
        public HelperSessionVisiteRoomComposer(int roomId)
            : base(ServerPacketHeader.HelperSessionVisiteRoomMessageComposer)
        {
            base.WriteInteger(roomId);
        }
    }
}
