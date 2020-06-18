using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.Communication.Packets.Outgoing.Moderation;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class DJAlert : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_djalert"; }
        }

        public string Parameters
        {
            get { return "%message%"; }
        }

        public string Description
        {
            get { return "Envía una alerta a todo el hotel de emisión."; }
        }

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
