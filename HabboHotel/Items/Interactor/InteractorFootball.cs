using System;
using System.Drawing;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Items.Interactor
{
    public class InteractorFootball : IFurniInteractor
    {
        public void OnPlace(GameClient Session, Item Item)
        {
        }

        public void OnRemove(GameClient Session, Item Item)
        {
        }

        public void OnTrigger(GameClient Session, Item Item, int Request, bool HasRights)
        {
            if (Session == null || Session.GetHabbo() == null || Item == null || Item.GetRoom() == null)
                return;

            RoomUser User = Item.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);

            if (User == null || Item.GetRoom().Shoot == 0)
                return;

            if (!((Math.Abs((User.X - Item.GetX)) >= 2) || (Math.Abs((User.Y - Item.GetY)) >= 2)))
            {
                Point NewPoint = new Point(User.X, User.Y);
                Item.ExtraData = "55";
                Item.BallIsMoving = true;
                Item.BallValue = 1;

            }
        }

        public void OnWiredTrigger(Item Item)
        {
        }
    }
}
