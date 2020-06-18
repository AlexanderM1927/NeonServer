using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.Rooms;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator.Fun
{
    class AllAroundMeCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_allaroundme"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "¿Necesitas atencion? Pon a todos a mirarte"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            List<RoomUser> Users = Room.GetRoomUserManager().GetRoomUsers();
            foreach (RoomUser U in Users.ToList())
            {
                if (U == null || Session.GetHabbo().Id == U.UserId)
                    continue;

                U.MoveTo(User.X, User.Y, true);
            }
        }
    }
}
