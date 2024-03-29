﻿namespace Neon.Communication.Packets.Outgoing.Moderation
{
    internal class BroadcastMessageAlertComposer : ServerPacket
    {
        public BroadcastMessageAlertComposer(string Message, string URL = "")
            : base(ServerPacketHeader.BroadcastMessageAlertMessageComposer)
        {
            base.WriteString(Message);
            base.WriteString(URL);
        }
    }
}

