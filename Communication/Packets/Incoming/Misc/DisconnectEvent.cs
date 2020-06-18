﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Neon.Communication.Packets.Incoming.Misc
{
    class DisconnectEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.Disconnect();
        }
    }
}