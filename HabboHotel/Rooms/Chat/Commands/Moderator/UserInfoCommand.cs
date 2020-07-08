using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.Database.Interfaces;
using Neon.HabboHotel.GameClients;
using System;
using System.Data;
using System.Text;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class UserInfoCommand : IChatCommand
    {
        public string PermissionRequired => "command_user_info";

        public string Parameters => "%username%";

        public string Description => "Ve la informacion del usuario";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Introduce el nombre del usuario que deseas ver revisar su información.");
                return;
            }

            DataRow UserData = null;
            DataRow UserInfo = null;
            string Username = Params[1];

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM users WHERE `username` = @Username LIMIT 1");
                dbClient.AddParameter("Username", Username);
                UserData = dbClient.getRow();
            }

            if (UserData == null)
            {
                Session.SendNotification("No existe ningún usuario con el nombre " + Username + ".");
                return;
            }

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `user_info` WHERE `user_id` = '" + Convert.ToInt32(UserData["id"]) + "' LIMIT 1");
                UserInfo = dbClient.getRow();
                if (UserInfo == null)
                {
                    dbClient.RunQuery("INSERT INTO `user_info` (`user_id`) VALUES ('" + Convert.ToInt32(UserData["id"]) + "')");

                    dbClient.SetQuery("SELECT * FROM `user_info` WHERE `user_id` = '" + Convert.ToInt32(UserData["id"]) + "' LIMIT 1");
                    UserInfo = dbClient.getRow();
                }
            }

            GameClient TargetClient = NeonEnvironment.GetGame().GetClientManager().GetClientByUsername(Username);

            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Convert.ToDouble(UserInfo["trading_locked"]));

            DateTime valecrack = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            valecrack = valecrack.AddSeconds(Session.GetHabbo().LastOnline).ToLocalTime();

            string time = valecrack.ToString();

            StringBuilder HabboInfo = new StringBuilder();//
            HabboInfo.Append("<font color='#0489B1'><b>Información General:</b></font>\r");
            HabboInfo.Append("<font size='10'>ID: " + Convert.ToInt32(UserData["id"]) + "\r");
            HabboInfo.Append("Rango: " + Convert.ToInt32(UserData["rank"]) + "\r");
            HabboInfo.Append("Guía: " + Convert.ToInt32(UserData["guia"]) + "\r");
            HabboInfo.Append("VIP: " + Convert.ToInt32(UserData["rank_vip"]) + "\r");
            HabboInfo.Append("Email: " + Convert.ToString(UserData["mail"]) + "\r");
            HabboInfo.Append("Online: " + (TargetClient != null ? "Sí" : "No") + "\r");
            HabboInfo.Append("Última conexión: " + time + "</font>\r\r");

            HabboInfo.Append("<font color='#0489B1'><b>Información Monetaria:</b></font>\r");
            HabboInfo.Append("<font size ='10'>Créditos:<font color='#F7D358'><b> " + Convert.ToInt32(UserData["credits"]) + "</b></font>\r");
            HabboInfo.Append("Duckets:<font color='#BF00FF'><b> " + Convert.ToInt32(UserData["activity_points"]) + "</b></font>\r");
            HabboInfo.Append("Diamantes:<font color='#2E9AFE'><b> " + Convert.ToInt32(UserData["vip_points"]) + "</b></font>\r");
            HabboInfo.Append("Puntos de Juego:<font color='#FE9A2E'><b> " + Convert.ToInt32(UserData["gotw_points"]) + "</font></b></font>\r\r");

            HabboInfo.Append("<font color='#0489B1'><b>Información Moderación:</b></font>\r");
            HabboInfo.Append("<font size='10'>Baneos: " + Convert.ToInt32(UserInfo["bans"]) + "\r");
            HabboInfo.Append("Reportes Enviados: " + Convert.ToInt32(UserInfo["cfhs"]) + "\r");
            HabboInfo.Append("Abuso: " + Convert.ToInt32(UserInfo["cfhs_abusive"]) + "\r");
            HabboInfo.Append("Bloq. tradeos: " + (Convert.ToInt32(UserInfo["trading_locked"]) == 0 ? "Ninguno." : "Expira: " + (origin.ToString("dd/MM/yyyy")) + "") + "\r");
            HabboInfo.Append("Total bloqueos: " + Convert.ToInt32(UserInfo["trading_locks_count"]) + "</font>\r\r");

            if (TargetClient != null)
            {
                HabboInfo.Append("<font color = '#0489B1'><b> Localización:</b></font>\r");
                if (!TargetClient.GetHabbo().InRoom)
                {
                    HabboInfo.Append("No se encuentra en ninguna sala.\r");
                }
                else
                {
                    HabboInfo.Append("<font size='10'>Sala: " + TargetClient.GetHabbo().CurrentRoom.Name + " (" + TargetClient.GetHabbo().CurrentRoom.RoomId + ")\r");
                    HabboInfo.Append("Dueño: " + TargetClient.GetHabbo().CurrentRoom.OwnerName + "\r");
                    HabboInfo.Append("Visitantes: " + TargetClient.GetHabbo().CurrentRoom.UserCount + " de " + TargetClient.GetHabbo().CurrentRoom.UsersMax);
                }
            }
            Session.SendMessage(new RoomNotificationComposer("Información de " + Username + ":", (HabboInfo.ToString()), "usr/body/" + Username + "", "", ""));
        }
    }
}
