using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class TrollAlert : IChatCommand
    {
        public string PermissionRequired => "command_staff_alert";

        public string Parameters => "%message%";

        public string Description => "Enviale un mensaje de alerta a todos los staff online.";

        public void Execute(GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Escribe el mensaje que deseas enviar.");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 1);

            NeonEnvironment.GetGame().GetClientManager().StaffAlert(RoomNotificationComposer.SendBubble("estaff", "" + Message + "\n\n- " + Session.GetHabbo().Username + "", ""));
            return;


        }
    }
}
