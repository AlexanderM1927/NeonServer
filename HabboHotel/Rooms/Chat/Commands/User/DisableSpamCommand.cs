using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class DisableSpamCommand : IChatCommand
    {
        public string PermissionRequired => "command_disable_spam";

        public string Parameters => "";

        public string Description => "Activar o desactivar la capacidad de recibir spam.";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            Session.GetHabbo().AllowMessengerInvites = false;
            Session.SendWhisper("Usted " + (Session.GetHabbo().AllowMessengerInvites ? "ahora" : "ya no") + " recibe Spams de consola!");
        }
    }
}
