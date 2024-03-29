﻿using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Items.Wired;


namespace Neon.HabboHotel.Items.Interactor
{
    public class InteractorGate : IFurniInteractor
    {
        public void OnPlace(GameClient Session, Item Item)
        {
        }

        public void OnRemove(GameClient Session, Item Item)
        {
        }

        public void OnTrigger(GameClient Session, Item Item, int Request, bool HasRights)
        {
            int Modes = Item.GetBaseItem().Modes - 1;

            if (!HasRights)
            {
                return;
            }
            else if (Modes <= 0)
            {
                Item.UpdateState(false, true);
            }

            if (!int.TryParse(Item.ExtraData, out int CurrentMode))
            {
            }

            int NewMode;
            if (CurrentMode <= 0)
            {
                NewMode = 1;
            }
            else if (CurrentMode >= Modes)
            {
                NewMode = 0;
            }
            else
            {
                NewMode = CurrentMode + 1;
            }

            if (NewMode == 0)
            {
                if (!Item.GetRoom().GetGameMap().ItemCanBePlacedHere(Item.GetX, Item.GetY))
                {
                    return;
                }
            }

            if (Item.GetRoom() == null || Item.GetRoom().GetGameMap() == null ||
               Item.GetRoom().GetGameMap().SquareHasUsers(Item.GetX, Item.GetY))
            {
                return;
            }

            Item.ExtraData = NewMode.ToString();
            Item.UpdateState();

            Item.RegenerateBlock(NewMode.ToString(), Item.GetRoom().GetGameMap());

            Item.GetRoom().GetGameMap().UpdateMapForItem(Item);
            Item.GetRoom().GetWired().TriggerEvent(WiredBoxType.TriggerStateChanges, Session.GetHabbo(), Item);
            //Item.GetRoom().GenerateMaps();
        }

        public void OnWiredTrigger(Item Item)
        {
            int Modes = Item.GetBaseItem().Modes - 1;

            if (Modes <= 0)
            {
                Item.UpdateState(false, true);
            }

            if (!int.TryParse(Item.ExtraData, out int CurrentMode))
            {
            }


            int NewMode;
            if (CurrentMode <= 0)
            {
                NewMode = 1;
            }
            else if (CurrentMode >= Modes)
            {
                NewMode = 0;
            }
            else
            {
                NewMode = CurrentMode + 1;
            }

            if (NewMode == 0)
            {
                if (!Item.GetRoom().GetGameMap().ItemCanBePlacedHere(Item.GetX, Item.GetY))
                {
                    return;
                }
            }

            if (Item.GetRoom() == null || Item.GetRoom().GetGameMap() == null ||
               Item.GetRoom().GetGameMap().SquareHasUsers(Item.GetX, Item.GetY))
            {
                return;
            }

            Item.ExtraData = NewMode.ToString();
            Item.UpdateState();

            Item.GetRoom().GetGameMap().UpdateMapForItem(Item);
            //Item.GetRoom().GenerateMaps();
        }
    }
}