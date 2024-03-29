﻿namespace Neon.Communication.Packets.Outgoing.Inventory.Trading
{
    internal class TradingConfirmedComposer : ServerPacket
    {
        public TradingConfirmedComposer(int UserId, bool Confirmed)
            : base(ServerPacketHeader.TradingConfirmedMessageComposer)
        {
            base.WriteInteger(UserId);
            base.WriteInteger(Confirmed ? 1 : 0);
        }
    }
}
