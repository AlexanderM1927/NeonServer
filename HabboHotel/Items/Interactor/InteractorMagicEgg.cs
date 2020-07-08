using Neon.HabboHotel.Rooms;

namespace Neon.HabboHotel.Items.Interactor
{
    internal class InteractorMagicEgg : IFurniInteractor
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

            int tick = int.Parse(Item.ExtraData);

            if (tick < 23)
            {
                if (Actor.CurrentEffect == 186)
                {
                    if (Gamemap.TileDistance(Actor.X, Actor.Y, Item.GetX, Item.GetY) < 2)
                    {
                        tick++;
                        Item.ExtraData = tick.ToString();
                        Item.UpdateState(true, true);
                        int X = Item.GetX, Y = Item.GetY, Rot = Item.Rotation;
                        double Z = Item.GetZ;
                        NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Actor.GetClient(), "ACH_EggCracker", 1);
                        if (tick == 19)
                        {
                            NeonEnvironment.GetGame().GetPinataManager().ReceiveCrackableReward(Actor, Room, Item);
                            NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Actor.GetClient(), "ACH_EggMaster", 1);
                        }
                    }
                }
                else
                {
                    Session.SendWhisper("¡Vaya, no tienes la varita! Escriba el comando [:enable 186]");
                    return;
                }
            }
        }

        public void OnWiredTrigger(Item Item)
        {

        }
    }
}
