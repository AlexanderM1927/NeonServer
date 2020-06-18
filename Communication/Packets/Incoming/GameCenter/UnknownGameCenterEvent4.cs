using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.Games;
using Neon.Communication.Packets.Outgoing.GameCenter;
using System.Data;

using Neon.HabboHotel.Users;
using Neon.HabboHotel.GameClients;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Neon.Communication.Packets.Incoming.GameCenter
{
    class UnknownGameCenterEvent4 : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int pop = Packet.PopInt();
        }
    }
}
