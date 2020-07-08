using Neon.Communication.Packets.Outgoing.Rooms.Session;
using System.Collections.Generic;
using System.Linq;


namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class Reloadcommand : IChatCommand
    {
        public string PermissionRequired => "command_reload";

        public string Parameters => "";

        public string Description => "Recarga la sala";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().Id != Room.OwnerId && !Session.GetHabbo().GetPermissions().HasRight("room_any_owner"))
            {
                Session.SendWhisper("Lo sentimos, este comando solo está disponible si eres el propietario de la sala");
                return;
            }

            List<RoomUser> UsersToReturn = Room.GetRoomUserManager().GetRoomUsers().ToList();

            NeonEnvironment.GetGame().GetRoomManager().UnloadRoom(Room);


            foreach (RoomUser User in UsersToReturn)
            {
                if (User == null || User.GetClient() == null)
                {
                    continue;
                }

                User.GetClient().SendMessage(new RoomForwardComposer(Room.Id));
            }


        }
    }
}
