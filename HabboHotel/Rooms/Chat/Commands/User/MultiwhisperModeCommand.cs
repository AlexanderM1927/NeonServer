using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Neon.Database.Interfaces;
using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    class MultiwhisperModeCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_enable_friends"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Activar o desactivar las solicitudes de amistad."; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            Session.GetHabbo().MultiWhisper = !Session.GetHabbo().MultiWhisper;
            Session.SendWhisper("Ahora mismo " + (Session.GetHabbo().MultiWhisper == true ? "no aceptas" : "aceptas") + " nuevas peticiones de amistad");
        }
    }
}