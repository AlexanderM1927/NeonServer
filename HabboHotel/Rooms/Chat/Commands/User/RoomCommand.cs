using Neon.Communication.Packets.Outgoing.Rooms.Engine;
using Neon.Database.Interfaces;
using System.Text;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class RoomCommand : IChatCommand
    {
        public string PermissionRequired => "command_room";

        public string Parameters => "respect/pets/push/pull/spull/spush/enable";

        public string Description => "Activar o desactivar estas funciones dentro de tu sala.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Oops, debes elegir una opcion para desactivar. Ejecuta el comando ':room list'", 34);
                return;
            }

            if (!Room.CheckRights(Session, true))
            {
                Session.SendWhisper("Oops, solo el dueño de la sala puede ejecutar este comando", 34);
                return;
            }

            string Option = Params[1];
            switch (Option)
            {
                case "list":
                    {
                        StringBuilder List = new StringBuilder("");
                        List.AppendLine("Lista de comando en salas");
                        List.AppendLine("-------------------------");
                        List.AppendLine("Pet Morphs: " + (Room.PetMorphsAllowed == true ? "Habilitado" : "Deshabilitado"));
                        List.AppendLine("Pull: " + (Room.PullEnabled == true ? "Habilitado" : "Deshabilitado"));
                        List.AppendLine("Push: " + (Room.PushEnabled == true ? "Habilitado" : "Deshabilitado"));
                        //List.AppendLine("Golpes: " + (Room.GolpeEnabled == true ? "Habilitado" : "Deshabilitado"));
                        List.AppendLine("Super Pull: " + (Room.SPullEnabled == true ? "Habilitado" : "Deshabilitado"));
                        List.AppendLine("Super Push: " + (Room.SPushEnabled == true ? "Habilitado" : "Deshabilitado"));
                        List.AppendLine("Respect: " + (Room.RespectNotificationsEnabled == true ? "Habilitado" : "Deshabilitado"));
                        List.AppendLine("Enables: " + (Room.EnablesEnabled == true ? "Habilitado" : "Deshabilitado"));
                        //List.AppendLine("Besos: " + (Room.BesosEnabled == true ? "Habilitado" : "Deshabilitado"));
                        //List.AppendLine("Quemar: " + (Room.QuemarEnabled == true ? "Habilitado" : "Deshabilitado"));
                        Session.SendNotification(List.ToString());
                        break;
                    }

                /*case "golpe":
                {
                    Room.GolpeEnabled = !Room.GolpeEnabled;
                    using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("UPDATE `rooms` SET `golpe_enabled` = @GolpeEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                        dbClient.AddParameter("GolpeEnabled", NeonEnvironment.BoolToEnum(Room.GolpeEnabled));
                        dbClient.RunQuery();
                    }

                    Session.SendWhisper("Los golpes en esta sala estan " + (Room.GolpeEnabled == true ? "Habilitado!" : "Deshabilitado!"));
                    break;
                }

                case "quemar":
                    {
                        Room.QuemarEnabled = !Room.QuemarEnabled;
                        using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `quemar_enabled` = @QuemarEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("QuemarEnabled", NeonEnvironment.BoolToEnum(Room.QuemarEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("¡El poder de quemar en esta sala está " + (Room.QuemarEnabled == true ? "habilitado!" : "deshabilitado!"));
                        break;
                    }

                case "beso":
                    {
                        Room.BesosEnabled = !Room.BesosEnabled;
                        using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `besos_enabled` = @BesosENabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("BesosEnabled", NeonEnvironment.BoolToEnum(Room.QuemarEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("¡El poder de besar en esta sala está " + (Room.QuemarEnabled == true ? "habilitado!" : "deshabilitado!"));
                        break;
                    }*/

                case "push":
                    {
                        Room.PushEnabled = !Room.PushEnabled;
                        using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `push_enabled` = @PushEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("PushEnabled", NeonEnvironment.BoolToEnum(Room.PushEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("Modo Push ahora esta " + (Room.PushEnabled == true ? "Habilitado!" : "Deshabilitado!"));
                        break;
                    }

                case "spush":
                    {
                        Room.SPushEnabled = !Room.SPushEnabled;
                        using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `spush_enabled` = @PushEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("PushEnabled", NeonEnvironment.BoolToEnum(Room.SPushEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("Modo Super Push ahora esta " + (Room.SPushEnabled == true ? "Habilitado!" : "Deshabilitado!"));
                        break;
                    }

                case "spull":
                    {
                        Room.SPullEnabled = !Room.SPullEnabled;
                        using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `spull_enabled` = @PullEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("PullEnabled", NeonEnvironment.BoolToEnum(Room.SPullEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("Modo Super Pull ahora esta  " + (Room.SPullEnabled == true ? "Habilitado!" : "Deshabilitado!"));
                        break;
                    }

                case "pull":
                    {
                        Room.PullEnabled = !Room.PullEnabled;
                        using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `pull_enabled` = @PullEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("PullEnabled", NeonEnvironment.BoolToEnum(Room.PullEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("Modo Pull ahora esta " + (Room.PullEnabled == true ? "Habilitado!" : "Deshabilitado!"));
                        break;
                    }

                case "enable":
                case "enables":
                    {
                        Room.EnablesEnabled = !Room.EnablesEnabled;
                        using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `enables_enabled` = @EnablesEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("EnablesEnabled", NeonEnvironment.BoolToEnum(Room.EnablesEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("Los efectos en esta sala estan " + (Room.EnablesEnabled == true ? "Habilitados!" : "Deshabilitados!"));
                        break;
                    }

                case "respect":
                    {
                        Room.RespectNotificationsEnabled = !Room.RespectNotificationsEnabled;
                        using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `respect_notifications_enabled` = @RespectNotificationsEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("RespectNotificationsEnabled", NeonEnvironment.BoolToEnum(Room.RespectNotificationsEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("Notificaciones de Respeto estan " + (Room.RespectNotificationsEnabled == true ? "Habilitado!" : "Deshabilitado!"));
                        break;
                    }

                case "pets":
                case "morphs":
                    {
                        Room.PetMorphsAllowed = !Room.PetMorphsAllowed;
                        using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `pet_morphs_allowed` = @PetMorphsAllowed WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("PetMorphsAllowed", NeonEnvironment.BoolToEnum(Room.PetMorphsAllowed));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("Que se convierta en mascotas esta " + (Room.PetMorphsAllowed == true ? "Habilitado!" : "Deshabilitado!"));

                        if (!Room.PetMorphsAllowed)
                        {
                            foreach (RoomUser User in Room.GetRoomUserManager().GetRoomUsers())
                            {
                                if (User == null || User.GetClient() == null || User.GetClient().GetHabbo() == null)
                                {
                                    continue;
                                }

                                User.GetClient().SendWhisper("El propietario de la sala ha deshabilitado la opcion de convertirse en mascota.");
                                if (User.GetClient().GetHabbo().PetId > 0)
                                {
                                    //Tell the user what is going on.
                                    User.GetClient().SendWhisper("Oops, el dueño de la sala solo permite Usuarios normales, no mascotas..");

                                    //Change the users Pet Id.
                                    User.GetClient().GetHabbo().PetId = 0;

                                    //Quickly remove the old user instance.
                                    Room.SendMessage(new UserRemoveComposer(User.VirtualId));

                                    //Add the new one, they won't even notice a thing!!11 8-)
                                    Room.SendMessage(new UsersComposer(User));
                                }
                            }
                        }
                        break;
                    }
            }
        }
    }
}
