using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.GameClients;


namespace Neon.HabboHotel.Rooms.Chat.Commands.Events
{
    internal class SpecialEvent : IChatCommand
    {
        public string PermissionRequired => "command_addpredesigned";
        public string Parameters => "%message%";
        public string Description => "Manda un evento a todo el hotel.";
        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            string Message = CommandManager.MergeParams(Params, 1);

            NeonEnvironment.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("¿Qué está pasando en " + NeonEnvironment.GetDBConfig().DBData["hotel.name"] + "...?",
                 "Algo está ocurriendo en Habbi, Custom, HiddenKey y Root han desaparecido en medio de la ceremonia...<br><br>Un ente susurra y pide ayuda a todo Habbi, parece que los espíritus reclaman la presencia de todos nuestros usuarios.<br></font></b><br>Si quieres colaborar haz click en el botón inferior y sigue las instrucciones.<br><br></font>", "2mesex", "¡A la aventura!", "event:navigator/goto/" + Session.GetHabbo().CurrentRoomId));

        }
    }
}

