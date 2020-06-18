using Neon.HabboHotel.Rooms.Games.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User.Fun
{
    class CurarCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_stats"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Curar el una hinchazón por algún golpe recibido."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            RoomUser ThisUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Username);
            if (ThisUser == null)
                return;

            if (ThisUser.RidingHorse)
            {
                Session.SendWhisper("No se puede curar de un golpe mientras montas un caballo.");
                return;
            }
            else if (ThisUser.Team != TEAM.NONE)
                return;
            else if (ThisUser.isLying)
                return;

            Session.GetHabbo().Effects().ApplyEffect(0);
            Session.SendWhisper("Te has curado correctamente de un golpe en la cara.");
        }
    }
}
