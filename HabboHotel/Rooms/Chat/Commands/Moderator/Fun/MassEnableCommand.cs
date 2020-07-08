using System.Collections.Generic;
using System.Linq;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator.Fun
{
    internal class MassEnableCommand : IChatCommand
    {
        public string PermissionRequired => "command_massenable";

        public string Parameters => "%EffectId%";

        public string Description => "Coloca a todos los de la sala con un efecto.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor introduce el efecto ID.");
                return;
            }

            if (int.TryParse(Params[1], out int EnableId))
            {
                if (EnableId == 102 || EnableId == 178)
                {
                    Session.Disconnect();
                    return;
                }

                if (!Session.GetHabbo().GetPermissions().HasCommand("command_override_massenable") && Room.OwnerId != Session.GetHabbo().Id)
                {
                    Session.SendWhisper("Este comando lo puedes usar solo si eres el dueño.");
                    return;
                }

                List<RoomUser> Users = Room.GetRoomUserManager().GetRoomUsers();
                if (Users.Count > 0)
                {
                    foreach (RoomUser U in Users.ToList())
                    {
                        if (U == null || U.RidingHorse)
                        {
                            continue;
                        }

                        U.ApplyEffect(EnableId);
                    }
                }
            }
            else
            {
                Session.SendWhisper("Por favor introduce el efecto ID.");
                return;
            }

        }
    }
}
