﻿namespace Neon.HabboHotel.Catalog
{
    using log4net;
    using Neon;
    using Neon.Database.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class FrontpageManager
    {
        private readonly Dictionary<int, Frontpage> _frontPageData = new Dictionary<int, Frontpage>();
        private readonly Dictionary<int, Frontpage> _bcfrontPageData = new Dictionary<int, Frontpage>();
        private static readonly ILog log = LogManager.GetLogger("Neon.HabboHotel.Catalog.FrontPage");

        public FrontpageManager()
        {
            LoadFrontPage();
        }

        public ICollection<Frontpage> GetCatalogFrontPage()
        {
            return _frontPageData.Values;
        }
        public ICollection<Frontpage> GetBCCatalogFrontPage()
        {
            return _bcfrontPageData.Values;
        }
        public void LoadFrontPage()
        {
            if (_frontPageData.Count > 0)
            {
                _frontPageData.Clear();
            }

            if (_bcfrontPageData.Count > 0)
            {
                _bcfrontPageData.Clear();
            }

            using (IQueryAdapter adapter = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                adapter.SetQuery("SELECT * FROM `catalog_frontpage`");
                DataTable table = adapter.getTable();
                if (table != null)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        _frontPageData.Add(Convert.ToInt32(row["id"]), new Frontpage(Convert.ToInt32(row["id"]), Convert.ToString(row["front_name"]), Convert.ToString(row["front_link"]), Convert.ToString(row["front_image"])));
                    }
                }
            }

            using (IQueryAdapter adapter = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                adapter.SetQuery("SELECT * FROM `catalog_bc_frontpage`");
                DataTable table = adapter.getTable();
                if (table != null)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        _bcfrontPageData.Add(Convert.ToInt32(row["id"]), new Frontpage(Convert.ToInt32(row["id"]), Convert.ToString(row["front_name"]), Convert.ToString(row["front_link"]), Convert.ToString(row["front_image"])));
                    }
                }
            }
        }
    }
}

