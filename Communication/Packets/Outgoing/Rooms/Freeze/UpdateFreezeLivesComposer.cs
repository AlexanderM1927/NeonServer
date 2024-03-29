﻿namespace Neon.Communication.Packets.Outgoing.Rooms.Freeze
{
    internal class UpdateFreezeLivesComposer : ServerPacket
    {
        public UpdateFreezeLivesComposer(int UserId, int FreezeLives)
            : base(ServerPacketHeader.UpdateFreezeLivesMessageComposer)
        {
            base.WriteInteger(UserId);
            base.WriteInteger(FreezeLives);
        }
    }
}
