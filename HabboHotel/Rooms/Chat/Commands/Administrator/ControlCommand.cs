using System;
using Neon.HabboHotel.GameClients;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.Communication.Packets.Outgoing.Nux;
using Neon.Communication.Packets.Outgoing.Rooms.Furni.RentableSpaces;
using Neon.Communication.Packets.Outgoing.Moderation;
using Neon.Communication.Packets.Outgoing.Catalog;
using Neon.Communication.Packets.Outgoing.Users;
using Neon.Database.Interfaces;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    class ControlCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_control"; }
        }

        public string Parameters
        {
            get { return "<usuario>"; }
        }

        public string Description
        {
            get { return "Controla al usuario que selecciones."; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length != 2)
            {
                Session.SendWhisper("Introduce el nombre del usuario a quien deseas enviar una placa!", 34);
                return;
            }

            if (Params.Length == 2 && Params[1] == "end")
            {
                Session.SendWhisper("Has dejado de controlar a " + Session.GetHabbo().Opponent +".", 34);
                Session.GetHabbo().IsControlling = false;
                return;
            }

            GameClient TargetClient = NeonEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient != null)
            {
                Session.GetHabbo().Opponent = TargetClient.GetHabbo().Username;
                Session.GetHabbo().IsControlling = true;
                Session.SendMessage(RoomNotificationComposer.SendBubble("definitions", "Ahora estás controlando a " + TargetClient.GetHabbo().Username + ". Para parar di :control end."));
                return;
            }

            else Session.SendMessage(RoomNotificationComposer.SendBubble("definitions", "No se ha encontrado el usuario " + Params[1] + ".", ""));
        }
    }
}
