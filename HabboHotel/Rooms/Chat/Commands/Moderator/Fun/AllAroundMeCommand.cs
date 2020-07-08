using System.Collections.Generic;
using System.Linq;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator.Fun
{
    internal class AllAroundMeCommand : IChatCommand
    {
        public string PermissionRequired => "command_allaroundme";

        public string Parameters => "";

        public string Description => "¿Necesitas atencion? Pon a todos a mirarte";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
            {
                return;
            }

            List<RoomUser> Users = Room.GetRoomUserManager().GetRoomUsers();
            foreach (RoomUser U in Users.ToList())
            {
                if (U == null || Session.GetHabbo().Id == U.UserId)
                {
                    continue;
                }

                U.MoveTo(User.X, User.Y, true);
            }
        }
    }
}
