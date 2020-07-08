using log4net;
using Neon.Database.Interfaces;
using System.Data;

namespace Neon.HabboHotel.LandingView.CommunityGoal
{
    public class CommunityGoalVS
    {
        private static readonly ILog log = LogManager.GetLogger("Neon.HabboHotel.LandingView.CommunityGoalVS");

        private int Id;
        private string Name;
        private int LeftVotes;
        private int RightVotes;

        public void LoadCommunityGoalVS()
        {
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `landing_communitygoalvs` ORDER BY `id` DESC LIMIT 1");
                DataRow dRow = dbClient.getRow();

                if (dRow != null)
                {
                    Id = (int)dRow["id"];
                    Name = (string)dRow["name"];
                    LeftVotes = (int)dRow["left_votes"];
                    RightVotes = (int)dRow["right_votes"];
                }
            }
        }

        public int GetId()
        {
            return Id;
        }

        public string GetName()
        {
            return Name;
        }

        public int GetLeftVotes()
        {
            return LeftVotes;
        }

        public int GetRightVotes()
        {
            return RightVotes;
        }

        public void IncreaseLeftVotes()
        {
            LeftVotes++;
        }

        public void IncreaseRightVotes()
        {
            RightVotes++;
        }
    }
}
