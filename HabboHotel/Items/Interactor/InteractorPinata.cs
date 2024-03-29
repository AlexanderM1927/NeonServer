﻿using Neon.HabboHotel.Rooms;

namespace Neon.HabboHotel.Items.Interactor
{
    internal class InteractorPinata : IFurniInteractor
    {
        public void OnPlace(GameClients.GameClient Session, Item Item)
        {
            Item.ExtraData = "0";
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

            NeonEnvironment.GetGame().GetPinataManager().ReceiveCrackableReward(Actor, Room, Item);

        }

        public void OnWiredTrigger(Item Item)
        {

        }
    }
}
