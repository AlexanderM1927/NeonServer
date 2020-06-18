using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.Communication.Packets.Outgoing.Moderation;
using Neon.Communication.Packets.Outgoing.Notifications;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class ChangeLog : IChatCommand
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

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Escribe el mensaje que deseas enviar.");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 1);
            NeonEnvironment.GetGame().GetClientManager().StaffAlert(new MOTDNotificationComposer("[STAFF][" + Session.GetHabbo().Username + "]\r\r" + Message));
            return;

        }
    }
}
