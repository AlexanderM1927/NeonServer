using System.Collections.Generic;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class RoomUnmuteCommand : IChatCommand
    {
        public string PermissionRequired => "command_unroommute";

        public string Parameters => "";

        public string Description => "Desmutea";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (!Room.RoomMuted)
            {
                Session.SendWhisper("Esta habitacion no se silencia.");
                return;
            }

            Room.RoomMuted = false;

            List<RoomUser> RoomUsers = Room.GetRoomUserManager().GetRoomUsers();
            if (RoomUsers.Count > 0)
            {
                foreach (RoomUser User in RoomUsers)
                {
                    if (User == null || User.GetClient() == null || User.GetClient().GetHabbo() == null || User.GetClient().GetHabbo().Username == Session.GetHabbo().Username)
                    {
                        continue;
                    }

                    User.GetClient().SendWhisper("Esta sala ha sido desmuteada, puedes volver a hablar con normalidad.");
                }
            }
        }
    }
}