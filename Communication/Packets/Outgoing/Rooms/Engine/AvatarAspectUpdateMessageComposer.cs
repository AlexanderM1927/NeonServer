using System;
using System.Linq;
using System.Text;

using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.GameClients;

namespace Neon.Communication.Packets.Outgoing.Rooms.Engine
{
    class AvatarAspectUpdateMessageComposer : ServerPacket
    {
        public AvatarAspectUpdateMessageComposer(string Figure, string Gender)
            : base(ServerPacketHeader.AvatarAspectUpdateMessageComposer)
        {
            base.WriteString(Figure);
            base.WriteString(Gender);

        }
    }
}