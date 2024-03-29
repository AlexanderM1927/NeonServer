﻿using System;

namespace Neon.Communication.Packets.Outgoing.Rooms.Furni
{
    internal class UpdateMagicTileComposer : ServerPacket
    {
        public UpdateMagicTileComposer(int ItemId, int Decimal)
            : base(ServerPacketHeader.UpdateMagicTileMessageComposer)
        {
            base.WriteInteger(Convert.ToInt32(ItemId));
            base.WriteInteger(Decimal);
        }
    }
}
