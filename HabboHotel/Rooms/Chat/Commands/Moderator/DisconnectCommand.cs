using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class DisconnectCommand :IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_disconnect"; }
        }

        public string Parameters
        {
            get { return "%username%"; }
        }

        public string Description
        {
            get { return "Desconecta a cualquier usuario a la fuerza"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor, introduce el nombre del usuario que quieres desconectar.");
                return;
            }

            GameClient TargetClient = NeonEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Ocurrio un error, al parecer no se consigue el usuario o no se encuentra online");
                return;
            }

            if (TargetClient.GetHabbo().GetPermissions().HasRight("mod_tool") && !Session.GetHabbo().GetPermissions().HasRight("mod_disconnect_any"))
            {
                Session.SendWhisper("No tiene permitido desconectar a este usuario.");
                return;
            }

            TargetClient.GetConnection().Dispose();
        }
    }
}
