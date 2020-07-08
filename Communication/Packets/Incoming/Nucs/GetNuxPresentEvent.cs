using Neon.Communication.Packets.Outgoing.Inventory.Furni;
using Neon.Communication.Packets.Outgoing.Inventory.Purse;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Items;
using Neon.Utilities;

namespace Neon.Communication.Packets.Incoming.Nucs
{
    internal class GetNuxPresentEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int Data1 = Packet.PopInt(); // ELEMENTO 1
            int Data2 = Packet.PopInt(); // ELEMENTO 2
            int Data3 = Packet.PopInt(); // ELEMENTO 3
            int Data4 = Packet.PopInt(); // SELECTOR
            string RewardName = "";
            Outgoing.Nux.NuxUserGiftsList NuxGift = NeonEnvironment.GetGame().GetNuxUserGiftsListManager().NuxUserGiftsList;

            switch (Data4)
            {
                case 0:
                    switch (NuxGift.Type[0])
                    {
                        case "diamonds":
                            string[] Posibility = NuxGift.Reward[0].Split(',');

                            int Posibility1 = int.Parse(Posibility[0]);
                            int Posibility2 = int.Parse(Posibility[1]);

                            int RewardDiamonds = RandomNumber.GenerateRandom(Posibility1, Posibility2);
                            Session.GetHabbo().Diamonds += RewardDiamonds;
                            Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, RewardDiamonds, 5));
                            break;

                        case "honor":
                            string[] Posibilitya = NuxGift.Reward[0].Split(',');

                            int Posibility1a = int.Parse(Posibilitya[0]);
                            int Posibility2a = int.Parse(Posibilitya[1]);

                            int RewardHonor = RandomNumber.GenerateRandom(Posibility1a, Posibility2a);
                            Session.GetHabbo().GOTWPoints += RewardHonor;
                            Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().GOTWPoints, RewardHonor, 103));
                            break;

                        case "item":
                            int RewardItem = RandomNumber.GenerateRandom(1, 10);
                            string[] Furnis = NuxGift.Reward[0].Split(',');

                            string[] Present = Furnis[0].Split(':');
                            string[] Present1 = Furnis[1].Split(':');
                            string[] Present2 = Furnis[2].Split(':');
                            string[] Present3 = Furnis[3].Split(':');
                            string[] Present4 = Furnis[4].Split(':');
                            string[] Present5 = Furnis[5].Split(':');
                            string[] Present6 = Furnis[6].Split(':');
                            string[] Present7 = Furnis[7].Split(':');
                            string[] Present8 = Furnis[8].Split(':');
                            string[] Present9 = Furnis[9].Split(':');

                            int RewardItemId = 0;

                            switch (RewardItem)
                            {
                                case 1:
                                    RewardItemId = int.Parse(Present[0]); // VIP - club_sofa
                                    RewardName = Present[1];
                                    break;
                                case 2:
                                    RewardItemId = int.Parse(Present1[0]); // VIP - club_sofa
                                    RewardName = Present1[1];
                                    break;
                                case 3:
                                    RewardItemId = int.Parse(Present2[0]); // VIP - club_sofa
                                    RewardName = Present2[1];
                                    break;
                                case 4:
                                    RewardItemId = int.Parse(Present3[0]); // VIP - club_sofa
                                    RewardName = Present3[1];
                                    break;
                                case 5:
                                    RewardItemId = int.Parse(Present4[0]); // VIP - club_sofa
                                    RewardName = Present4[1];
                                    break;
                                case 6:
                                    RewardItemId = int.Parse(Present5[0]); // VIP - club_sofa
                                    RewardName = Present5[1];
                                    break;
                                case 7:
                                    RewardItemId = int.Parse(Present6[0]); // VIP - club_sofa
                                    RewardName = Present6[1];
                                    break;
                                case 8:
                                    RewardItemId = int.Parse(Present7[0]); // VIP - club_sofa
                                    RewardName = Present7[1];
                                    break;
                                case 9:
                                    RewardItemId = int.Parse(Present8[0]); // VIP - club_sofa
                                    RewardName = Present8[1];
                                    break;
                                case 10:
                                    RewardItemId = int.Parse(Present9[0]); // VIP - club_sofa
                                    RewardName = Present9[1];
                                    break;
                            }

                            ItemData Item = null;
                            if (!NeonEnvironment.GetGame().GetItemManager().GetItem(RewardItemId, out Item))
                            {
                                return;
                            }

                            Item GiveItem = ItemFactory.CreateSingleItemNullable(Item, Session.GetHabbo(), "", "");
                            if (GiveItem != null)
                            {
                                Session.GetHabbo().GetInventoryComponent().TryAddItem(GiveItem);
                                Session.SendMessage(RoomNotificationComposer.SendBubble("image2", "Acabas de recibir un " + RewardName + ".\n\n¡Corre, " + Session.GetHabbo().Username + ", revisa tu inventario, hay algo nuevo al parecer!", "inventory/open/furni"));
                                Session.SendMessage(new FurniListNotificationComposer(GiveItem.Id, 1));
                                Session.SendMessage(new FurniListUpdateComposer());
                            }

                            Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
                            break;
                    }
                    break;

                case 1:
                    switch (NuxGift.Type[1])
                    {
                        case "diamonds":
                            string[] Posibility = NuxGift.Reward[1].Split(',');

                            int Posibility1 = int.Parse(Posibility[0]);
                            int Posibility2 = int.Parse(Posibility[1]);

                            int RewardDiamonds = RandomNumber.GenerateRandom(Posibility1, Posibility2);
                            Session.GetHabbo().Diamonds += RewardDiamonds;
                            Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, RewardDiamonds, 5));
                            break;

                        case "honor":
                            string[] Posibilitya = NuxGift.Reward[1].Split(',');

                            int Posibility1a = int.Parse(Posibilitya[0]);
                            int Posibility2a = int.Parse(Posibilitya[1]);

                            int RewardHonor = RandomNumber.GenerateRandom(Posibility1a, Posibility2a);
                            Session.GetHabbo().GOTWPoints += RewardHonor;
                            Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().GOTWPoints, RewardHonor, 103));
                            break;

                        case "item":
                            int RewardItem = RandomNumber.GenerateRandom(1, 10);
                            string[] Furnis = NuxGift.Reward[1].Split(',');

                            string[] Present = Furnis[0].Split(':');
                            string[] Present1 = Furnis[1].Split(':');
                            string[] Present2 = Furnis[2].Split(':');
                            string[] Present3 = Furnis[3].Split(':');
                            string[] Present4 = Furnis[4].Split(':');
                            string[] Present5 = Furnis[5].Split(':');
                            string[] Present6 = Furnis[6].Split(':');
                            string[] Present7 = Furnis[7].Split(':');
                            string[] Present8 = Furnis[8].Split(':');
                            string[] Present9 = Furnis[9].Split(':');

                            int RewardItemId = 0;

                            switch (RewardItem)
                            {
                                case 1:
                                    RewardItemId = int.Parse(Present[0]); // VIP - club_sofa
                                    RewardName = Present[1];
                                    break;
                                case 2:
                                    RewardItemId = int.Parse(Present1[0]); // VIP - club_sofa
                                    RewardName = Present1[1];
                                    break;
                                case 3:
                                    RewardItemId = int.Parse(Present2[0]); // VIP - club_sofa
                                    RewardName = Present2[1];
                                    break;
                                case 4:
                                    RewardItemId = int.Parse(Present3[0]); // VIP - club_sofa
                                    RewardName = Present3[1];
                                    break;
                                case 5:
                                    RewardItemId = int.Parse(Present4[0]); // VIP - club_sofa
                                    RewardName = Present4[1];
                                    break;
                                case 6:
                                    RewardItemId = int.Parse(Present5[0]); // VIP - club_sofa
                                    RewardName = Present5[1];
                                    break;
                                case 7:
                                    RewardItemId = int.Parse(Present6[0]); // VIP - club_sofa
                                    RewardName = Present6[1];
                                    break;
                                case 8:
                                    RewardItemId = int.Parse(Present7[0]); // VIP - club_sofa
                                    RewardName = Present7[1];
                                    break;
                                case 9:
                                    RewardItemId = int.Parse(Present8[0]); // VIP - club_sofa
                                    RewardName = Present8[1];
                                    break;
                                case 10:
                                    RewardItemId = int.Parse(Present9[0]); // VIP - club_sofa
                                    RewardName = Present9[1];
                                    break;
                            }

                            ItemData Item = null;
                            if (!NeonEnvironment.GetGame().GetItemManager().GetItem(RewardItemId, out Item))
                            {
                                return;
                            }

                            Item GiveItem = ItemFactory.CreateSingleItemNullable(Item, Session.GetHabbo(), "", "");
                            if (GiveItem != null)
                            {
                                Session.GetHabbo().GetInventoryComponent().TryAddItem(GiveItem);
                                Session.SendMessage(RoomNotificationComposer.SendBubble("image2", "Acabas de recibir un " + RewardName + ".\n\n¡Corre, " + Session.GetHabbo().Username + ", revisa tu inventario, hay algo nuevo al parecer!", "inventory/open/furni"));
                                Session.SendMessage(new FurniListNotificationComposer(GiveItem.Id, 1));
                                Session.SendMessage(new FurniListUpdateComposer());
                            }

                            Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
                            break;
                    }
                    break;
                case 2:
                    switch (NuxGift.Type[2])
                    {
                        case "diamonds":
                            string[] Posibility = NuxGift.Reward[2].Split(',');

                            int Posibility1 = int.Parse(Posibility[0]);
                            int Posibility2 = int.Parse(Posibility[1]);

                            int RewardDiamonds = RandomNumber.GenerateRandom(Posibility1, Posibility2);
                            Session.GetHabbo().Diamonds += RewardDiamonds;
                            Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, RewardDiamonds, 5));
                            break;

                        case "honor":
                            string[] Posibilitya = NuxGift.Reward[2].Split(',');

                            int Posibility1a = int.Parse(Posibilitya[0]);
                            int Posibility2a = int.Parse(Posibilitya[1]);

                            int RewardHonor = RandomNumber.GenerateRandom(Posibility1a, Posibility2a);
                            Session.GetHabbo().GOTWPoints += RewardHonor;
                            Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().GOTWPoints, RewardHonor, 103));
                            break;

                        case "item":
                            int RewardItem = RandomNumber.GenerateRandom(1, 10);
                            string[] Furnis = NuxGift.Reward[2].Split(',');

                            string[] Present = Furnis[0].Split(':');
                            string[] Present1 = Furnis[1].Split(':');
                            string[] Present2 = Furnis[2].Split(':');
                            string[] Present3 = Furnis[3].Split(':');
                            string[] Present4 = Furnis[4].Split(':');
                            string[] Present5 = Furnis[5].Split(':');
                            string[] Present6 = Furnis[6].Split(':');
                            string[] Present7 = Furnis[7].Split(':');
                            string[] Present8 = Furnis[8].Split(':');
                            string[] Present9 = Furnis[9].Split(':');

                            int RewardItemId = 0;

                            switch (RewardItem)
                            {
                                case 1:
                                    RewardItemId = int.Parse(Present[0]); // VIP - club_sofa
                                    RewardName = Present[1];
                                    break;
                                case 2:
                                    RewardItemId = int.Parse(Present1[0]); // VIP - club_sofa
                                    RewardName = Present1[1];
                                    break;
                                case 3:
                                    RewardItemId = int.Parse(Present2[0]); // VIP - club_sofa
                                    RewardName = Present2[1];
                                    break;
                                case 4:
                                    RewardItemId = int.Parse(Present3[0]); // VIP - club_sofa
                                    RewardName = Present3[1];
                                    break;
                                case 5:
                                    RewardItemId = int.Parse(Present4[0]); // VIP - club_sofa
                                    RewardName = Present4[1];
                                    break;
                                case 6:
                                    RewardItemId = int.Parse(Present5[0]); // VIP - club_sofa
                                    RewardName = Present5[1];
                                    break;
                                case 7:
                                    RewardItemId = int.Parse(Present6[0]); // VIP - club_sofa
                                    RewardName = Present6[1];
                                    break;
                                case 8:
                                    RewardItemId = int.Parse(Present7[0]); // VIP - club_sofa
                                    RewardName = Present7[1];
                                    break;
                                case 9:
                                    RewardItemId = int.Parse(Present8[0]); // VIP - club_sofa
                                    RewardName = Present8[1];
                                    break;
                                case 10:
                                    RewardItemId = int.Parse(Present9[0]); // VIP - club_sofa
                                    RewardName = Present9[1];
                                    break;
                            }

                            ItemData Item = null;
                            if (!NeonEnvironment.GetGame().GetItemManager().GetItem(RewardItemId, out Item))
                            {
                                return;
                            }

                            Item GiveItem = ItemFactory.CreateSingleItemNullable(Item, Session.GetHabbo(), "", "");
                            if (GiveItem != null)
                            {
                                Session.GetHabbo().GetInventoryComponent().TryAddItem(GiveItem);
                                Session.SendMessage(RoomNotificationComposer.SendBubble("image2", "Acabas de recibir un " + RewardName + ".\n\n¡Corre, " + Session.GetHabbo().Username + ", revisa tu inventario, hay algo nuevo al parecer!", "inventory/open/furni"));
                                Session.SendMessage(new FurniListNotificationComposer(GiveItem.Id, 1));
                                Session.SendMessage(new FurniListUpdateComposer());
                            }

                            Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
                            break;
                    }
                    break;
            }
        }
    }
}