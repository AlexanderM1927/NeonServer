namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator.Fun
{
    internal class TeleportCommand : IChatCommand
    {
        public string PermissionRequired => "command_teleport";

        public string Parameters => "";

        public string Description => "Obten la habilidad de teletransportarte";

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {
            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
            {
                return;
            }

            User.TeleportEnabled = !User.TeleportEnabled;
            Room.GetGameMap().GenerateMaps();
        }
    }
}
