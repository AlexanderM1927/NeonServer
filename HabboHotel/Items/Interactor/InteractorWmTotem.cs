using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.Database.Interfaces;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Rooms;
using System;

namespace Neon.HabboHotel.Items.Interactor
{
    public class InteractorWmTotem : IFurniInteractor
    {
        public void OnPlace(GameClient Session, Item Item)
        {
            Item.ExtraData = "0";
            Item.UpdateNeeded = true;
        }

        public void OnRemove(GameClient Session, Item Item)
        {
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

            if (Item.GetBaseItem().InteractionType == InteractionType.TotemPlanet)
            {
                if (Session.GetHabbo().Rank > 0)
                {

                    if (Item.UserID != Session.GetHabbo().Id)
                    {
                        if (Item.GetZ < 2.5)
                        {
                            Session.SendWhisper("El planeta tótem solo se puede activar si está encima de un tótem.", 34);
                            return;
                        }
                        if (Item.UserID != Session.GetHabbo().Id)
                        {
                            Session.SendWhisper("Este tótem no te pertenece.", 34);
                            return;
                        }
                        if (Session.GetHabbo().Rank > 0)
                        {

                            if (Item.ExtraData == "0")
                            {
                                User.ApplyEffect(24);
                                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                                {
                                    dbClient.SetQuery("INSERT INTO `user_effects` (`user_id`,`effect_id`) VALUES ('" + Session.GetHabbo().Id + "', '24')");
                                    dbClient.RunQuery();
                                }
                                Session.SendWhisper("El efecto de lluvia se agregó con éxito, tan pronto como vuelva a iniciar sesión se guardará en su inventario.", 34);
                                return;
                            }
                            if (Item.ExtraData == "1")
                            {
                                User.ApplyEffect(25);
                                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                                {
                                    dbClient.SetQuery("INSERT INTO `user_effects` (`user_id`,`effect_id`) VALUES ('" + Session.GetHabbo().Id + "', '25')");
                                    dbClient.RunQuery();
                                }
                                Session.SendWhisper("El efecto de fuego se agregó con éxito, tan pronto como vuelva a iniciar sesión se guardará en su inventario.", 34);

                                return;
                            }
                            if (Item.ExtraData == "2")
                            {
                                User.ApplyEffect(26);
                                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                                {
                                    dbClient.SetQuery("INSERT INTO `user_effects` (`user_id`,`effect_id`) VALUES ('" + Session.GetHabbo().Id + "', '26')");
                                    dbClient.RunQuery();
                                }
                                Session.SendWhisper("El efecto de personal se agregó con éxito, tan pronto como vuelva a iniciar sesión se guardará en su inventario.", 34);

                                return;
                            }
                            if (Item.ExtraData == "3")
                            {
                                User.ApplyEffect(23);
                                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                                {
                                    dbClient.SetQuery("INSERT INTO `user_effects` (`user_id`,`effect_id`) VALUES ('" + Session.GetHabbo().Id + "', '23')");
                                    dbClient.RunQuery();
                                }
                                Session.SendWhisper("El efecto de levitación se agregó con éxito, tan pronto como lo registres se guardará en tu inventario.", 34);

                                return;
                            }
                        }
                    }
                    else
                    {
                        if (Item.GetZ < 2.5)
                        {
                            Session.SendWhisper("El planeta tótem solo se puede activar si está encima de un tótem.", 34);
                            return;
                        }
                        if (Item.UserID != Session.GetHabbo().Id)
                        {
                            Session.SendWhisper("Esta caja de la suerte no te pertenece.", 34);
                            return;
                        }
                        if (Item.ExtraData == "0")
                        {
                            User.ApplyEffect(24);
                            Item.ExtraData = "1";
                            Item.UpdateState(true, true);
                            Item.RequestUpdate(0, true);
                            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.SetQuery("INSERT INTO `user_effects` (`user_id`,`effect_id`) VALUES ('" + Session.GetHabbo().Id + "', '24')");
                                dbClient.RunQuery();
                            }
                            Session.SendWhisper("El efecto de lluvia se agregó con éxito, tan pronto como vuelva a iniciar sesión se guardará en su inventario.", 34);
                            return;
                        }
                        if (Item.ExtraData == "1")
                        {
                            User.ApplyEffect(25);
                            Item.ExtraData = "2";
                            Item.UpdateState(true, true);
                            Item.RequestUpdate(0, true);
                            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.SetQuery("INSERT INTO `user_effects` (`user_id`,`effect_id`) VALUES ('" + Session.GetHabbo().Id + "', '25')");
                                dbClient.RunQuery();
                            }
                            Session.SendWhisper("El efecto de fuego se agregó con éxito, tan pronto como vuelva a iniciar sesión se guardará en su inventario.", 34);

                            return;
                        }
                        if (Item.ExtraData == "2")
                        {
                            User.ApplyEffect(26);
                            Item.ExtraData = "3";
                            Item.UpdateState(true, true);
                            Item.RequestUpdate(0, true);
                            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.SetQuery("INSERT INTO `user_effects` (`user_id`,`effect_id`) VALUES ('" + Session.GetHabbo().Id + "', '26')");
                                dbClient.RunQuery();
                            }
                            Session.SendWhisper("El efecto de personal se agregó con éxito, tan pronto como vuelva a iniciar sesión se guardará en su inventario.", 34);

                            return;
                        }
                        if (Item.ExtraData == "3")
                        {
                            User.ApplyEffect(23);
                            Item.ExtraData = "0";
                            Item.UpdateState(true, true);
                            Item.RequestUpdate(0, true);
                            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.SetQuery("INSERT INTO `user_effects` (`user_id`,`effect_id`) VALUES ('" + Session.GetHabbo().Id + "', '23')");
                                dbClient.RunQuery();
                            }
                            Session.SendWhisper("El efecto de levitación se agregó con éxito, tan pronto como lo registres se guardará en tu inventario.", 34);

                            return;
                        }
                    }
                }
                else
                {
                    Session.SendMessage(new RoomNotificationComposer("furni_placement_error", "message", "Solo los miembros VIP tienen acceso al Totem Effect."));
                    return;
                }
            }

            if (Item.GetBaseItem().InteractionType == InteractionType.TotemPlanet)
            {
                if (Session.GetHabbo().Rank > 0)
                {

                    if (Item.UserID != Session.GetHabbo().Id)
                    {
                        if (Item.GetZ < 2.5)
                        {
                            Session.SendWhisper("El planeta tótem solo se puede activar si está encima de un tótem.", 34);
                            return;
                        }
                        if (Item.UserID != Session.GetHabbo().Id)
                        {
                            Session.SendWhisper("Este tótem no te pertenece.", 34);
                            return;
                        }
                        if (Item.ExtraData == "0")
                        {
                            User.ApplyEffect(548);
                            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.SetQuery("INSERT INTO `user_effects` (`user_id`,`effect_id`) VALUES ('" + Session.GetHabbo().Id + "', '548')");
                                dbClient.RunQuery();
                            }
                            Session.SendWhisper("El efecto de fuego se agregó con éxito, tan pronto como vuelva a iniciar sesión se guardará en su inventario.", 34);
                            return;
                        }
                        if (Item.ExtraData == "1")
                        {
                            User.ApplyEffect(531);
                            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.SetQuery("INSERT INTO `user_effects` (`user_id`,`effect_id`) VALUES ('" + Session.GetHabbo().Id + "', '531')");
                                dbClient.RunQuery();
                            }
                            Session.SendWhisper("El efecto de fuego se agregó con éxito, tan pronto como vuelva a iniciar sesión se guardará en su inventario.", 34);

                            return;
                        }
                        if (Item.ExtraData == "2")
                        {
                            User.ApplyEffect(26);
                            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.SetQuery("INSERT INTO `user_effects` (`user_id`,`effect_id`) VALUES ('" + Session.GetHabbo().Id + "', '26')");
                                dbClient.RunQuery();
                            }
                            Session.SendWhisper("El efecto de personal se agregó con éxito, tan pronto como vuelva a iniciar sesión se guardará en su inventario.", 34);

                            return;
                        }
                        if (Item.ExtraData == "3")
                        {
                            User.ApplyEffect(23);
                            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.SetQuery("INSERT INTO `user_effects` (`user_id`,`effect_id`) VALUES ('" + Session.GetHabbo().Id + "', '23')");
                                dbClient.RunQuery();
                            }
                            Session.SendWhisper("El efecto de levitación se agregó con éxito, tan pronto como lo registres se guardará en tu inventario.", 34);

                            return;
                        }
                    }
                    else
                    {
                        if (Item.GetZ < 2.5)
                        {
                            Session.SendWhisper("El planeta tótem solo se puede activar si está encima de un tótem.", 34);
                            return;
                        }
                        if (Item.UserID != Session.GetHabbo().Id)
                        {
                            Session.SendWhisper("Esta caja de la suerte no te pertenece.", 34);
                            return;
                        }
                        if (Item.ExtraData == "0")
                        {
                            User.ApplyEffect(548);
                            Item.ExtraData = "1";
                            Item.UpdateState(true, true);
                            Item.RequestUpdate(0, true);
                            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.SetQuery("INSERT INTO `user_effects` (`user_id`,`effect_id`) VALUES ('" + Session.GetHabbo().Id + "', '548')");
                                dbClient.RunQuery();
                            }
                            Session.SendWhisper("El efecto de fuego se agregó con éxito, tan pronto como vuelva a iniciar sesión se guardará en su inventario.", 34);
                            return;
                        }
                        if (Item.ExtraData == "1")
                        {
                            User.ApplyEffect(531);
                            Item.ExtraData = "2";
                            Item.UpdateState(true, true);
                            Item.RequestUpdate(0, true);
                            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.SetQuery("INSERT INTO `user_effects` (`user_id`,`effect_id`) VALUES ('" + Session.GetHabbo().Id + "', '531')");
                                dbClient.RunQuery();
                            }
                            Session.SendWhisper("El efecto de fuego se agregó con éxito, tan pronto como vuelva a iniciar sesión se guardará en su inventario.", 34);

                            return;
                        }
                        if (Item.ExtraData == "2")
                        {
                            User.ApplyEffect(26);
                            Item.ExtraData = "3";
                            Item.UpdateState(true, true);
                            Item.RequestUpdate(0, true);
                            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.SetQuery("INSERT INTO `user_effects` (`user_id`,`effect_id`) VALUES ('" + Session.GetHabbo().Id + "', '26')");
                                dbClient.RunQuery();
                            }
                            Session.SendWhisper("El efecto de personal se agregó con éxito, tan pronto como vuelva a iniciar sesión se guardará en su inventario.", 34);

                            return;
                        }
                        if (Item.ExtraData == "3")
                        {
                            User.ApplyEffect(23);
                            Item.ExtraData = "0";
                            Item.UpdateState(true, true);
                            Item.RequestUpdate(0, true);
                            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.SetQuery("INSERT INTO `user_effects` (`user_id`,`effect_id`) VALUES ('" + Session.GetHabbo().Id + "', '23')");
                                dbClient.RunQuery();
                            }
                            Session.SendWhisper("El efecto de levitación se agregó con éxito, tan pronto como lo registres se guardará en tu inventario.", 34);

                            return;
                        }
                    }
                }
                else
                {
                    Session.SendMessage(new RoomNotificationComposer("furni_placement_error", "message", "Solo los miembros VIP tienen acceso al Totem Effect."));
                    return;
                }
            }

            if (Item.BaseItem == 1100001342)
            {
                if (Session.GetHabbo().Rank > 0)
                {
                    if (Item.ExtraData == "0")
                    {
                        Session.SendMessage(new RoomNotificationComposer("habbopages/infoterminal/" + Item.UserID + ".txt"));
                        Session.SendWhisper("Se abrió la terminal de información de mobi, con la identificación del propietario: " + Item.UserID + "");
                        return;
                    }
                    if (Item.ExtraData == "1")
                    {
                        Session.SendMessage(new RoomNotificationComposer("habbopages/infoterminal/" + Item.UserID + ".txt"));
                        Session.SendWhisper("Se abrió la terminal de información de mobi, con la identificación del propietario: " + Item.UserID + "");
                        return;
                    }
                    if (Item.ExtraData == "2")
                    {
                        Session.SendMessage(new RoomNotificationComposer("habbopages/infoterminal/" + Item.UserID + ".txt"));
                        Session.SendWhisper("Se abrió la terminal de información de mobi, con la identificación del propietario: " + Item.UserID + "");
                        return;
                    }
                    if (Item.ExtraData == "3")
                    {
                        Session.SendMessage(new RoomNotificationComposer("habbopages/infoterminal/" + Item.UserID + ".txt"));
                        Session.SendWhisper("Se abrió la terminal de información de mobi, con la identificación del propietario: " + Item.UserID + "");
                        return;
                    }
                    else
                    {
                        if (Item.ExtraData == "0")
                        {
                            Item.ExtraData = "1";
                            Item.UpdateState(true, true);
                            Item.RequestUpdate(0, true);
                            Session.SendMessage(new RoomNotificationComposer("habbopages/infoterminal/" + Item.UserID + ".txt"));
                            Session.SendWhisper("Se abrió la terminal de información de mobi, con la identificación del propietario: " + Item.UserID + "");
                            return;
                        }
                        if (Item.ExtraData == "1")
                        {
                            Item.ExtraData = "2";
                            Item.UpdateState(true, true);
                            Item.RequestUpdate(0, true);
                            Session.SendMessage(new RoomNotificationComposer("habbopages/infoterminal/" + Item.UserID + ".txt"));
                            Session.SendWhisper("Se abrió la terminal de información de mobi, con la identificación del propietario: " + Item.UserID + "");
                            return;
                        }
                        if (Item.ExtraData == "2")
                        {
                            Item.ExtraData = "3";
                            Item.UpdateState(true, true);
                            Item.RequestUpdate(0, true);
                            Session.SendMessage(new RoomNotificationComposer("habbopages/infoterminal/" + Item.UserID + ".txt"));
                            Session.SendWhisper("Se abrió la terminal de información de mobi, con la identificación del propietario: " + Item.UserID + "");
                            return;
                        }
                        if (Item.ExtraData == "3")
                        {
                            Item.ExtraData = "0";
                            Item.UpdateState(true, true);
                            Item.RequestUpdate(0, true);
                            Session.SendMessage(new RoomNotificationComposer("habbopages/infoterminal/" + Item.UserID + ".txt"));
                            Session.SendWhisper("Se abrió la terminal de información de mobi, con la identificación del propietario: " + Item.UserID + "");
                            return;
                        }
                    }
                }
                else
                {
                    Session.SendWhisper("Solo los miembros VIP tienen acceso a la terminal de información de mobi.");
                    return;
                }
            }

            if (Item.GetBaseItem().InteractionType == InteractionType.LuckyBox)
            {
                Random random = new Random();
                int mobi1 = 1145;
                int mobi2 = 1145;
                int mobi3 = 1145;
                int mobi4 = 1145;
                int mobi5 = 1145;
                int mobi6 = 540019;
                int mobi7 = 540019;
                int mobi8 = 540019;
                int mobi9 = 72021;
                string mobi01 = "Barra de 500 de Oro";
                string imagem01 = "CFC_500_goldbar";
                string mobi02 = "Barra de 500 de Oro";
                string imagem02 = "CFC_500_goldbar";
                string mobi03 = "Barra de 500 de Oro";
                string imagem03 = "CFC_500_goldbar";
                string mobi04 = "Barra de 500 de Oro";
                string imagem04 = "CFC_500_goldbar";
                string mobi05 = "Barra de 500 de Oro";
                string imagem05 = "CFC_500_goldbar";
                string mobi06 = "Pato de Ouro 250c";
                string imagem06 = "CFC_500_goldbar";
                string mobi07 = "Pato de Ouro 250c";
                string imagem07 = "CFC_500_goldbar";
                string mobi08 = "Pato de Ouro 250c";
                string imagem08 = "CFC_500_goldbar";
                string mobi09 = "Maleta de 5000c";
                string imagem09 = "CFC_500_goldbar";
                int randomNumber = random.Next(1, 9);
                if (Item.UserID != Session.GetHabbo().Id)
                {
                    Session.SendMessage(new RoomNotificationComposer("furni_placement_error", "message", "Esta caja de la suerte no te pertenece."));
                    return;
                }
                Room Room = Session.GetHabbo().CurrentRoom;
                Room.GetRoomItemHandler().RemoveFurniture(null, Item.Id);
                if (randomNumber == 1)
                {
                    using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi1 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi1, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem01 + "_icon", 3, "Acabas de ganar el mobi : " + mobi01 + " #Haga clic aquí para ver en su inventario#", "inventory/open/furni"));
                    return;
                }
                if (randomNumber == 2)
                {
                    using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi2 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi2, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem02 + "_icon", 3, "Acabas de ganar el mobi : " + mobi02 + " #Haga clic aquí para ver en su inventario#", "inventory/open/furni"));
                    return;
                }
                if (randomNumber == 3)
                {
                    using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi3 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi3, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem03 + "_icon", 3, "Acabas de ganar el mobi : " + mobi03 + " #Haga clic aquí para ver en su inventario#", "inventory/open/furni"));
                    return;
                }
                if (randomNumber == 4)
                {
                    using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi4 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi4, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem04 + "_icon", 3, "Acabas de ganar el mobi : " + mobi04 + " #Haga clic aquí para ver en su inventario#", "inventory/open/furni"));
                    return;
                }
                if (randomNumber == 5)
                {
                    using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi5 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi5, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem05 + "_icon", 3, "Acabas de ganar el mobi : " + mobi05 + " #Haga clic aquí para ver en su inventario#", "inventory/open/furni"));
                    return;
                }
                if (randomNumber == 6)
                {
                    using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi6 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi6, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem06 + "_icon", 3, "Acabas de ganar el mobi : " + mobi06 + " #Haga clic aquí para ver en su inventario#", "inventory/open/furni"));
                    return;
                }
                if (randomNumber == 7)
                {
                    using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi7 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi7, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem07 + "_icon", 3, "Acabas de ganar el mobi : " + mobi07 + " #Haga clic aquí para ver en su inventario#", "inventory/open/furni"));
                    return;
                }
                if (randomNumber == 8)
                {
                    using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi8 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi8, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem08 + "_icon", 3, "Acabas de ganar el mobi : " + mobi08 + " #Haga clic aquí para ver en su inventario#", "inventory/open/furni"));
                    return;
                }
                if (randomNumber == 9)
                {
                    using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi9 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi9, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem09 + "_icon", 3, "Acabas de ganar el mobi : " + mobi09 + " #Haga clic aquí para ver en su inventario#", "inventory/open/furni"));
                    return;
                }
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