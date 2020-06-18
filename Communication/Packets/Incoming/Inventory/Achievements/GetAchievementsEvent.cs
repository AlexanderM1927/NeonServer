using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.Communication.Packets.Outgoing.Inventory.Achievements;

namespace Neon.Communication.Packets.Incoming.Inventory.Achievements
{
    class GetAchievementsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new AchievementsComposer(Session, NeonEnvironment.GetGame().GetAchievementManager()._achievements.Values.ToList()));
        }
    }
}
