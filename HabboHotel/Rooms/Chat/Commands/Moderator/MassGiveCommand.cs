using Neon.Communication.Packets.Outgoing.Inventory.Purse;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.GameClients;
using System.Linq;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class MassGiveCommand : IChatCommand
    {
        public string PermissionRequired => "command_massgive";

        public string Parameters => "%moneda% %cantidad%";

        public string Description => "";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Debes introducir el tipo de moneda: <b>credits</b>, <b>duckets</b>, <b>diamonds</b>, <b>kks</b>.", 34);
                return;
            }

            string UpdateVal = Params[1];
            switch (UpdateVal.ToLower())
            {
                case "credits":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_massgive"))
                        {
                            Session.SendWhisper("No tienes los permisos necesarios para usar este comando.", 34);
                            break;
                        }
                        else
                        {
                            if (int.TryParse(Params[2], out int Amount))
                            {
                                foreach (GameClient Target in NeonEnvironment.GetGame().GetClientManager().GetClients.ToList())
                                {
                                    if (Target == null || Target.GetHabbo() == null || Target.GetHabbo().Username == Session.GetHabbo().Username)
                                    {
                                        continue;
                                    }

                                    Target.GetHabbo().Credits = Target.GetHabbo().Credits += Amount;
                                    Target.SendMessage(new CreditBalanceComposer(Target.GetHabbo().Credits));
                                    Target.SendMessage(RoomNotificationComposer.SendBubble("cred", "" + Session.GetHabbo().Username + " te acaba de enviar " + Amount + " créditos.", ""));

                                }

                                break;
                            }
                            else
                            {
                                Session.SendWhisper("Sólo puedes introducir cantidades numerales.", 34);
                                break;
                            }
                        }
                    }

                case "duckets":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_massgive"))
                        {
                            Session.SendWhisper("No tienes los permisos necesarios para usar este comando.", 34);
                            break;
                        }
                        else
                        {
                            if (int.TryParse(Params[2], out int Amount))
                            {
                                foreach (GameClient Target in NeonEnvironment.GetGame().GetClientManager().GetClients.ToList())
                                {
                                    if (Target == null || Target.GetHabbo() == null || Target.GetHabbo().Username == Session.GetHabbo().Username)
                                    {
                                        continue;
                                    }

                                    Target.GetHabbo().Duckets += Amount;
                                    Target.SendMessage(new HabboActivityPointNotificationComposer(Target.GetHabbo().Duckets, Amount));
                                    Target.SendMessage(RoomNotificationComposer.SendBubble("duckets", "" + Session.GetHabbo().Username + " te acaba de enviar " + Amount + " fuegos.", ""));
                                }

                                break;
                            }
                            else
                            {
                                Session.SendWhisper("Sólo puedes introducir cantidades numerales.", 34);
                                break;
                            }
                        }
                    }



                case "diamonds":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_massgive"))
                        {
                            Session.SendWhisper("No tienes los permisos necesarios para usar este comando.", 34);
                            break;
                        }
                        else
                        {
                            if (int.TryParse(Params[2], out int Amount))
                            {
                                foreach (GameClient Target in NeonEnvironment.GetGame().GetClientManager().GetClients.ToList())
                                {
                                    if (Target == null || Target.GetHabbo() == null || Target.GetHabbo().Username == Session.GetHabbo().Username)
                                    {
                                        continue;
                                    }

                                    Target.GetHabbo().Diamonds += Amount;
                                    Target.SendMessage(new HabboActivityPointNotificationComposer(Target.GetHabbo().Diamonds, Amount, 5));

                                }

                                break;
                            }
                            else
                            {
                                Session.SendWhisper("Sólo puedes introducir cantidades numerales.", 34);
                                break;
                            }
                        }
                    }

                case "reward":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_massgive"))
                        {
                            Session.SendWhisper("Oops, No tiene el permiso necesario para usar este comando!");
                            break;
                        }

                        else
                        {
                            foreach (GameClient Target in NeonEnvironment.GetGame().GetClientManager().GetClients.ToList())
                            {
                                if (Target == null || Target.GetHabbo() == null)
                                {
                                    continue;
                                }

                                Target.SendMessage(NeonEnvironment.GetGame().GetNuxUserGiftsManager().NuxUserGifts.Serialize());
                            }
                        }

                    }
                    break;

                case "pixeles":
                case "honor":
                case "kks":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_massgive"))
                        {
                            Session.SendWhisper("No tienes los permisos necesarios para usar este comando.", 34);
                            break;
                        }

                        else
                        {
                            if (int.TryParse(Params[2], out int Amount))
                            {
                                if (Amount > 50)
                                {
                                    Session.SendWhisper("No pueden enviar más de 50 Puntos, esto será notificado al CEO y tomará medidas.");
                                    return;
                                }

                                foreach (GameClient Target in NeonEnvironment.GetGame().GetClientManager().GetClients.ToList())
                                {
                                    if (Target == null || Target.GetHabbo() == null || Target.GetHabbo().Username == Session.GetHabbo().Username)
                                    {
                                        continue;
                                    }

                                    Target.GetHabbo().GOTWPoints = Target.GetHabbo().GOTWPoints + Amount;
                                    Target.GetHabbo().UserPoints = Target.GetHabbo().UserPoints + 1;
                                    Target.SendMessage(new HabboActivityPointNotificationComposer(Target.GetHabbo().GOTWPoints, Amount, 103));
                                    Target.SendMessage(RoomNotificationComposer.SendBubble("pumpkinz", "" + Session.GetHabbo().Username + " te acaba de enviar " + Amount + " Puntos de Juego.\nHaz click para ver los premios disponibles.", "catalog/open/habbiween")); /*(RoomNotificationComposer.SendBubble("honor", "" + Session.GetHabbo().Username + " te acaba de enviar " + Amount + " puntos de honor.", ""));*/
                                }


                                break;
                            }
                            else
                            {
                                Session.SendWhisper("No tienes los permisos necesarios para usar este comando.", 34);
                                break;
                            }
                        }
                    }
                default:
                    Session.SendWhisper("¡'" + UpdateVal + "' no es una moneda válida!", 34);
                    break;
            }
        }
    }
}
