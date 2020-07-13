﻿namespace Neon.Communication.Packets.Outgoing.Rooms.Furni
{
    internal class GnomeBoxComposer : ServerPacket
    {
        public GnomeBoxComposer(int ItemId)
            : base(ServerPacketHeader.GnomeBoxMessageComposer)
        {
            base.WriteInteger(ItemId);
        }
    }
}
