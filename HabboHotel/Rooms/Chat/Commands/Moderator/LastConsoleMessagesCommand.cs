using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.Database.Interfaces;
using Neon.HabboHotel.GameClients;
using System;
using System.Data;
using System.Text;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class LastConsoleMessagesCommand : IChatCommand
    {
        public string PermissionRequired => "command_user_info";

        public string Parameters => "%username%";

        public string Description => "Consulta los últimos mensajes del usuario en la consola.";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Introduce el nombre del usuario que deseas ver revisar su información.");
                return;
            }

            DataRow UserData = null;
            string Username = Params[1];

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `username` FROM users WHERE `username` = @Username LIMIT 1");
                dbClient.AddParameter("Username", Username);
                UserData = dbClient.getRow();
            }

            if (UserData == null)
            {
                Session.SendMessage(new RoomCustomizedAlertComposer("No existe ningún usuario con el nombre " + Username + "."));
                return;
            }

            GameClient TargetClient = NeonEnvironment.GetGame().GetClientManager().GetClientByUsername(Username);
            StringBuilder HabboInfo = new StringBuilder();

            HabboInfo.Append("Estos son los últimos mensajes del usuario sospechoso, recuerda revisar siempre estos casos antes de proceder a banear a menos que sea un  caso evidente de spam.\n\n");

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `message` FROM `chatlogs_console` WHERE `user_id` = '" + TargetClient.GetHabbo().Id + "' ORDER BY `id` DESC LIMIT 10");
                DataTable GetLogs = dbClient.getTable();

                if (GetLogs == null)
                {
                    Session.SendMessage(new RoomCustomizedAlertComposer("Lamentablemente el usuario que has solicitado no tiene mensajes en el registro."));
                }

                else if (GetLogs != null)
                {
                    int Number = 11;
                    foreach (DataRow Log in GetLogs.Rows)
                    {
                        Number -= 1;
                        HabboInfo.Append("<font size ='8' color='#B40404'><b>[" + Number + "]</b></font>" + " " + Convert.ToString(Log["message"]) + "\r");
                    }
                }

                Session.SendMessage(new RoomNotificationComposer("Últimos mensajes de " + Username + ":", (HabboInfo.ToString()), "usr/body/" + Username + "", "", ""));

            }
        }
    }
}