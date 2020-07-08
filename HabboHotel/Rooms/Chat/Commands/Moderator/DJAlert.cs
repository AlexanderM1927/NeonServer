using Neon.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class DJAlert : IChatCommand
    {
        public string PermissionRequired => "command_djalert";

        public string Parameters => "%message%";

        public string Description => "Envía una alerta a todo el hotel de emisión.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor escribe el mensaje a enviar");
                return;
            }
            string Message = CommandManager.MergeParams(Params, 1);
            NeonEnvironment.GetGame().GetClientManager().SendMessage(RoomNotificationComposer.SendBubble("DJAlertNEW", "¡DJ " + Session.GetHabbo().Username + " está emitiendo en vivo! Sintoniza RadioFM ahora mismo y disfruta al máximo.", ""));
            return;
        }
    }
}
