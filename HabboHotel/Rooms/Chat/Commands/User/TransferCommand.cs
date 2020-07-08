using Neon.Communication.Packets.Outgoing.Inventory.Purse;
using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User.Fun
{
    internal class PayCommand : IChatCommand
    {
        public string PermissionRequired => "command_pagar";
        public string Parameters => "%username% %type% %amount%";
        public string Description => "Pagar a un usuario créditos/diamantes/duckets";
        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor, introduzca un tipo de moneda! monedas/diamantes/duckets.");
                return;
            }

            GameClient TargetClient = NeonEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("¡Ese usuario no se puede encontrar!", 34);
                return;
            }
            else if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
            {
                Session.SendWhisper("No puedes pagar a ti mismmo", 34);
                return;
            }

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
            {
                return;
            }

            string Currenices = Params[2];
            switch (Currenices.ToLower())
            {
                case "monedas":
                case "moneda":
                case "credits":
                case "credit":
                case "creditos":
                case "credito":
                case "c":
                    {
                        if (int.TryParse(Params[3], out int Amount))
                        {
                            if (Session.GetHabbo().Credits < Amount)
                            {
                                Session.SendWhisper("No tienes creditos suficientes", 34);
                                return;
                            }
                            Session.GetHabbo().Credits -= Amount;
                            Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
                            TargetClient.GetHabbo().Credits += Amount;
                            TargetClient.SendMessage(new CreditBalanceComposer(TargetClient.GetHabbo().Credits));
                            Session.SendWhisper("*Has enviado " + Amount + " crédito(s) a " + TargetClient.GetHabbo().Username + "*", 34);
                            TargetClient.SendWhisper("*Has recebido " + Amount + " crédito(s) de " + Session.GetHabbo().Username + "*", 34);
                            break;
                        }
                        else
                        {
                            Session.SendWhisper("¡Esa parece ser una cantidad no válida!", 34);
                            break;
                        }
                    }

                case "diamantes":
                case "diamante":
                case "diamonds":
                case "dm":
                    {
                        if (int.TryParse(Params[3], out int Amount))
                        {
                            if (Session.GetHabbo().Diamonds < Amount)
                            {
                                Session.SendWhisper("No tienes diamantes suficientes", 34);
                                return;
                            }
                            Session.GetHabbo().Diamonds -= Amount;
                            Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, Amount, 5));
                            TargetClient.GetHabbo().Diamonds += Amount;
                            TargetClient.SendMessage(new HabboActivityPointNotificationComposer(TargetClient.GetHabbo().Diamonds, Amount, 5));
                            Session.SendWhisper("*Has enviado " + Amount + " diamante(s) a " + TargetClient.GetHabbo().Username + "*", 34);
                            TargetClient.SendWhisper("*Has recebido " + Amount + " diamante(s) de " + Session.GetHabbo().Username + "*", 34);

                            break;
                        }
                        else
                        {
                            Session.SendWhisper("¡Esa parece ser una cantidad no válida!", 34);
                            break;
                        }
                    }

                case "duckets":
                    {
                        if (int.TryParse(Params[3], out int Amount))
                        {
                            if (Session.GetHabbo().Duckets < Amount)
                            {
                                Session.SendWhisper("No tienes duckets suficientes", 34);
                                return;
                            }
                            Session.GetHabbo().Duckets -= Amount;
                            Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Duckets, Amount));
                            TargetClient.GetHabbo().Duckets += Amount;
                            TargetClient.SendMessage(new HabboActivityPointNotificationComposer(TargetClient.GetHabbo().Duckets, Amount));
                            Session.SendWhisper("*Has enviado " + Amount + " ducket(s) a " + TargetClient.GetHabbo().Username + "*", 34);
                            TargetClient.SendWhisper("*Has recebido " + Amount + " ducket(s) de " + Session.GetHabbo().Username + "*", 34);
                            break;
                        }
                        else
                        {
                            Session.SendWhisper("¡Esa parece ser una cantidad no válida!", 34);
                            break;
                        }
                    }

                case "kekoins":
                case "keko":
                case "kks":
                    {
                        if (int.TryParse(Params[3], out int Amount))
                        {
                            if (Session.GetHabbo().GOTWPoints < Amount)
                            {
                                Session.SendWhisper("No tienes Kekoins suficientes", 34);
                                return;
                            }
                            Session.GetHabbo().GOTWPoints -= Amount;
                            Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().GOTWPoints, Amount, 103));
                            TargetClient.GetHabbo().GOTWPoints += Amount;
                            TargetClient.SendMessage(new HabboActivityPointNotificationComposer(TargetClient.GetHabbo().GOTWPoints, Amount, 103));
                            Session.SendWhisper("*Has enviado " + Amount + " Kekoin(s) a " + TargetClient.GetHabbo().Username + "*", 34);
                            TargetClient.SendWhisper("*Has recebido " + Amount + " Kekoin(s) de " + Session.GetHabbo().Username + "*", 34);
                            break;
                        }
                        else
                        {
                            Session.SendWhisper("¡Esa parece ser una cantidad no válida!", 34);
                            break;
                        }
                    }
            }
        }
    }
}