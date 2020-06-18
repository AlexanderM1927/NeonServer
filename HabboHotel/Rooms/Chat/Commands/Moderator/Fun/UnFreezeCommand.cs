using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator.Fun
{
    class UnFreezeCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_unfreeze"; }
        }

        public string Parameters
        {
            get { return "%username%"; }
        }

        public string Description
        {
            get { return "Quita el congelamiento al usuario"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Introduce el nombre del usuario que deseas descongelar.");
                return;
            }

            GameClient TargetClient = NeonEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Ocurrio un error, al parecer no se consigue el usuario o no se encuentra online");
                return;
            }

            RoomUser TargetUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Params[1]);
            if (TargetUser != null)
                TargetUser.Frozen = false;

            Session.SendWhisper("Descongelado correctamente " + TargetClient.GetHabbo().Username + "!");
        }
    }
}
