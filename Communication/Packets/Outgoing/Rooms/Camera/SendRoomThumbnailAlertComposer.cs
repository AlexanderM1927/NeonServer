using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neon.Communication.Packets.Outgoing.HabboCamera
{
    class ThumbnailSuccessMessageComposer : ServerPacket
    {        
        public ThumbnailSuccessMessageComposer()
            : base(ServerPacketHeader.ThumbnailSuccessMessageComposer)
        {

        }
    }
}
