using Neon.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class MassiveEventCommand : IChatCommand
    {
        public string PermissionRequired => "command_massevent";

        public string Parameters => "%event%";

        public string Description => "Ejecuta un EVENT a todos los usuarios en línea.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor escribe el event a lanzar.");
                return;
            }

            string Event = CommandManager.MergeParams(Params, 1);
            NeonEnvironment.GetGame().GetClientManager().SendMessage(new MassEventComposer(Event));
            return;
        }
    }
}
