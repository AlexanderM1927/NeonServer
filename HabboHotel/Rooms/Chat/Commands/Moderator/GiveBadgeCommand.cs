
using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class GiveBadgeCommand : IChatCommand
    {
        public string PermissionRequired => "command_give_badge";

        public string Parameters => "%username% %badge%";

        public string Description => "Dar una placa a un usuario";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length != 3)
            {
                Session.SendWhisper("Introduce el nombre del usuario a quien deseas enviar una placa!");
                return;
            }

            GameClient TargetClient = NeonEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient != null)
            {
                if (!TargetClient.GetHabbo().GetBadgeComponent().HasBadge(Params[2]))
                {
                    TargetClient.GetHabbo().GetBadgeComponent().GiveBadge(Params[2], true, TargetClient);
                    if (TargetClient.GetHabbo().Id != Session.GetHabbo().Id)
                    {
                        TargetClient.SendNotification("You have just been given a badge!");
                    }
                    else
                    {
                        Session.SendWhisper("Ha enviado correctamente la placa  " + Params[2] + "!");
                    }
                }
                else
                {
                    Session.SendWhisper("Oops, este usuario ya tiene la placa  (" + Params[2] + ") !");
                }

                return;
            }
            else
            {
                Session.SendWhisper("Oops, no se ha encontrado al usuario!");
                return;
            }
        }
    }
}
