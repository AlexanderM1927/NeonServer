using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.GameClients;


namespace Neon.HabboHotel.Rooms.Chat.Commands.Events
{
    internal class PubliAlert : IChatCommand
    {
        public string PermissionRequired => "command_publi_alert";
        public string Parameters => "%message%";
        public string Description => "Manda un Evento a todo el Hotel!";
        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            string Message = CommandManager.MergeParams(Params, 1);
            NeonEnvironment.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("Se ha abierto oleada de publicidad..",
                 "¡Hay una nueva oleada de publicidad en activo! Si quieres ganar <b>distintas recompensas</b> por participar acude a la sala de publicidad.<br><br>¿Quién ha abierto la oleada? <b> <font color=\"#58ACFA\">  "
                 + Session.GetHabbo().Username + "</font></b><br>Si quieres participar haz click en el botón inferior de <b>Ir a la sala del evento</b>, y ahí dentro podrás participar.<br><br>¿De qué trata este evento?<br><br><font color='#084B8A'><b>Trata de seguir las instrucciones de los guías de la oleada para participar y así ganar tu premio!</b></font><br><br>¡Te esperamos!", "zpam", "Ir a la sala de la oleada", "event:navigator/goto/" + Session.GetHabbo().CurrentRoomId));

        }
    }
}

