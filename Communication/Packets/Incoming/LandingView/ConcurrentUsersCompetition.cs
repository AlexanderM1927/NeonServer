using Neon.Communication.Packets.Outgoing.LandingView;
using Neon.HabboHotel.GameClients;
using System.Linq;

namespace Neon.Communication.Packets.Incoming.LandingView
{
    internal class ConcurrentUsersCompetition : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int goal = int.Parse(NeonEnvironment.GetDBConfig().DBData["usersconcurrent_goal"]); ;
            int UsersOnline = NeonEnvironment.GetGame().GetClientManager().Count;
            foreach (GameClient Target in NeonEnvironment.GetGame().GetClientManager().GetClients.ToList())
            {
                if (UsersOnline < goal)
                {
                    int type = 1;
                    Target.SendMessage(new ConcurrentUsersGoalProgressComposer(UsersOnline, type, goal));
                }
                else if (!Target.GetHabbo().GetStats().PurchaseUsersConcurrent && UsersOnline >= goal)
                {
                    int type = 2;
                    Target.SendMessage(new ConcurrentUsersGoalProgressComposer(UsersOnline, type, goal));
                }
                else if (Target.GetHabbo().GetStats().PurchaseUsersConcurrent && UsersOnline >= goal)
                {
                    int type = 3;
                    Target.SendMessage(new ConcurrentUsersGoalProgressComposer(UsersOnline, type, goal));
                }
                else
                {
                    int type = 0;
                    Target.SendMessage(new ConcurrentUsersGoalProgressComposer(UsersOnline, type, goal));
                }
            }
        }
    }
}
