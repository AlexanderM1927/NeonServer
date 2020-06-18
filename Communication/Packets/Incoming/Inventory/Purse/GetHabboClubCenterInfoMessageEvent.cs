using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.Global;
using Neon.Communication.Packets.Outgoing.Inventory.Purse;

namespace Neon.Communication.Packets.Incoming.Inventory.Purse
{
    class GetHabboClubCenterInfoMessageEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new GetHabboClubCenterInfoMessageComposer(Session));
        }
    }
}