using log4net;
using Neon.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace Neon.HabboHotel.Badges
{
    public class BadgeManager
    {
        private static readonly ILog log = LogManager.GetLogger("Neon.HabboHotel.Badges.BadgeManager");

        private readonly Dictionary<string, BadgeDefinition> _badges;

        public BadgeManager()
        {
            _badges = new Dictionary<string, BadgeDefinition>();
        }

        public void Init()
        {
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `badge_definitions`;");
                DataTable GetBadges = dbClient.getTable();

                foreach (DataRow Row in GetBadges.Rows)
                {
                    string BadgeCode = Convert.ToString(Row["code"]).ToUpper();

                    if (!_badges.ContainsKey(BadgeCode))
                    {
                        _badges.Add(BadgeCode, new BadgeDefinition(BadgeCode, Convert.ToString(Row["required_right"])));
                    }
                }
            }

            //log.Info(">> Badge Manager with " + this._badges.Count + " badges loaded -> READY!");
            log.Info(">> Badge Manager -> READY!");
        }

        public bool TryGetBadge(string BadgeCode, out BadgeDefinition Badge)
        {
            return _badges.TryGetValue(BadgeCode.ToUpper(), out Badge);
        }
    }
}