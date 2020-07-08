namespace Neon.HabboHotel.Rooms.Chat.Commands.User.Fun
{
    internal class MoonwalkCommand : IChatCommand
    {
        public string PermissionRequired => "command_moonwalk";

        public string Parameters => "";

        public string Description => "Ponte en los pies de Michael Jackson.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
            {
                return;
            }

            User.moonwalkEnabled = !User.moonwalkEnabled;

            if (User.moonwalkEnabled)
            {
                Session.SendWhisper("Moonwalk Activado!");
            }
            else
            {
                Session.SendWhisper("Moonwalk desactivado!");
            }
        }
    }
}
