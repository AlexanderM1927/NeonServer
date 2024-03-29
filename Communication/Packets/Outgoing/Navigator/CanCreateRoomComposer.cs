﻿namespace Neon.Communication.Packets.Outgoing.Navigator
{
    internal class CanCreateRoomComposer : ServerPacket
    {
        public CanCreateRoomComposer(bool Error, int MaxRoomsPerUser)
            : base(ServerPacketHeader.CanCreateRoomMessageComposer)
        {
            base.WriteInteger(Error ? 1 : 0);
            base.WriteInteger(MaxRoomsPerUser);
        }
    }
}
