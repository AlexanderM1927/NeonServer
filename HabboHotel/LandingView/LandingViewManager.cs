﻿using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using Neon.Database.Interfaces;
using Neon.HabboHotel.LandingView.Promotions;
using log4net;
using Neon.Communication.Packets.Incoming.LandingView;

namespace Neon.HabboHotel.LandingView
{
    public class LandingViewManager
    {
        private static readonly ILog log = LogManager.GetLogger("Neon.HabboHotel.LandingView.LandingViewManager");

        internal BonusRareList BonusRareLists;

        private Dictionary<int, Promotion> _promotionItems;
        public Dictionary<uint, UserRank> ranks;
        public List<UserCompetition> usersWithRank;

        public LandingViewManager()
        {
            this._promotionItems = new Dictionary<int, Promotion>();

            this.LoadBonusRare(NeonEnvironment.GetDatabaseManager().GetQueryReactor());

            this.LoadPromotions();
        }
        public void LoadHallOfFame()
        {

            ranks = new Dictionary<uint, UserRank>();
            usersWithRank = new List<UserCompetition>();

            usersWithRank = new List<UserCompetition>();
            usersWithRank.Clear();
            using (IQueryAdapter queryReactor = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                ranks = new Dictionary<uint, UserRank>();
                usersWithRank = new List<UserCompetition>();

                queryReactor.SetQuery("SELECT * FROM `users` WHERE `gotw_points` >= '1' AND `rank` = '1' ORDER BY `gotw_points` DESC LIMIT 16");
                DataTable gUsersTable = queryReactor.getTable();

                foreach (DataRow Row in gUsersTable.Rows)
                {
                    var staff = new UserCompetition(Row);
                    if (!usersWithRank.Contains(staff))
                        usersWithRank.Add(staff);
                }
            }
        }

        public void LoadPromotions()
        {
            if (this._promotionItems.Count > 0)
                this._promotionItems.Clear();

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `server_landing` ORDER BY `id` DESC");
                DataTable GetData = dbClient.getTable();

                if (GetData != null)
                {
                    foreach (DataRow Row in GetData.Rows)
                    {
                        this._promotionItems.Add(Convert.ToInt32(Row[0]), new Promotion((int)Row[0], Row[1].ToString(), Row[2].ToString(), Row[3].ToString(), Convert.ToInt32(Row[4]), Row[5].ToString(), Row[6].ToString()));
                    }
                }
            }


            log.Info(">> LandingView Manager -> READY! ");
        }
        public void LoadBonusRare(IQueryAdapter dbClient)
        {

            BonusRareLists = null;

            dbClient.SetQuery("SELECT * FROM landing_bonus WHERE enable = 'true' LIMIT 1");
            var row = dbClient.getRow();

            if (row == null)
                return;

            BonusRareLists = new BonusRareList((string)row["item_name"], (int)row["base_item"], (int)row["bonus_score"]);
            log.Info("» Vista do hotel -> READY!");
        }


        public class BonusRareList
        {
            internal string Item;
            internal int Baseid, Score;
            internal BonusRareList(string item, int baseid, int score)
            {
                Item = item;
                Baseid = baseid;
                Score = score;
            }
        }

        public ICollection<Promotion> GetPromotionItems()
        {
            return this._promotionItems.Values;
        }
    }
}