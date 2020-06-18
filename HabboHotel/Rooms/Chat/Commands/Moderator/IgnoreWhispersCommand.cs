using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class IgnoreWhispersCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_ignore_whispers"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Le permite ignorar todos los murmullos en la sala , a excepción de su propia"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            Session.GetHabbo().IgnorePublicWhispers = !Session.GetHabbo().IgnorePublicWhispers;
            Session.SendWhisper("Usted " + (Session.GetHabbo().IgnorePublicWhispers ? "ahora" : "ya no") + " Ignora los susurros de otros!");
        }
    }
}
