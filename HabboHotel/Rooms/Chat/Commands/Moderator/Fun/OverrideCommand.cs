﻿namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator.Fun
{
    internal class OverrideCommand : IChatCommand
    {
        public string PermissionRequired => "command_override";

        public string Parameters => "";

        public string Description => "Obten la habilidad de caminar sobre cualquier cosa.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
            {
                return;
            }

            User.AllowOverride = !User.AllowOverride;
            Session.SendWhisper("Override mode updated.");
        }
    }
}
