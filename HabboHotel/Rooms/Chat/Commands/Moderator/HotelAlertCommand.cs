
using Neon.Communication.Packets.Outgoing.Moderation;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class HotelAlertCommand : IChatCommand
    {
        public string PermissionRequired => "command_hotel_alert";

        public string Parameters => "%message%";

        public string Description => "Envia un mensaje a todo el Hotel";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor escribe el mensaje a enviar");
                return;
            }
            string Message = CommandManager.MergeParams(Params, 1);
            if (NeonEnvironment.GetDBConfig().DBData["hotel.name"] == "Habbi")
            {
                NeonEnvironment.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("Mensaje de " + Session.GetHabbo().Username + ":", "<font size =\"11\">Querido usuario de " + NeonEnvironment.GetDBConfig().DBData["hotel.name"] + ", el usuario " + Session.GetHabbo().Username + " tiene un mensaje para todo el hotel:</font><br><br><font size =\"11\" color=\"#B40404\">" + Message + "</font><br><br><font size =\"10\" color=\"#0B4C5F\">Recuerda estar atent@ a las redes sociales para mantenerte siempre al d\x00eda de las actualizaciones en Habbi Hotel:<br><br><b>FACEBOOK</b>: @EsHabbiHotel<br><b>TWITTER</b>: @EsHabbi<br><b>INSTAGRAM:</b> @EsHabbi</font>", "alertz", ""));
            }
            else
            {
                NeonEnvironment.GetGame().GetClientManager().SendMessage(new BroadcastMessageAlertComposer(NeonEnvironment.GetGame().GetLanguageLocale().TryGetValue("hotelalert_text") + Message + "\r\n- " + Session.GetHabbo().Username, ""));
            }

            return;
        }
    }
}
