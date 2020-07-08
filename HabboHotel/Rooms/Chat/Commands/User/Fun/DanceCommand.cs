using Neon.Communication.Packets.Outgoing.Rooms.Avatar;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User.Fun
{
    internal class DanceCommand : IChatCommand
    {
        public string PermissionRequired => "command_dance";

        public string Parameters => "%DanceId%";

        public string Description => "Activar un baile en tu personaje, de 0 a 4.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            RoomUser ThisUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (ThisUser == null)
            {
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Please enter an ID of a dance.");
                return;
            }

            if (int.TryParse(Params[1], out int DanceId))
            {
                if (DanceId > 4 || DanceId < 0)
                {
                    Session.SendWhisper("The dance ID must be between 0 and 4!");
                    return;
                }

                Session.GetHabbo().CurrentRoom.SendMessage(new DanceComposer(ThisUser, DanceId));
            }
            else
            {
                Session.SendWhisper("Please enter a valid dance ID.");
            }
        }
    }
}
