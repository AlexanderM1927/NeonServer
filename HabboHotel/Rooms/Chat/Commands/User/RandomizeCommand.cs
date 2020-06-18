using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    class RandomizeCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_info"; }
        }

        public string Parameters
        {
            get { return "%min% %max%"; }
        }

        public string Description
        {
            get { return "Genera una cifra aleatoria entre 2 números."; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            int Rand1;
            int Rand2;
            int.TryParse(Params[1], out Rand1);
            int.TryParse(Params[2], out Rand2);

            Random Rand = new Random();

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            User.OnChat(8, "He pedido un número aleatorio entre el " + Rand1 + " y el " + Rand2 + " y he obtenido " + Rand.Next(Rand1, Rand2) + ".", false);

        }
    }
}
