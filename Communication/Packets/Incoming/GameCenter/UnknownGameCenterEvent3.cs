﻿using Neon.HabboHotel.GameClients;

namespace Neon.Communication.Packets.Incoming.GameCenter
{
    internal class UnknownGameCenterEvent3 : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int pop = Packet.PopInt();
        }
    }
}
