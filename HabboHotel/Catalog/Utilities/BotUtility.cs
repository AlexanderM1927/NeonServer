﻿using Neon.Database.Interfaces;
using Neon.HabboHotel.Catalog;
using Neon.HabboHotel.Rooms.AI;
using Neon.HabboHotel.Users.Inventory.Bots;
using System;
using System.Data;



namespace Neon.HabboHotel.Items.Utilities
{
    public static class BotUtility
    {
        public static Bot CreateBot(ItemData Data, int OwnerId)
        {
            DataRow BotData = null;
            if (!NeonEnvironment.GetGame().GetCatalog().TryGetBot(Data.Id, out CatalogBot CataBot))
            {
                return null;
            }

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO bots (`user_id`,`name`,`motto`,`look`,`gender`,`ai_type`) VALUES ('" + OwnerId + "', '" + CataBot.Name + "', '" + CataBot.Motto + "', '" + CataBot.Figure + "', '" + CataBot.Gender + "', '" + CataBot.AIType + "')");
                int Id = Convert.ToInt32(dbClient.InsertQuery());

                dbClient.SetQuery("SELECT `id`,`user_id`,`name`,`motto`,`look`,`gender` FROM `bots` WHERE `user_id` = '" + OwnerId + "' AND `id` = '" + Id + "' LIMIT 1");
                BotData = dbClient.getRow();
            }

            return new Bot(Convert.ToInt32(BotData["id"]), Convert.ToInt32(BotData["user_id"]), Convert.ToString(BotData["name"]), Convert.ToString(BotData["motto"]), Convert.ToString(BotData["look"]), Convert.ToString(BotData["gender"]));
        }


        public static BotAIType GetAIFromString(string Type)
        {
            switch (Type)
            {
                case "pet":
                    return BotAIType.PET;
                case "generic":
                    return BotAIType.GENERIC;
                case "bartender":
                    return BotAIType.BARTENDER;
                case "visitor_logger":
                    return BotAIType.VISITORLOGGER;
                case "casino_bot":
                    return BotAIType.CASINO_BOT;
                case "roulette_bot":
                    return BotAIType.ROULLETE_BOT;
                case "rouleteducketbot":
                    return BotAIType.ROULETTEBOTDUCKET;
                case "rouletehonorbot":
                    return BotAIType.ROULETTEBOTHONOR;
                default:
                    return BotAIType.GENERIC;
            }
        }
    }
}