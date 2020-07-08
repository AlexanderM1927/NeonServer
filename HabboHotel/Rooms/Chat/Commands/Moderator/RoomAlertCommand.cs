namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class RoomAlertCommand : IChatCommand
    {
        public string PermissionRequired => "command_room_alert";

        public string Parameters => "%message%";

        public string Description => "Enviar un mensaje a todos los usuarios en una sala.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor introduce el mensaje que deseas enviar en la sala");
                return;
            }

            if (!Session.GetHabbo().GetPermissions().HasRight("mod_alert") && Room.OwnerId != Session.GetHabbo().Id)
            {
                Session.SendWhisper("Solo puede hacerlo en su propia habitacion..");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 1);
            foreach (RoomUser RoomUser in Room.GetRoomUserManager().GetRoomUsers())
            {
                if (RoomUser == null || RoomUser.GetClient() == null || Session.GetHabbo().Id == RoomUser.UserId)
                {
                    continue;
                }

                RoomUser.GetClient().SendNotification(Message + "\n\n - " + Session.GetHabbo().Username);
            }
            Session.SendWhisper("Mensaje enviado correctamente en la sala.");
        }
    }
}
