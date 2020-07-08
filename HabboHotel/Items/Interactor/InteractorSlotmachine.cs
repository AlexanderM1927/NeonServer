using Neon.Communication.Packets.Outgoing.Inventory.Purse;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Rooms;
using Neon.Utilities;
using System.Threading;

namespace Neon.HabboHotel.Items.Interactor
{
    internal class InteractorSlotmachine : IFurniInteractor
    {
        private string Rand1;
        private string Rand2;
        private string Rand3;

        public void OnPlace(GameClient Session, Item Item)
        {
            Item.ExtraData = "0";
        }

        public void OnRemove(GameClient Session, Item Item)
        {
        }

        public void OnTrigger(GameClient Session, Item Item, int Request, bool HasRights)
        {
            // Revisar la cantidad de diamantes que tiene.
            if (Session.GetHabbo().Diamonds <= 0)
            {
                Session.SendWhisper("Para poder apostar debes tener diamantes, ahora mismo tienes 0.", 34);
                return;
            }

            if (Session.GetHabbo()._bet > Session.GetHabbo().Diamonds)
            {
                Session.SendWhisper("Estás apostando más diamantes de los que tienes.", 34);
                return;
            }

            if (Session.GetHabbo()._bet <= 0)
            {
                Session.SendWhisper("No puedes apostar 0 diamantes.", 34);
                return;
            }

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

            if (Gamemap.TileDistance(Actor.X, Actor.Y, Item.GetX, Item.GetY) < 2)
            {
                int Bet = Session.GetHabbo()._bet;

                Session.GetHabbo().Diamonds -= Bet;
                Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, -Bet, 5));

                int Random1 = RandomNumber.GenerateRandom(1, 3);
                switch (Random1)
                {
                    case 1:
                        Rand1 = "¥";
                        break;
                    case 2:
                        Rand1 = "|";
                        break;
                    case 3:
                        Rand1 = "ª";
                        break;
                }

                int Random2 = RandomNumber.GenerateRandom(1, 3);
                switch (Random2)
                {
                    case 1:
                        Rand2 = "¥";
                        break;
                    case 2:
                        Rand2 = "|";
                        break;
                    case 3:
                        Rand2 = "ª";
                        break;
                }

                int Random3 = RandomNumber.GenerateRandom(1, 3);
                switch (Random3)
                {
                    case 1:
                        Rand3 = "¥";
                        break;
                    case 2:
                        Rand3 = "|";
                        break;
                    case 3:
                        Rand3 = "ª";
                        break;
                }

                Session.SendWhisper("[ " + Rand1 + " - " + Rand2 + " - " + Rand3 + " ]", 34);
                Item.ExtraData = "1";
                Item.UpdateState(true, true);

                new Thread(() =>
                {
                    Thread.Sleep(1000);
                    Item.ExtraData = "0";
                    Item.UpdateState(true, true);
                }).Start();

                if (Random1 == Random2 && Random1 == Random3 && Random3 == Random2)
                {
                    //  ¥ - Estrella - » Trebol - ª Calavera
                    switch (Random1)
                    {
                        case 1:
                            Session.GetHabbo().Diamonds += Bet * 4;
                            Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, -Bet, 5));
                            Session.SendWhisper("Has ganado " + Bet * 4 + " diamantes con una triple estrella.", 34);
                            NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Actor.GetClient(), "ACH_StarBet", 1);
                            break;
                        case 2:
                            Session.GetHabbo().Diamonds += Bet * 5;
                            Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, -Bet, 5));
                            Session.SendWhisper("Has ganado " + Bet * 5 + " diamantes con un triple corazón.", 34);
                            NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Actor.GetClient(), "ACH_HeartBet", 1);
                            break;
                        case 3:
                            Session.GetHabbo().Diamonds += Bet * 3;
                            Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, -Bet, 5));
                            Session.SendWhisper("Has ganado " + Bet * 4 + " diamantes con una triple calavera.", 34);
                            NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Actor.GetClient(), "ACH_SkullBet", 1);
                            break;
                    }
                }

                NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Actor.GetClient(), "ACH_GeneralBet", 1);
                return;
            }
        }

        public void OnWiredTrigger(Item Item)
        {

        }
    }
}
