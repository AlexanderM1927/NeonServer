﻿using log4net;
using Neon.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace Neon.HabboHotel.Items.Televisions
{
    public class TelevisionManager
    {
        private static readonly ILog log = LogManager.GetLogger("Neon.HabboHotel.Items.Televisions.TelevisionManager");

        public Dictionary<int, TelevisionItem> _televisions;

        public TelevisionManager()
        {
            _televisions = new Dictionary<int, TelevisionItem>();

            Init();
        }

        public void Init()
        {
            if (_televisions.Count > 0)
            {
                _televisions.Clear();
            }

            DataTable getData = null;
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `items_youtube` ORDER BY `id` DESC");
                getData = dbClient.getTable();

                if (getData != null)
                {
                    foreach (DataRow Row in getData.Rows)
                    {
                        _televisions.Add(Convert.ToInt32(Row["id"]), new TelevisionItem(Convert.ToInt32(Row["id"]), Row["youtube_id"].ToString(), Row["title"].ToString(), Row["description"].ToString(), NeonEnvironment.EnumToBool(Row["enabled"].ToString())));
                    }
                }
            }
        }


        public ICollection<TelevisionItem> TelevisionList => _televisions.Values;

        public bool TryGet(int ItemId, out TelevisionItem TelevisionItem)
        {
            if (_televisions.TryGetValue(ItemId, out TelevisionItem))
            {
                return true;
            }

            return false;
        }
    }
}
