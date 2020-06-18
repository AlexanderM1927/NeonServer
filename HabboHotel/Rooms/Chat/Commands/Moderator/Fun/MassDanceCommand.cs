using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.Rooms;
using Neon.Communication.Packets.Outgoing.Rooms.Avatar;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator.Fun
{
    class MassDanceCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_massdance"; }
        }

        public string Parameters
        {
            get { return "%DanceId%"; }
        }

        public string Description
        {
            get { return "Obliga a todos a Bailar"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor introduce un numero de baile. (1-4)");
                return;
            }

            int DanceId = Convert.ToInt32(Params[1]);
            if (DanceId < 0 || DanceId > 4)
            {
                Session.SendWhisper("Por favor introduce un numero de baile. (1-4)");
                return;
            }

            List<RoomUser> Users = Room.GetRoomUserManager().GetRoomUsers();
            if (Users.Count > 0)
            {
                foreach (RoomUser U in Users.ToList())
                {
                    if (U == null)
                        continue;

                    if (U.CarryItemID > 0)
                        U.CarryItemID = 0;

                    U.DanceId = DanceId;
                    Room.SendMessage(new DanceComposer(U, DanceId));
                }
            }
        }
    }
}
