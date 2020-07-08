using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Rooms;
using System;

namespace Neon.HabboHotel.Items.Interactor
{
    public class InteractorDice : IFurniInteractor
    {
        public void OnPlace(GameClient Session, Item Item)
        {
            if (Item.ExtraData == "-1")
            {
                Item.ExtraData = "0";
                Item.UpdateNeeded = true;
            }
        }

        public void OnRemove(GameClient Session, Item Item)
        {
            if (Item.ExtraData == "-1")
            {
                Item.ExtraData = "0";
            }
        }

        public void OnTrigger(GameClient Session, Item Item, int Request, bool HasRights)
        {
            RoomUser User = null;
            if (Session != null)
            {
                User = Item.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            }

            if (User == null)
            {
                return;
            }

            if (Gamemap.TilesTouching(Item.GetX, Item.GetY, User.X, User.Y))
            {
                if (Item.ExtraData != "-1")
                {
                    if (Request == -1)
                    {
                        Item.ExtraData = "0";
                        Item.UpdateState();
                    }
                    else
                    {
                        Item.ExtraData = "-1";
                        Item.UpdateState(false, true);
                        if (Session.GetHabbo().DiceNumber > 0)
                        {
                            Item.ExtraData = Convert.ToString(Session.GetHabbo().DiceNumber);
                        }

                        Item.RequestUpdate(3, true);
                        Session.GetHabbo().DiceNumber = 0;
                        Session.GetHabbo().RigDice = false;
                    }
                }
            }
            else
            {
                User.MoveTo(Item.SquareInFront);
            }
        }

        public void OnWiredTrigger(Item Item)
        {
            Item.ExtraData = "-1";
            Item.UpdateState(false, true);
            Item.RequestUpdate(4, true);
        }
    }
}