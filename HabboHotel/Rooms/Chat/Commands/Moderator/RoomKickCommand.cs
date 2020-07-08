using System.Linq;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class RoomKickCommand : IChatCommand
    {
        public string PermissionRequired => "command_room_kick";

        public string Parameters => "%message%";

        public string Description => "Expulsa a todos los usuarios de una sala y enviale un mensaje.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor dale una razon a los usuarios.");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 1);
            foreach (RoomUser RoomUser in Room.GetRoomUserManager().GetUserList().ToList())
            {
                if (RoomUser == null || RoomUser.IsBot || RoomUser.GetClient() == null || RoomUser.GetClient().GetHabbo() == null || RoomUser.GetClient().GetHabbo().GetPermissions().HasRight("mod_tool") || RoomUser.GetClient().GetHabbo().Id == Session.GetHabbo().Id)
                {
                    continue;
                }

                RoomUser.GetClient().SendNotification("Usted ha sido expulsado por un moderador por la siguiente razon: " + Message);

                Room.GetRoomUserManager().RemoveUserFromRoom(RoomUser.GetClient(), true, false);
            }

            Session.SendWhisper("Expulso a todos correctamente");
        }
    }
}
