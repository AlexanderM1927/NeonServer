namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator.Fun
{
    internal class SuperFastwalkCommand : IChatCommand
    {
        public string PermissionRequired => "command_super_fastwalk";

        public string Parameters => "";

        public string Description => "Obten la habilidad de caminar mas y mas rapido";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
            {
                return;
            }

            User.SuperFastWalking = !User.SuperFastWalking;

            if (User.FastWalking)
            {
                User.FastWalking = false;
            }

            Session.SendWhisper("Walking mode updated.");
        }
    }
}
