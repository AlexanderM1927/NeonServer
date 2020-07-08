using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class RemoveBadgeCommand : IChatCommand
    {
        public string PermissionRequired => "command_remove_badge";

        public string Parameters => "%username% %badge%";

        public string Description => "Borra la placa a un usuario";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 3)
            {
                GameClient TargetClient = null;
                TargetClient = NeonEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
                if (TargetClient != null)
                {
                    if (!TargetClient.GetHabbo().GetBadgeComponent().HasBadge(Params[2]))
                    {
                        {
                            Session.SendNotification("Este usuario no tiene la placa " + Params[2] + "");
                        }
                    }
                    else
                    {
                        RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                        TargetClient.GetHabbo().GetBadgeComponent().RemoveBadge(Params[2], TargetClient);
                        TargetClient.SendNotification("Tu placa " + Params[2] + " ha sido robada por " + ThisUser.GetUsername() + "!");
                        Session.SendNotification("La placa se le ha removido al usuario");

                    }
                }
            }
            else
            {
                Session.SendNotification("Usuario no encontrado.");
                return;
            }
        }
    }
}