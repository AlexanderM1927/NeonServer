using log4net;
using Neon.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace Neon.HabboHotel.Talents
{
    public class TalentTrackManager
    {
        private static readonly ILog log = LogManager.GetLogger("Neon.HabboHotel.Talents.TalentManager");

        private readonly Dictionary<int, TalentTrackLevel> _citizenshipLevels;

        public TalentTrackManager()
        {
            _citizenshipLevels = new Dictionary<int, TalentTrackLevel>();

            Init();
        }

        public void Init()
        {
            DataTable GetTable = null;
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `type`,`level`,`data_actions`,`data_gifts` FROM `talents`");
                GetTable = dbClient.getTable();
            }

            if (GetTable != null)
            {
                foreach (DataRow Row in GetTable.Rows)
                {
                    _citizenshipLevels.Add(Convert.ToInt32(Row["level"]), new TalentTrackLevel(Convert.ToString(Row["type"]), Convert.ToInt32(Row["level"]), Convert.ToString(Row["data_actions"]), Convert.ToString(Row["data_gifts"])));
                }
            }
        }

        public ICollection<TalentTrackLevel> GetLevels()
        {
            return _citizenshipLevels.Values;
        }
    }
}
