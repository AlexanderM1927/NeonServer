using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.Rooms;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator.Fun
{
    class SuperFastwalkCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_super_fastwalk"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Obten la habilidad de caminar mas y mas rapido"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            User.SuperFastWalking = !User.SuperFastWalking;

            if (User.FastWalking)
                User.FastWalking = false;

            Session.SendWhisper("Walking mode updated.");
        }
    }
}
