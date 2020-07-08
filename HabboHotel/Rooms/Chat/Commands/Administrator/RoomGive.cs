using Neon.Communication.Packets.Outgoing.Inventory.Purse;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.GameClients;
using System.Linq;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class RoomGiveCommand : IChatCommand
    {
        public string PermissionRequired => "command_room_give";

        public string Parameters => "%type% %amount%";

        public string Description => "";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor introduce ! (coins, duckets, diamonds, pixeles)");
                return;
            }

            string UpdateVal = Params[1];
            switch (UpdateVal.ToLower())
            {
                case "diamonds":
                    {
                        if (Params.Length == 1)
                        {
                            Session.SendWhisper("Introduce el numero de diamantes");
                            return;
                        }
                        else
                        {
                            if (int.TryParse(Params[2], out int Amount))
                            {
                                foreach (RoomUser User in Room.GetRoomUserManager().GetUserList().ToList())
                                {
                                    User.GetClient().GetHabbo().Diamonds += Amount;
                                    User.GetClient().SendMessage(new HabboActivityPointNotificationComposer(User.GetClient().GetHabbo().Diamonds, Amount, 5));
                                    User.GetClient().SendMessage(new RoomCustomizedAlertComposer(Session.GetHabbo().Username + " te acaba de regalar " + Amount + " Diamantes."));
                                }
                            }
                        }
                        Session.SendWhisper("Enviaste correctamente en la sala " + Params[2] + " diamantes!");
                    }
                    break;

                case "reward":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_roomgive_reward"))
                        {
                            Session.SendWhisper("Oops, No tiene el permiso necesario para usar este comando!");
                            break;
                        }

                        else
                        {
                            foreach (RoomUser User in Room.GetRoomUserManager().GetUserList().ToList())
                            {
                                User.GetClient().SendMessage(NeonEnvironment.GetGame().GetNuxUserGiftsManager().NuxUserGifts.Serialize());
                            }
                        }
                    }
                    break;

                case "pixeles":
                    {
                        if (Params.Length == 1)
                        {
                            Session.SendWhisper("Introduce el numero de pixeles");
                            return;
                        }
                        else
                        {
                            if (int.TryParse(Params[2], out int Amount))
                            {
                                if (Amount > 50)
                                {
                                    Session.SendWhisper("No pueden enviar más de 50 Pixeles, esto será notificado al CEO y tomará medidas.");
                                    return;
                                }
                            }

                            foreach (RoomUser User in Room.GetRoomUserManager().GetUserList().ToList())
                            {
                                User.GetClient().GetHabbo().GOTWPoints += Amount;
                                User.GetClient().SendMessage(new HabboActivityPointNotificationComposer(User.GetClient().GetHabbo().GOTWPoints, Amount, 103));
                                User.GetClient().SendMessage(new RoomCustomizedAlertComposer(Session.GetHabbo().Username + " te acaba de regalar " + Amount + " Pixeles."));
                            }
                        }
                        Session.SendWhisper("Enviaste correctamente en la sala " + Params[2] + " pixeles!");
                    }
                    break;
            }
        }
    }
}
