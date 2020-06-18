using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neon.Communication.Packets.Outgoing.Rooms.Notifications
{
    class AlertNotificationHCMessageComposer : ServerPacket
    {
        public AlertNotificationHCMessageComposer(int type)
            : base(ServerPacketHeader.AlertNotificationHCMessageComposer)
        {
            base.WriteInteger(type);
        }
    }
}
