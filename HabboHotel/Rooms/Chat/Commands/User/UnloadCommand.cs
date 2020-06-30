using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.Rooms;
using Neon.Communication.Packets.Outgoing.Rooms.Session;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    class UnloadCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_unload"; }
        }

        public string Parameters
        {
            get { return "id"; }
        }

        public string Description
        {
            get { return "Recarga la sala."; }
        }

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {
            if (Session.GetHabbo().GetPermissions().HasRight("room_unload_any"))
            {
                if (!NeonEnvironment.GetGame().GetRoomManager().TryGetRoom(Room.Id, out Room r))
                {
                    return;
                }

                List<RoomUser> UsersToReturn = Room.GetRoomUserManager().GetRoomUsers().ToList();
                NeonEnvironment.GetGame().GetRoomManager().UnloadRoom(r, true);

                foreach (RoomUser User in UsersToReturn)
                {
                    if (User != null)
                    {
                        User.GetClient().SendMessage(new RoomForwardComposer(Room.Id));
                    }
                }

                if (Room.HideWired == true)
                {
                    return;
                }
            }
            else
            {
                if (Room.CheckRights(Session, true))
                {
                    List<RoomUser> UsersToReturn = Room.GetRoomUserManager().GetRoomUsers().ToList();
                    NeonEnvironment.GetGame().GetRoomManager().UnloadRoom(Room);

                    foreach (RoomUser User in UsersToReturn)
                    {
                        if (User != null)
                        {
                            User.GetClient().SendMessage(new RoomForwardComposer(Room.Id));
                        }
                    }

                    if (Room.HideWired == true)
                    {
                        return;
                    }
                }
            }
        }
    }
}
