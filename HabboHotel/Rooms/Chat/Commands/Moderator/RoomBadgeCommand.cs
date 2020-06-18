using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.Rooms;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class RoomBadgeCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_room_badge"; }
        }

        public string Parameters
        {
            get { return "%badge%"; }
        }

        public string Description
        {
            get { return "Dar placa a todos los de una sala"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Introduce el codigo de la placa que deseas enviar en esta sala.");
                return;
            }

            foreach (RoomUser User in Room.GetRoomUserManager().GetUserList().ToList())
            {
                if (User == null || User.GetClient() == null || User.GetClient().GetHabbo() == null)
                    continue;

                if (!User.GetClient().GetHabbo().GetBadgeComponent().HasBadge(Params[1]))
                {
                    User.GetClient().GetHabbo().GetBadgeComponent().GiveBadge(Params[1], true, User.GetClient());
                    User.GetClient().SendNotification("Acabas de recibir una placa de sala.");
                }
                else
                    User.GetClient().SendWhisper(Session.GetHabbo().Username + " ya tiene esa placa");
            }

            Session.SendWhisper("Enviaste correctamente en la sala el codigo  " + Params[2] + " placa!");
        }
    }
}
