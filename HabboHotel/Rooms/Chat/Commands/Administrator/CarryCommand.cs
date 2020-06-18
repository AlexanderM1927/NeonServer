using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Administrator
{
    class CarryCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_carry"; }
        }

        public string Parameters
        {
            get { return "%ItemId%"; }
        }

        public string Description
        {
            get { return "Le permite llevar un item en su mano"; }
        }

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {
            int ItemId = 0;
            if (!int.TryParse(Convert.ToString(Params[1]), out ItemId))
            {
                Session.SendWhisper("Por favor introduzca un item valido", 34);
                return;
            }

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            User.CarryItem(ItemId);
        }
    }
}
