using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neon.Communication.Packets.Outgoing.Catalog
{
    public class LTDSoldAlertComposer : ServerPacket
    {
        public LTDSoldAlertComposer()
            : base(ServerPacketHeader.LTDSoldAlertComposer)
        {
            base.WriteInteger(0);
        }
    }
}
