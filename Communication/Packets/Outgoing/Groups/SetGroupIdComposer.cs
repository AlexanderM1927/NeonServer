﻿namespace Neon.Communication.Packets.Outgoing.Groups
{
    internal class SetGroupIdComposer : ServerPacket
    {
        public SetGroupIdComposer(int Id)
            : base(ServerPacketHeader.SetGroupIdMessageComposer)
        {
            base.WriteInteger(Id);
        }
    }
}
