using log4net;
using Neon.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace Neon.HabboHotel.Global
{
    public class LanguageLocale
    {
        private readonly Dictionary<string, string> _values = new Dictionary<string, string>();

        private static readonly ILog log = LogManager.GetLogger("Neon.HabboHotel.Global.LanguageLocale");

        public LanguageLocale()
        {
            _values = new Dictionary<string, string>();

            Init();
        }

        internal static string Value(string v, object p)
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            if (_values.Count > 0)
            {
                _values.Clear();
            }

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `server_locale`");
                DataTable Table = dbClient.getTable();

                if (Table != null)
                {
                    foreach (DataRow Row in Table.Rows)
                    {
                        _values.Add(Row["key"].ToString(), Row["value"].ToString());
                    }
                }
            }

            log.Info(">> Language Manager -> READY!");
        }

        public string TryGetValue(string value)
        {
            return _values.ContainsKey(value) ? _values[value] : "Missing language locale for [" + value + "]";
        }
    }
}