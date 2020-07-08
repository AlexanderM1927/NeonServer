using System;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Administrator
{
    internal class CarryCommand : IChatCommand
    {
        public string PermissionRequired => "command_carry";

        public string Parameters => "%ItemId%";

        public string Description => "Le permite llevar un item en su mano";

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {
            if (!int.TryParse(Convert.ToString(Params[1]), out int ItemId))
            {
                Session.SendWhisper("Por favor introduzca un item valido", 34);
                return;
            }

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
            {
                return;
            }

            User.CarryItem(ItemId);
        }
    }
}
