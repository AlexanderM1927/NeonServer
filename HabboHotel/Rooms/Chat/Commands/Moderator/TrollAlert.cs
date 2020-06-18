using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class TrollAlert : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_staff_alert"; }
        }

        public string Parameters
        {
            get { return "%message%"; }
        }

        public string Description
        {
            get { return "Enviale un mensaje de alerta a todos los staff online."; }
        }

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
