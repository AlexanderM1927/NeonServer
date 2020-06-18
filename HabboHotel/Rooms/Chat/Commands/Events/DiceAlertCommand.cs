﻿using Neon.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class DiceAlertCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_da2_alert"; }
        }

        public string Parameters
        {
            get { return "%message%"; }
        }

        public string Description
        {
            get { return "Envía una alerta a todo el hotel de dados."; }
        }

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {
            if (Params[1] == "on")
            {
                NeonEnvironment.GetGame().GetClientManager().SendMessage(RoomNotificationComposer.SendBubble("DiceAlert", "¡El inter " + Session.GetHabbo().Username + " ha abierto los dados oficiales de Keko. Escribe :follow " + Session.GetHabbo().Username + "", ""));
                return;
            }
            else
                Session.SendWhisper("Por favor escribe el mensaje a enviar");
        }
    }
}