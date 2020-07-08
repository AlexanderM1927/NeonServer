using Neon.Database.Interfaces;

namespace Neon.Communication.Packets.Incoming.LandingView
{
    internal class VoteCommunityGoalVS : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int VoteType = Packet.PopInt(); // 1 izq, 2 der

            if (VoteType == 1)
            {
                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.runFastQuery("UPDATE landing_communitygoalvs SET left_votes = left_votes + 1 WHERE id = " + NeonEnvironment.GetGame().GetCommunityGoalVS().GetId());
                }

                NeonEnvironment.GetGame().GetCommunityGoalVS().IncreaseLeftVotes();
            }
            else if (VoteType == 2)
            {
                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.runFastQuery("UPDATE landing_communitygoalvs SET right_votes = right_votes + 1 WHERE id = " + NeonEnvironment.GetGame().GetCommunityGoalVS().GetId());
                }

                NeonEnvironment.GetGame().GetCommunityGoalVS().IncreaseRightVotes();
            }
            NeonEnvironment.GetGame().GetCommunityGoalVS().LoadCommunityGoalVS();
        }
    }
}
