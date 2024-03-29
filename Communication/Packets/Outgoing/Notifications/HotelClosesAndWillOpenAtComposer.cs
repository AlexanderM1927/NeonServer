﻿namespace Neon.Communication.Packets.Outgoing.Notifications
{
    internal class HotelClosesAndWillOpenAtComposer : ServerPacket
    {
        public HotelClosesAndWillOpenAtComposer(int Hour, int Minute, bool Closed)
            : base(ServerPacketHeader.HotelClosesAndWillOpenAtComposer)
        {
            base.WriteInteger(Hour);
            base.WriteInteger(Minute);
            base.WriteBoolean(true);
        }
    }
}