namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class DNDCommand : IChatCommand
    {
        public string PermissionRequired => "command_dnd";

        public string Parameters => "";

        public string Description => "Activar o desactivar los mensajes de consola.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            Session.GetHabbo().AllowConsoleMessages = !Session.GetHabbo().AllowConsoleMessages;
            Session.SendWhisper("Usted " + (Session.GetHabbo().AllowConsoleMessages == true ? "ahora" : "ya no") + " acepta mensajes en su consola de amigos.");
        }
    }
}