﻿using log4net;
using Neon.Database.Interfaces;
using System.Collections.Generic;
using System.Data;

namespace Neon.HabboHotel.Items.Crafting
{
    public class CraftingManager
    {
        private static readonly ILog log = LogManager.GetLogger("Neon.HabboHotel.Items.Crafting.CraftingManager");
        internal Dictionary<string, CraftingRecipe> CraftingRecipes;
        internal List<string> CraftableItems;

        public CraftingManager()
        {
            CraftingRecipes = new Dictionary<string, CraftingRecipe>();
            CraftableItems = new List<string>();
        }

        internal void Init()
        {
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                CraftingRecipes.Clear();
                dbClient.SetQuery("SELECT * FROM crafting_recipes");
                DataTable recipes = dbClient.getTable();
                foreach (DataRow recipe in recipes.Rows)
                {
                    CraftingRecipe value = new CraftingRecipe((string)recipe["id"], (string)recipe["items"], (string)recipe["result"], (int)recipe["type"]);
                    CraftingRecipes.Add((string)recipe["id"], value);
                }

                CraftableItems.Clear();
                dbClient.SetQuery("SELECT * FROM crafting_items");
                DataTable items = dbClient.getTable();
                foreach (DataRow item in items.Rows)
                {
                    CraftableItems.Add((string)item["itemName"]);
                }
            }

            log.Info(">> Crafting Manager -> READY!");
        }

        internal CraftingRecipe GetRecipe(string name)
        {
            if (CraftingRecipes.ContainsKey(name))
            {
                return CraftingRecipes[name];
            }
            else
            {
                return null;
            }
        }

        internal CraftingRecipe GetRecipeByPrize(string name)
        {
            foreach (CraftingRecipe c in CraftingRecipes.Values)
            {
                if (c.Result == name)
                {
                    return c;
                }
            }
            return null;
        }
    }
}