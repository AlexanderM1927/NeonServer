using System;
using Neon.HabboHotel.GameClients;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    class ChangelogCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_info"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Últimas actualizaciones de Neon."; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            var _cache = new Random().Next(0, 300);
            Session.SendMessage(new MassEventComposer("habbopages/changelogs.txt?" + _cache));
        }
    }
}