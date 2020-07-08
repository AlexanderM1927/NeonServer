using log4net;
using Neon.Database.Interfaces;
using System;
using System.Threading;


namespace Neon.HabboHotel.Global
{
    public class ServerStatusUpdater : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger("Mango.Global.ServerUpdater");

        private const int UPDATE_IN_SECS = 30;
        private readonly string HotelName = NeonEnvironment.GetConfig().data["hotel.name"];

        private Timer _timer;

        public ServerStatusUpdater()
        {
        }

        public void Init()
        {
            _timer = new Timer(new TimerCallback(OnTick), null, TimeSpan.FromSeconds(UPDATE_IN_SECS), TimeSpan.FromSeconds(UPDATE_IN_SECS));

            Console.Title = "Neon - [0] ON - [0] ROOMS - [0] UPTIME";

            log.Info(">> Server Status -> READY!");
        }

        public void OnTick(object Obj)
        {
            UpdateOnlineUsers();
        }

        private void UpdateOnlineUsers()
        {
            TimeSpan Uptime = DateTime.Now - NeonEnvironment.ServerStarted;

            int UsersOnline = Convert.ToInt32(NeonEnvironment.GetGame().GetClientManager().Count);
            int RoomCount = NeonEnvironment.GetGame().GetRoomManager().Count;

            Console.Title = "Neon - [" + UsersOnline + "] ON - [" + RoomCount + "] ROOMS - [" + Uptime.Days + "] DAYS " + Uptime.Hours + "] HOURS";

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `server_status` SET `users_online` = @users, `loaded_rooms` = @loadedRooms LIMIT 1;");
                dbClient.AddParameter("users", UsersOnline);
                dbClient.AddParameter("loadedRooms", RoomCount);
                dbClient.RunQuery();
            }
        }


        public void Dispose()
        {
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE `server_status` SET `users_online` = '0', `loaded_rooms` = '0'");
            }

            _timer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
