using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    class DNDCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_dnd"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Activar o desactivar los mensajes de consola."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            Session.GetHabbo().AllowConsoleMessages = !Session.GetHabbo().AllowConsoleMessages;
            Session.SendWhisper("Usted " + (Session.GetHabbo().AllowConsoleMessages == true ? "ahora" : "ya no") + " acepta mensajes en su consola de amigos.");
        }
    }
}