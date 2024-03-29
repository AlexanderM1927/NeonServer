﻿namespace Neon.Communication.Packets.Outgoing.Rooms.Engine
{
    internal class RoomPropertyComposer : ServerPacket
    {
        public RoomPropertyComposer(string name, string val)
            : base(ServerPacketHeader.RoomPropertyMessageComposer)
        {
            base.WriteString(name);
            base.WriteString(val);
        }
    }
}
