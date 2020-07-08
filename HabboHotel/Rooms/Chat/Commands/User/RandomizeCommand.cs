using Neon.HabboHotel.GameClients;
using System;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class RandomizeCommand : IChatCommand
    {
        public string PermissionRequired => "command_info";

        public string Parameters => "%min% %max%";

        public string Description => "Genera una cifra aleatoria entre 2 números.";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            int.TryParse(Params[1], out int Rand1);
            int.TryParse(Params[2], out int Rand2);

            Random Rand = new Random();

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            User.OnChat(8, "He pedido un número aleatorio entre el " + Rand1 + " y el " + Rand2 + " y he obtenido " + Rand.Next(Rand1, Rand2) + ".", false);

        }
    }
}
