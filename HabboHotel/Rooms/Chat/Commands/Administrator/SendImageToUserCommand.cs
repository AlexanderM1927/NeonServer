using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.GameClients;
using Neon.Communication.Packets.Outgoing.Notifications;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class SendImageToUserCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_alert_user"; }
        }

        public string Parameters
        {
            get { return "%usuario% %imagen%"; }
        }

        public string Description
        {
            get { return "Enviale un Mensaje de alerta a un usuario"; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor introduce el nombre del usuario al que le enviarás la alerta.");
                return;
            }

            GameClient TargetClient = NeonEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Ocurrió un error, al parecer no se consigue el usuario o no se encuentra en línea.");
                return;
            }

            if (TargetClient.GetHabbo() == null)
            {
                Session.SendWhisper("Ocurrió un error, al parecer no se consigue el usuario o no se encuentra en línea.");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 2);

            TargetClient.SendMessage(new GraphicAlertComposer(Message));
            Session.SendWhisper("Alerta enviada satisfactoriamente a " + TargetClient.GetHabbo().Username +".");

        }
    }
}
