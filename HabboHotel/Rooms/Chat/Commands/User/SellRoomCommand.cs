using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class SellRoomCommand : IChatCommand
    {
        public string Description => "Ponga la sala en que estás a venta.";
        public string Parameters => "%precio%";
        public string PermissionRequired => "";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Ponga un precio xD", 34);
                return;
            }

            if (!Room.CheckRights(Session, true))
            {
                return;
            }

            if (Room == null)
            {
                if (Params.Length == 1)
                {
                    Session.SendWhisper("Vaya, se te olvidó elegir un precio para vender esta sala..", 34);
                    return;
                }
                else if (Room.Group != null)
                {
                    Session.SendWhisper("Vaya, aparentemente esta sala tiene un grupo, por lo que no puedes vender, primero debes eliminar el grupo.", 34);
                    return;
                }
            }

            if (!int.TryParse(Params[1], out int Value))
            {
                Session.SendWhisper("Vaya, está ingresando un valor que no es correcto", 34);
                return;
            }

            if (Value < 0)
            {
                Session.SendWhisper("No puede vender una sala con un valor numérico negativo..", 34);
                return;
            }

            if (Room.ForSale)
            {
                Room.SalePrice = Value;
            }
            else
            {
                Room.ForSale = true;
                Room.SalePrice = Value;
            }

            foreach (RoomUser User in Room.GetRoomUserManager().GetRoomUsers())
            {
                if (User == null || User.GetClient() == null)
                {
                    continue;
                }

                Session.SendWhisper("Esta sala está a la venta, el precio actual es:  " + Value + " Duckets! Cómprelo escribiendo :buyroom.");
            }

            Session.SendNotification("Si desea vender su sala, debe incluir un valor numérico. \n\nTENGA EN CUENTA:\nSi vende una habitación, ¡no puede recuperarla!\n\n" +
        "Puede cancelar la venta de una habitación escribiendo ':unload' (sin as '')");

            return;
        }

    }
}