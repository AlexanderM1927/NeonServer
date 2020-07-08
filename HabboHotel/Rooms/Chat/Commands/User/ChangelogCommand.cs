using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.GameClients;
using System;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class ChangelogCommand : IChatCommand
    {
        public string PermissionRequired => "command_info";

        public string Parameters => "";

        public string Description => "Últimas actualizaciones de Neon.";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            int _cache = new Random().Next(0, 300);
            Session.SendMessage(new MassEventComposer("habbopages/changelogs.txt?" + _cache));
        }
    }
}