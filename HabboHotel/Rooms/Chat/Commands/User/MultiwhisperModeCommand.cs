using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class MultiwhisperModeCommand : IChatCommand
    {
        public string PermissionRequired => "command_enable_friends";

        public string Parameters => "";

        public string Description => "Activar o desactivar las solicitudes de amistad.";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            Session.GetHabbo().MultiWhisper = !Session.GetHabbo().MultiWhisper;
            Session.SendWhisper("Ahora mismo " + (Session.GetHabbo().MultiWhisper == true ? "no aceptas" : "aceptas") + " nuevas peticiones de amistad");
        }
    }
}