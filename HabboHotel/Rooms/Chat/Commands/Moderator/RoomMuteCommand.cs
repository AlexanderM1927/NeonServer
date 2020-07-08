using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using System.Collections.Generic;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class RoomMuteCommand : IChatCommand
    {
        public string PermissionRequired => "command_roommute";

        public string Parameters => "%razón%";

        public string Description => "Silencia la sala.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor introduce una razón para silenciar la sala.");
                return;
            }

            if (!Room.RoomMuted)
            {
                Room.RoomMuted = true;
            }

            string Msg = CommandManager.MergeParams(Params, 1);

            List<RoomUser> RoomUsers = Room.GetRoomUserManager().GetRoomUsers();
            if (RoomUsers.Count > 0)
            {
                foreach (RoomUser User in RoomUsers)
                {
                    if (User == null || User.GetClient() == null || User.GetClient().GetHabbo() == null || User.GetClient().GetHabbo().Username == Session.GetHabbo().Username)
                    {
                        continue;
                    }

                    User.GetClient().SendMessage(new RoomCustomizedAlertComposer("Esta sala ha salido silenciada por la siguiente razón:\n\n" + Msg + "\n\n - " + Session.GetHabbo().Username + ""));
                }
            }
        }
    }
}
