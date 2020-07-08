using Neon.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class WiredVariable : IChatCommand
    {
        public string PermissionRequired => "command_stats";

        public string Parameters => "";

        public string Description => "Lista de variables en tu WIRED: Mensaje.";

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {

            Session.SendMessage(new MassEventComposer("habbopages/chat/wiredvars.txt"));
            return;

        }
    }
}
