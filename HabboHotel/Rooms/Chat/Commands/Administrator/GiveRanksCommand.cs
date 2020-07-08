using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.Database.Interfaces;
using Neon.HabboHotel.GameClients;
using System;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class GiveRanksCommand : IChatCommand
    {
        public string PermissionRequired => "command_give";

        public string Parameters => "%username% %type% %rank%";

        public string Description => "Escribe :rank para ver la explicación.";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendMessage(new MassEventComposer("habbopages/chat/giverankinfo.txt"));
                return;
            }

            GameClient Target = NeonEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (Target == null)
            {
                Session.SendWhisper("Oops, no se ha conseguido este usuario.");
                return;
            }

            string RankType = Params[2];
            switch (RankType.ToLower())
            {
                case "guia":
                case "guide":
                    {
                        if (Session.GetHabbo().Rank < 6)
                        {
                            Session.SendWhisper("Oops, usted no tiene los permisos necesarios para usar este comando!");
                            break;
                        }
                        else
                        {
                            if (int.TryParse(Params[3], out int Rank))
                            {
                                byte RankByte = Convert.ToByte(Rank);

                                if (Rank > 4 || Rank < 0)
                                {
                                    Session.SendWhisper("No puedes superar la cifra de 4 o colocar una cifra inferior a 0.", 1);
                                    return;
                                }

                                Target.GetHabbo()._guidelevel = RankByte;

                                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                                { dbClient.RunQuery("UPDATE `users` SET `guia` = '" + RankByte + "' WHERE `id` = '" + Target.GetHabbo().Id + "' LIMIT 1"); }

                                switch (RankByte)
                                {
                                    case 0:
                                        Target.SendMessage(RoomNotificationComposer.SendBubble("eventoxx", Session.GetHabbo().Username + " acaba de retirarte del departamento de soporte. Reinicia para aplicar los cambios respectivos.\n\nRecuerda que hemos depositado nuestra confianza en tí y que todo esfuerzo tiene su recompensa.", ""));
                                        Session.SendWhisper("Rango retirado satisfactoriamente a " + Target.GetHabbo().Username + ".");
                                        Target.Disconnect();
                                        break;

                                    case 1:
                                        Target.SendMessage(RoomNotificationComposer.SendBubble("eventoxx", Session.GetHabbo().Username + " acaba de darte el rango de guía conejo. Reinicia para aplicar los cambios respectivos.\n\nRecuerda que hemos depositado nuestra confianza en tí y que todo esfuerzo tiene su recompensa.", ""));
                                        Session.SendWhisper("Rango entregado satisfactoriamente a " + Target.GetHabbo().Username + ".");
                                        break;

                                    case 2:
                                        Target.SendMessage(RoomNotificationComposer.SendBubble("eventoxx", Session.GetHabbo().Username + " acaba de darte el rango de guía búho. Reinicia para aplicar los cambios respectivos.\n\nRecuerda que hemos depositado nuestra confianza en tí y que todo esfuerzo tiene su recompensa.", ""));
                                        Session.SendWhisper("Rango entregado satisfactoriamente a " + Target.GetHabbo().Username + ".");
                                        break;

                                    case 3:
                                        Target.SendMessage(RoomNotificationComposer.SendBubble("eventoxx", Session.GetHabbo().Username + " acaba de darte el rango de encargado de guías. Reinicia para aplicar los cambios respectivos.\n\nRecuerda que hemos depositado nuestra confianza en tí y que todo esfuerzo tiene su recompensa.", ""));
                                        Session.SendWhisper("Rango entregado satisfactoriamente a " + Target.GetHabbo().Username + ".");
                                        break;

                                    case 4:
                                        Target.SendMessage(RoomNotificationComposer.SendBubble("eventoxx", Session.GetHabbo().Username + " acaba de darte el rango de encargado de guías oculto. Reinicia para aplicar los cambios respectivos.\n\nRecuerda que hemos depositado nuestra confianza en tí y que todo esfuerzo tiene su recompensa.", ""));
                                        Session.SendWhisper("Rango entregado satisfactoriamente a " + Target.GetHabbo().Username + ".");
                                        break;
                                }

                                break;
                            }

                            else { Session.SendWhisper("Oops, " + RankType + " no es un valor válido para otorgar."); break; }
                        }
                    }

                case "publi":
                case "publicista":
                    {
                        if (Session.GetHabbo().Rank < 6)
                        {
                            Session.SendWhisper("Oops, usted no tiene los permisos necesarios para usar este comando!");
                            break;
                        }

                        else
                        {
                            if (int.TryParse(Params[3], out int Rank))
                            {
                                byte RankByte = Convert.ToByte(Rank);

                                if (Rank > 4 || Rank < 0)
                                { Session.SendWhisper("No puedes superar la cifra de 4 o colocar una cifra inferior a 0.", 1); return; }

                                Target.GetHabbo()._publicistalevel = RankByte;
                                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                                { dbClient.RunQuery("UPDATE `users` SET `publi` = '" + RankByte + "' WHERE `id` = '" + Target.GetHabbo().Id + "' LIMIT 1"); }

                                switch (RankByte)
                                {
                                    case 0:
                                        Target.SendMessage(RoomNotificationComposer.SendBubble("eventoxx", Session.GetHabbo().Username + " acaba de retirarte del departamento de publicistas. Reinicia para aplicar los cambios respectivos.\n\nRecuerda que hemos depositado nuestra confianza en tí y que todo esfuerzo tiene su recompensa.", ""));
                                        Session.SendWhisper("Rango retirado satisfactoriamente a " + Target.GetHabbo().Username + ".");
                                        Target.Disconnect();
                                        break;

                                    case 1:
                                        Target.SendMessage(RoomNotificationComposer.SendBubble("eventoxx", Session.GetHabbo().Username + " acaba de darte el rango de publicista a prueba. Reinicia para aplicar los cambios respectivos.\n\nRecuerda que hemos depositado nuestra confianza en tí y que todo esfuerzo tiene su recompensa.", ""));
                                        Session.SendWhisper("Rango entregado satisfactoriamente a " + Target.GetHabbo().Username + ".");
                                        break;

                                    case 2:
                                        Target.SendMessage(RoomNotificationComposer.SendBubble("eventoxx", Session.GetHabbo().Username + " acaba de darte el rango de publicista. Reinicia para aplicar los cambios respectivos.\n\nRecuerda que hemos depositado nuestra confianza en tí y que todo esfuerzo tiene su recompensa.", ""));
                                        Session.SendWhisper("Rango entregado satisfactoriamente a " + Target.GetHabbo().Username + ".");
                                        break;

                                    case 3:
                                        Target.SendMessage(RoomNotificationComposer.SendBubble("eventoxx", Session.GetHabbo().Username + " acaba de darte el rango de encargado de publicidad. Reinicia para aplicar los cambios respectivos.\n\nRecuerda que hemos depositado nuestra confianza en tí y que todo esfuerzo tiene su recompensa.", "")); Session.SendWhisper("Rango entregado satisfactoriamente a " + Target.GetHabbo().Username + ".");
                                        break;

                                    case 4:
                                        Target.SendMessage(RoomNotificationComposer.SendBubble("eventoxx", Session.GetHabbo().Username + " acaba de darte el rango de encargado de publicidad oculto. Reinicia para aplicar los cambios respectivos.\n\nRecuerda que hemos depositado nuestra confianza en tí y que todo esfuerzo tiene su recompensa.", ""));
                                        Session.SendWhisper("Rango entregado satisfactoriamente a " + Target.GetHabbo().Username + ".");
                                        break;
                                }
                                break;
                            }
                            else
                            {
                                Session.SendWhisper("Oops, " + Rank + " no es un valor válido para otorgar.");
                                break;
                            }
                        }
                    }

                case "inter":
                case "croupier":
                    {
                        if (Session.GetHabbo().Rank < 6)
                        {
                            Session.SendWhisper("Oops, usted no tiene los permisos necesarios para usar este comando!");
                            break;
                        }
                        else
                        {
                            if (int.TryParse(Params[3], out int Rank))
                            {
                                byte RankByte = Convert.ToByte(Rank);
                                if (Rank > 1 || Rank < 0)
                                { Session.SendWhisper("No puedes superar la cifra de 1 o colocar una cifra inferior a 0.", 1); return; }
                                Target.GetHabbo()._croupier = RankByte;

                                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                                { dbClient.RunQuery("UPDATE `users` SET `croupier` = '" + RankByte + "' WHERE `id` = '" + Target.GetHabbo().Id + "' LIMIT 1"); }

                                switch (RankByte)
                                {
                                    case 0:
                                        Target.SendMessage(RoomNotificationComposer.SendBubble("inters", Session.GetHabbo().Username + " acaba de retirarte del departamento de intermediarios. Reinicia para aplicar los cambios respectivos.\n\nRecuerda que hemos depositado nuestra confianza en tí y que todo esfuerzo tiene su recompensa.", ""));
                                        Session.SendWhisper("Rango retirado satisfactoriamente a " + Target.GetHabbo().Username + ".");
                                        Target.Disconnect();
                                        break;

                                    case 1:
                                        Target.SendMessage(RoomNotificationComposer.SendBubble("inters", Session.GetHabbo().Username + " acaba de darte el cargo de intermediario. Reinicia para aplicar los cambios respectivos.\n\nRecuerda que hemos depositado nuestra confianza en tí y que todo esfuerzo tiene su recompensa.", ""));
                                        Session.SendWhisper("Rango entregado satisfactoriamente a " + Target.GetHabbo().Username + ".");

                                        if (!Target.GetHabbo().GetBadgeComponent().HasBadge("INT3"))
                                        { Target.GetHabbo().GetBadgeComponent().GiveBadge("INT3", true, Target); }
                                        break;
                                }
                                break;
                            }
                            else
                            {
                                Session.SendWhisper("Oops, " + Rank + " no es un valor válido para otorgar.");
                                break;
                            }
                        }
                    }

                case "builder":
                    {
                        if (Session.GetHabbo().Rank < 6)
                        {
                            Session.SendWhisper("Oops, usted no tiene los permisos necesarios para usar este comando!");
                            break;
                        }
                        else
                        {
                            if (int.TryParse(Params[3], out int Rank))
                            {
                                byte RankByte = Convert.ToByte(Rank);
                                if (Rank > 1 || Rank < 0)
                                { Session.SendWhisper("No puedes superar la cifra de 1 o colocar una cifra inferior a 0.", 1); return; }
                                Target.GetHabbo()._builder = RankByte;

                                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                                { dbClient.RunQuery("UPDATE `users` SET `builder` = '" + RankByte + "' WHERE `id` = '" + Target.GetHabbo().Id + "' LIMIT 1"); }

                                switch (RankByte)
                                {
                                    case 0:
                                        Target.SendMessage(RoomNotificationComposer.SendBubble("builder", Session.GetHabbo().Username + " acaba de retirarte del departamento de BAW. Reinicia para aplicar los cambios respectivos.\n\nRecuerda que hemos depositado nuestra confianza en tí y que todo esfuerzo tiene su recompensa.", ""));
                                        Session.SendWhisper("Rango retirado satisfactoriamente a " + Target.GetHabbo().Username + ".");
                                        Target.Disconnect();
                                        break;

                                    case 1:
                                        Target.SendMessage(RoomNotificationComposer.SendBubble("builder", Session.GetHabbo().Username + " acaba de darte el cargo de BAW. Reinicia para aplicar los cambios respectivos.\n\nRecuerda que hemos depositado nuestra confianza en tí y que todo esfuerzo tiene su recompensa.", ""));
                                        Session.SendWhisper("Rango entregado satisfactoriamente a " + Target.GetHabbo().Username + ".");
                                        Target.GetHabbo().Effects().ApplyEffect(599);
                                        //Target.GetHabbo().GetBadgeComponent().GiveBadge("BU1LD", true, Target);
                                        break;
                                }
                                break;
                            }
                            else
                            {
                                Session.SendWhisper("Oops, " + Rank + " no es un valor válido para otorgar.");
                                break;
                            }
                        }
                    }

                default:
                    Session.SendWhisper(RankType + "' no es un rango disponible para otorgar.");
                    break;
            }
        }
    }
}