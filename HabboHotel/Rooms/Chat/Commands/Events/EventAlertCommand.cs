using Neon.Communication.Packets.Outgoing.Rooms.Chat;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.Database.Interfaces;
using Neon.HabboHotel.GameClients;
using System;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Events
{
    internal class EventAlertCommand : IChatCommand
    {
        public string PermissionRequired => "command_event_alert";
        public string Parameters => "%message%";
        public string Description => "Abre un evento en todo el hotel.";
        public void Execute(GameClient Session, Room Room, string[] Params)
        {

            string Message = CommandManager.MergeParams(Params, 1);

            Session.GetHabbo()._eventsopened++;

            NeonEnvironment.GetGame().GetClientManager().SendEventType1(new RoomNotificationComposer("¡Nuevo evento!", "¡<b><font color=\"#2E9AFE\">" + Session.GetHabbo().Username + "</font></b> está organizando un nuevo evento en este momento! Si quieres ganar <font color=\"#f18914\"><b> Puntos de Juego </b></font> participa ahora mismo.<br><br>¿Quieres participar en este juego? ¡Haz click en el botón inferior de <b> Ir a la sala del evento</b>, y dentro podrás participar, sigue las instrucciones!<br><br>¿De qué trata este evento?<br><br><font color='#FF0040'><b>"
              + Message + "</b></font><br><br>¡Te esperamos con los brazos abiertos!<br><br>Recuerda que puedes cambiar esta alerta con el comando <b> :eventtype events 1</b> .", "eventoshabbi", "¡Ir a la sala del evento!", "event:navigator/goto/" + Session.GetHabbo().CurrentRoomId));

            NeonEnvironment.GetGame().GetClientManager().SendEventType2(new WhisperComposer(-1, "Hay un nuevo evento, haz <font color=\"#2E9AFE\"><a href='event:navigator/goto/" + Session.GetHabbo().CurrentRoomId + "'><b>click aquí</b></a></font> para ir al evento.", 0, 33));
            NeonEnvironment.GetGame().GetClientManager().SendEventType2(new WhisperComposer(-1, Message, 0, 33));
            NeonEnvironment.GetGame().GetClientManager().SendEventType2(new WhisperComposer(-1, "Evento organizado por: " + Session.GetHabbo().Username + ".", 0, 33));

            LogEvent(Session.GetHabbo().Id, Room.Id, Message);
        }

        public void LogEvent(int MasterID, int RoomID, string Message)
        {
            DateTime Now = DateTime.Now;
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO event_logs VALUES (NULL, " + MasterID + ", " + RoomID + ", @message, UNIX_TIMESTAMP())");
                dbClient.AddParameter("message", Message);
                dbClient.RunQuery();
            }
        }


    }
}

