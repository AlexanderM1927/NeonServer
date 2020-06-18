using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    class RegenMaps : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_regen_maps"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Regenerar el mapa de la sala en la que estás."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (!Room.CheckRights(Session, true))
            {
                Session.SendWhisper("Oops, solo el dueño de la sala puede ejecutar este comando!");
                return;
            }

            Room.GetGameMap().GenerateMaps();
            Session.SendWhisper("Excelente, el mapa del juego ha sido regenerado.");
        }
    }
}
