﻿using Neon.Communication.Packets.Outgoing.Marketplace;
using Neon.Database.Interfaces;
using Neon.HabboHotel.Catalog.Marketplace;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace Neon.Communication.Packets.Incoming.Marketplace
{
    internal class GetOffersEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int MinCost = Packet.PopInt();
            int MaxCost = Packet.PopInt();
            string SearchQuery = Packet.PopString();
            int FilterMode = Packet.PopInt();


            DataTable table = null;
            StringBuilder builder = new StringBuilder();
            string str = "";
            builder.Append("WHERE `state` = '1' AND `timestamp` >= " + NeonEnvironment.GetGame().GetCatalog().GetMarketplace().FormatTimestampString());
            if (MinCost >= 0)
            {
                builder.Append(" AND `total_price` > " + MinCost);
            }

            if (MaxCost >= 0)
            {
                builder.Append(" AND `total_price` < " + MaxCost);
            }

            switch (FilterMode)
            {
                case 1:
                    str = "ORDER BY `asking_price` DESC";
                    break;

                default:
                    str = "ORDER BY `asking_price` ASC";
                    break;
            }

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {

                dbClient.SetQuery("SELECT `offer_id`, item_type, sprite_id, total_price, `limited_number`,`limited_stack` FROM catalog_marketplace_offers " + builder.ToString() + " " + str + " LIMIT 500");
                dbClient.AddParameter("search_query", "%" + SearchQuery + "%");
                if (SearchQuery.Length >= 1)
                {
                    builder.Append(" AND public_name LIKE @search_query");
                }
                table = dbClient.getTable();
            }

            NeonEnvironment.GetGame().GetCatalog().GetMarketplace().MarketItems.Clear();
            NeonEnvironment.GetGame().GetCatalog().GetMarketplace().MarketItemKeys.Clear();
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    if (!NeonEnvironment.GetGame().GetCatalog().GetMarketplace().MarketItemKeys.Contains(Convert.ToInt32(row["offer_id"])))
                    {
                        NeonEnvironment.GetGame().GetCatalog().GetMarketplace().MarketItemKeys.Add(Convert.ToInt32(row["offer_id"]));
                        NeonEnvironment.GetGame().GetCatalog().GetMarketplace().MarketItems.Add(new MarketOffer(Convert.ToInt32(row["offer_id"]), Convert.ToInt32(row["sprite_id"]), Convert.ToInt32(row["total_price"]), int.Parse(row["item_type"].ToString()), Convert.ToInt32(row["limited_number"]), Convert.ToInt32(row["limited_stack"])));
                    }
                }
            }

            Dictionary<int, MarketOffer> dictionary = new Dictionary<int, MarketOffer>();
            Dictionary<int, int> dictionary2 = new Dictionary<int, int>();

            foreach (MarketOffer item in NeonEnvironment.GetGame().GetCatalog().GetMarketplace().MarketItems)
            {
                if (dictionary.ContainsKey(item.SpriteId))
                {
                    if (item.LimitedNumber > 0)
                    {
                        if (!dictionary.ContainsKey(item.OfferID))
                        {
                            dictionary.Add(item.OfferID, item);
                        }

                        if (!dictionary2.ContainsKey(item.OfferID))
                        {
                            dictionary2.Add(item.OfferID, 1);
                        }
                    }
                    else
                    {
                        if (dictionary[item.SpriteId].TotalPrice > item.TotalPrice)
                        {
                            dictionary.Remove(item.SpriteId);
                            dictionary.Add(item.SpriteId, item);
                        }

                        int num = dictionary2[item.SpriteId];
                        dictionary2.Remove(item.SpriteId);
                        dictionary2.Add(item.SpriteId, num + 1);
                    }
                }
                else
                {
                    if (!dictionary.ContainsKey(item.SpriteId))
                    {
                        dictionary.Add(item.SpriteId, item);
                    }

                    if (!dictionary2.ContainsKey(item.SpriteId))
                    {
                        dictionary2.Add(item.SpriteId, 1);
                    }
                }
            }

            Session.SendMessage(new MarketPlaceOffersComposer(MinCost, MaxCost, dictionary, dictionary2));
        }
    }
}
