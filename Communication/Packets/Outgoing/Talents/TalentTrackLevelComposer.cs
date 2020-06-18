using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Neon.Communication.Packets.Outgoing.Talents
{
    class TalentTrackLevelComposer : ServerPacket
    {
        public TalentTrackLevelComposer(string type)
            : base(ServerPacketHeader.TalentTrackLevelMessageComposer)
        {
            base.WriteString(type);
            base.WriteInteger(4);
            base.WriteInteger(4);
        }
    }
}