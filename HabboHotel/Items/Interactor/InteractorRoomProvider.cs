﻿using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Items.Interactor
{
    public class InteractorRoomProvider : IFurniInteractor
    {
        public void OnPlace(GameClient Session, Item Item)
        {
        }

        public void OnRemove(GameClient Session, Item Item)
        {
        }

        public void OnTrigger(GameClient Session, Item Item, int Request, bool HasRights)
        {
            Session.SendMessage(new GetGuestRoomResultMessageComposer(int.Parse(Item.ExtraData)));
        }

        public void OnWiredTrigger(Item Item)
        {
        }
    }
}