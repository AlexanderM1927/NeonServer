using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.Communication.Packets.Outgoing.Avatar;

namespace Neon.Communication.Packets.Incoming.Avatar
{
    class GetWardrobeEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new WardrobeComposer(Session));
        }
    }
}
