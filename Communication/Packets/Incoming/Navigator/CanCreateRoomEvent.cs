﻿
using Neon.Communication.Packets.Outgoing.Navigator;

namespace Neon.Communication.Packets.Incoming.Navigator
{
    internal class CanCreateRoomEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new CanCreateRoomComposer(false, 150));
        }
    }
}
