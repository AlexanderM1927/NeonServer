using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.Rooms;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator.Fun
{
    class FastwalkCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_fastwalk"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Obten la habilidad de caminar Rapido"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            User.FastWalking = !User.FastWalking;

            if (User.SuperFastWalking)
                User.SuperFastWalking = false;

            Session.SendWhisper("Caminar rapido Act.");
        }
    }
}
