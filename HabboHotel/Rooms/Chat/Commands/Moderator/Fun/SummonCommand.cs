using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.GameClients;
using Neon.Communication.Packets.Outgoing.Rooms.Session;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator.Fun
{
    class SummonCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_summon"; }
        }

        public string Parameters
        {
            get { return "%username%"; }
        }

        public string Description
        {
            get { return "Trae a un usuario a tu sala"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor introduce el nombre del usuario que deseas traer a la sala");
                return;
            }

            GameClient TargetClient = NeonEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Ocurrio un error, no se encuentra al usuario o tal vez no esta online");
                return;
            }

            if (TargetClient.GetHabbo() == null)
            {
                Session.SendWhisper("Ocurrio un error, no se encuentra al usuario o tal vez no esta online");
                return;
            }

            if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
            {
                Session.SendWhisper("Get a life.");
                return;
            }

            TargetClient.SendNotification("Usted ha sido traido por " + Session.GetHabbo().Username + "!");
            if (!TargetClient.GetHabbo().InRoom)
                TargetClient.SendMessage(new RoomForwardComposer(Session.GetHabbo().CurrentRoomId));
            else
                TargetClient.GetHabbo().PrepareRoom(Session.GetHabbo().CurrentRoomId, "");
        }
    }
}