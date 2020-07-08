using Neon.Communication.Packets.Outgoing.Inventory.Purse;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class GiveCommand : IChatCommand
    {
        public string PermissionRequired => "command_give";

        public string Parameters => "%username% %type% %amount%";

        public string Description => "";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor introduce ! (coins, duckets, diamonds, honor)");
                return;
            }

            GameClient Target = NeonEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (Target == null)
            {
                Session.SendWhisper("Oops, No se ha conseguido este usuario!");
                return;
            }

            string UpdateVal = Params[2];
            switch (UpdateVal.ToLower())
            {
                case "coins":
                case "credits":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_coins"))
                        {
                            Session.SendWhisper("Oops, usted no tiene los permisos necesarios para usar este comando!");
                            break;
                        }
                        else
                        {
                            if (int.TryParse(Params[3], out int Amount))
                            {
                                Target.GetHabbo().Credits = Target.GetHabbo().Credits += Amount;
                                Target.SendMessage(new CreditBalanceComposer(Target.GetHabbo().Credits));

                                if (Target.GetHabbo().Id != Session.GetHabbo().Id)
                                {
                                    Target.SendNotification(Session.GetHabbo().Username + " te ha enviado " + Amount.ToString() + " Credito(s)!");
                                }

                                Session.SendWhisper("Le enviaste " + Amount + " Credito(s) a " + Target.GetHabbo().Username + "!");
                                break;
                            }
                            else
                            {
                                Session.SendWhisper("Oops, las cantidades solo en numeros..");
                                break;
                            }
                        }
                    }

                case "pixels":
                case "duckets":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_pixels"))
                        {
                            Session.SendWhisper("Oops, usted no tiene los permisos necesarios para enviar duckets!");
                            break;
                        }
                        else
                        {
                            if (int.TryParse(Params[3], out int Amount))
                            {
                                Target.GetHabbo().Duckets += Amount;
                                Target.SendMessage(new HabboActivityPointNotificationComposer(Target.GetHabbo().Duckets, Amount));

                                if (Target.GetHabbo().Id != Session.GetHabbo().Id)
                                {
                                    Target.SendNotification(Session.GetHabbo().Username + " te ha enviado " + Amount.ToString() + " Ducket(s)!");
                                }

                                Session.SendWhisper("Le enviaste " + Amount + " Ducket(s) a " + Target.GetHabbo().Username + "!");
                                break;
                            }
                            else
                            {
                                Session.SendWhisper("Oops, las cantidades solo en numeros..");
                                break;
                            }
                        }
                    }

                case "diamonds":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_diamonds"))
                        {
                            Session.SendWhisper("Oops, No tiene los permisos necesarios para usar este comando!");
                            break;
                        }
                        else
                        {
                            if (int.TryParse(Params[3], out int Amount))
                            {
                                Target.GetHabbo().Diamonds += Amount;
                                Target.SendMessage(new HabboActivityPointNotificationComposer(Target.GetHabbo().Diamonds, Amount, 5));

                                if (Target.GetHabbo().Id != Session.GetHabbo().Id)
                                {
                                    Target.SendNotification(Session.GetHabbo().Username + " te ha enviado " + Amount.ToString() + " Diamante(s)!");
                                }

                                Session.SendWhisper("Le enviaste " + Amount + " Diamante(s) a " + Target.GetHabbo().Username + "!");
                                break;
                            }
                            else
                            {
                                Session.SendWhisper("Oops, las cantidades solo en numeros..!");
                                break;
                            }
                        }
                    }

                //case "gotw":
                case "pixeles":
                case "honor":
                case "copos":
                case "kks":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_gotw"))
                        {
                            Session.SendWhisper("Oops, no tiene el permiso necesario para usar este comando!");
                            break;
                        }

                        else
                        {
                            if (int.TryParse(Params[3], out int Amount))
                            {
                                if (Session.GetHabbo().Rank < 9 && Amount > 1)
                                {
                                    Session.SendWhisper("No pueden enviar más de 1 punto, esto será notificado a los CEO y se tomarán las medidas oportunas.");
                                    return;
                                }

                                Target.GetHabbo().GOTWPoints = Target.GetHabbo().GOTWPoints + Amount;
                                Target.GetHabbo().UserPoints = Target.GetHabbo().UserPoints + 1;
                                Target.SendMessage(new HabboActivityPointNotificationComposer(Target.GetHabbo().GOTWPoints, Amount, 103));

                                if (Target.GetHabbo().Id != Session.GetHabbo().Id)
                                {
                                    Target.SendMessage(RoomNotificationComposer.SendBubble("eventoxx", "" + Session.GetHabbo().Username + " te acaba de enviar " + Amount + " Kekoins.\nHaz click para ver los premios disponibles.", "catalog/open/habbiween"));
                                }

                                Session.SendMessage(RoomNotificationComposer.SendBubble("eventoxx", "Acabas de enviar " + Amount + " Kekoins a " + Target.GetHabbo().Username + "\nRecuerda que hemos depositado tu confianza en tí y que estos comandos los vemos en directo.", "catalog/open/habbiween"));
                                NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Target, "ACH_AnimationRanking", 1);
                                break;
                            }
                            else
                            {
                                Session.SendWhisper("Sólo puedes introducir parámetros numerales, de 1 a 50.");
                                break;
                            }
                        }
                    }
                default:
                    Session.SendWhisper("'" + UpdateVal + "' no es una moneda válida.");
                    break;
            }
        }
    }
}