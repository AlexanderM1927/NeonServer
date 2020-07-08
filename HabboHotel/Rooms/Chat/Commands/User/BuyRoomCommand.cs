using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class BuyRoomCommand : IChatCommand
    {
        public string Description => "Comprar una sala a venta del usuario.";
        public string Parameters => "";
        public string PermissionRequired => "";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            RoomUser Owner = Room.GetRoomUserManager().GetRoomUserByHabbo(Room.RoomData.OwnerId);

            if (User == null)
            {
                return;
            }

            if (!Room.RoomForSale)
            {
                Session.SendWhisper("Esta sala no está a la venta, póngase en contacto con el propietario si está interesado:" + Room.OwnerName);
                return;
            }

            if (Room.OwnerId == User.HabboId)
            {
                Session.SendWhisper("No puedes comprar tu propia sala.");
                return;
            }

            if (User.GetClient().GetHabbo().Duckets >= Room.ForSaleAmount)
            {
                Room.AssignNewOwner(Room, User, Owner);
            }
            else
            {
                User.GetClient().SendWhisper("Usted no tienes duckets suficientes!");
                return;
            }

        }
    }
}
