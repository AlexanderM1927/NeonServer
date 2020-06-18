using Neon.HabboHotel.Rooms.Chat.Styles;
using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Administrator
{
    class SetBetCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_bubble"; }
        }

        public string Parameters
        {
            get { return "%diamantes%"; }
        }

        public string Description
        {
            get { return "Coloca tu apuesta para la tragaperras."; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            if (Params.Length == 1)
            {
                Session.SendWhisper("Debes introducir un valor en diamantes, por ejemplo :setbet 50.", 34);
                return;
            }

            int Bet = 0;
            if (!int.TryParse(Params[1].ToString(), out Bet))
            {
                Session.SendWhisper("Por favor introduce un número valido.", 34);
                return;
            }
            
            Session.GetHabbo()._bet = Bet;
            Session.SendWhisper("Has establecido tus apuestas a " + Bet + " diamantes. ¡Apuesta con cabeza!", 34);
        }
    }
}