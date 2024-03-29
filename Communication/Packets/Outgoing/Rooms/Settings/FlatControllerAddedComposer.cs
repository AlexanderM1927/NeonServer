﻿namespace Neon.Communication.Packets.Outgoing.Rooms.Settings
{
    internal class FlatControllerAddedComposer : ServerPacket
    {
        public FlatControllerAddedComposer(int RoomId, int UserId, string Username)
            : base(ServerPacketHeader.FlatControllerAddedMessageComposer)
        {
            base.WriteInteger(RoomId);
            base.WriteInteger(UserId);
            base.WriteString(Username);
        }
    }
}
