using log4net;
using Neon.Database.Interfaces;
using System.Collections.Generic;
using System.Data;

namespace Neon.HabboHotel.Calendar
{
    public class CalendarManager
    {
        private static readonly ILog log = LogManager.GetLogger("Neon.HabboHotel.Calendar.CalendarManager");

        private readonly string CampaignName;
        private double StartUnix;

        private readonly Dictionary<int, CalendarDay> CalendarDays;

        public string GetCampaignName()
        {
            return CampaignName;
        }

        public bool CampaignEnable()
        {
            return StartUnix > 0;
        }

        public CalendarDay GetCampaignDay(int Day)
        {
            if (CalendarDays.ContainsKey(Day))
            {
                return CalendarDays[Day];
            }

            return null;
        }

        public CalendarManager()
        {
            CampaignName = NeonEnvironment.GetDBConfig().DBData["advent.calendar.campaign"];
            StartUnix = 0;
            CalendarDays = new Dictionary<int, CalendarDay>();
        }

        public void Init()
        {
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                // Comprobamos en advent_calendar que exista esta campaña y obtenemos su tiempo de inicio.
                GetCalendarCampaignData(dbClient);

                // Si no está activado no cargamos los días.
                if (StartUnix == 0)
                {
                    return;
                }

                // Cargamos los premios de todos los días.
                LoadCampaignDays(dbClient);
            }

            log.Info(">> Calendar Manager -> READY!");
        }

        private void GetCalendarCampaignData(IQueryAdapter dbClient)
        {
            dbClient.SetQuery("SELECT start_unix FROM advent_calendar WHERE name = @name AND enable = '1'");
            dbClient.AddParameter("name", CampaignName);
            StartUnix = dbClient.getInteger();
        }

        private void LoadCampaignDays(IQueryAdapter dbClient)
        {
            DataTable GetData = null;
            dbClient.SetQuery("SELECT * FROM advent_calendar_gifts WHERE name = @name");
            dbClient.AddParameter("name", CampaignName);
            GetData = dbClient.getTable();

            if (GetData != null)
            {
                foreach (DataRow Row in GetData.Rows)
                {
                    int Day = (int)Row["day"];
                    string Gift = (string)Row["gift"];
                    string ProductName = (string)Row["productname"];
                    string ImageLink = (string)Row["imagelink"];
                    string ItemName = (string)Row["itemname"];

                    CalendarDays.Add(Day, new CalendarDay(Day, Gift, ProductName, ImageLink, ItemName));
                }
            }
        }

        public string GetGiftByDay(int Day)
        {
            if (CalendarDays.ContainsKey(Day))
            {
                return CalendarDays[Day].Gift;
            }

            return "";
        }

        public int GetTotalDays()
        {
            return CalendarDays.Count;
        }

        public int GetUnlockDays()
        {
            int Time = (int)(NeonEnvironment.GetUnixTimestamp() - StartUnix);
            return (((Time / 60) / 60) / 24);
        }
    }
}
