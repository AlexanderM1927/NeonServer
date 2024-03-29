﻿using Neon.HabboHotel.Rooms;

namespace Neon.HabboHotel.Items.Interactor
{
    internal class InteractorCannon : IFurniInteractor
    {
        public void OnPlace(GameClients.GameClient Session, Item Item)
        {
        }

        public void OnRemove(GameClients.GameClient Session, Item Item)
        {
        }

        public void OnTrigger(GameClients.GameClient Session, Item Item, int Request, bool HasRights)
        {
            if (Session == null || Session.GetHabbo() == null || Item == null)
            {
                return;
            }

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
            {
                return;
            }

            RoomUser Actor = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (Actor == null)
            {
                return;
            }

            if (Item.ExtraData == "1")
            {
                return;
            }

            if (Gamemap.TileDistance(Actor.X, Actor.Y, Item.GetX, Item.GetY) > 2)
            {
                return;
            }

            Item.ExtraData = "1";
            Item.UpdateState(false, true);

            Item.RequestUpdate(2, true);
        }

        public void OnWiredTrigger(Item Item)
        {
            if (Item == null)
            {
                return;
            }

            if (Item.ExtraData == "1")
            {
                return;
            }

            Item.ExtraData = "1";
            Item.UpdateState(false, true);

            Item.RequestUpdate(2, true);
        }
    }
}
