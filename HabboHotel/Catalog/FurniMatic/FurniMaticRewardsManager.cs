﻿using Neon.Database.Interfaces;
using Neon.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Neon.HabboHotel.Catalog.FurniMatic
{
    public class FurniMaticRewardsManager
    {
        private List<FurniMaticRewards> Rewards;
        public List<FurniMaticRewards> GetRewards() { return Rewards; }
        public List<FurniMaticRewards> GetRewardsByLevel(int level)
        {
            List<FurniMaticRewards> rewards = new List<FurniMaticRewards>();
            foreach (FurniMaticRewards reward in Rewards.Where(furni => furni.Level == level))
            {
                rewards.Add(reward);
            }

            return rewards;
        }

        public FurniMaticRewards GetRandomReward()
        {
            int level = 0;
            int rand5 = RandomNumber.GenerateRandom(1, 2000);
            if (rand5 == 1999)
            {
                level = 5;
                List<FurniMaticRewards> Reward = GetRewardsByLevel(level);
                int prize5 = RandomNumber.GenerateRandom(1, 2);
                if (prize5 == 1) { return Reward[0]; }
                else
                {
                    return Reward[1];
                }
            }

            int rand4 = RandomNumber.GenerateRandom(1, 250);
            if (rand4 == 199)
            {
                level = 4;
                List<FurniMaticRewards> Reward = GetRewardsByLevel(level);
                int prize4 = RandomNumber.GenerateRandom(1, 2);
                if (prize4 == 1) { return Reward[0]; }
                else
                {
                    return Reward[1];
                }
            }

            int rand3 = RandomNumber.GenerateRandom(1, 100);
            if (rand3 == 80)
            {
                level = 3;
                List<FurniMaticRewards> Reward = GetRewardsByLevel(level);
                int prize3 = RandomNumber.GenerateRandom(1, 4);
                if (prize3 == 1) { return Reward[0]; }
                if (prize3 == 2) { return Reward[1]; }
                if (prize3 == 3) { return Reward[2]; }
                if (prize3 == 4) { return Reward[3]; }
            }

            int rand2 = RandomNumber.GenerateRandom(1, 25);
            if (rand2 == 20)
            {
                level = 2;
                List<FurniMaticRewards> Reward = GetRewardsByLevel(level);
                int prize2 = RandomNumber.GenerateRandom(1, 3);
                if (prize2 == 1) { return Reward[0]; }
                if (prize2 == 2) { return Reward[1]; }
                if (prize2 == 3) { return Reward[2]; }
            }

            else
            {
                level = 1;
            }

            List<FurniMaticRewards> possibleRewards = GetRewardsByLevel(level);
            if (possibleRewards != null && possibleRewards.Count >= 1)
            {
                return possibleRewards[new Random().Next(0, (possibleRewards.Count - 1))];
            }
            else
            {
                return new FurniMaticRewards(0, 470, 0);
            }
        }

        public void Initialize(IQueryAdapter dbClient)
        {
            Rewards = new List<FurniMaticRewards>();
            dbClient.SetQuery("SELECT display_id, item_id, reward_level FROM ecotron_rewards");
            DataTable table = dbClient.getTable();
            if (table == null)
            {
                return;
            }

            foreach (DataRow row in table.Rows)
            {
                Rewards.Add(new FurniMaticRewards(Convert.ToInt32(row["display_id"]), Convert.ToInt32(row["item_id"]), Convert.ToInt32(row["reward_level"])));
            }
        }
    }
}