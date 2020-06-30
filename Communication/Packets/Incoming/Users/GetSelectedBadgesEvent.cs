using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.Users;
using Neon.Communication.Packets.Outgoing.Users;

namespace Neon.Communication.Packets.Incoming.Users
{
    class GetSelectedBadgesEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int UserId = Packet.PopInt();
            Habbo Habbo = NeonEnvironment.GetHabboById(UserId);
            if (Habbo == null)
                return;

            Session.GetHabbo().LastUserId = UserId;
            Session.SendMessage(new HabboUserBadgesComposer(Habbo));
        }
    }
}