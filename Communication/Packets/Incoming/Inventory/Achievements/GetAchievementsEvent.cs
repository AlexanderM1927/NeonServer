using Neon.Communication.Packets.Outgoing.Inventory.Achievements;
using System.Linq;

namespace Neon.Communication.Packets.Incoming.Inventory.Achievements
{
    internal class GetAchievementsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new AchievementsComposer(Session, NeonEnvironment.GetGame().GetAchievementManager()._achievements.Values.ToList()));
        }
    }
}
