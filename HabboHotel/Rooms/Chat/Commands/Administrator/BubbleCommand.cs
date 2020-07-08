using Neon.HabboHotel.Rooms.Chat.Styles;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Administrator
{
    internal class BubbleCommand : IChatCommand
    {
        public string PermissionRequired => "command_bubble";

        public string Parameters => "%id%";

        public string Description => "Use una burbuja de conversacion con un ID";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
            {
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Oops, usted no ha introducido el ID", 34);
                return;
            }

            if (!int.TryParse(Params[1].ToString(), out int Bubble))
            {
                Session.SendWhisper("Por favor introduce un numero valido.", 34);
                return;
            }

            if (!NeonEnvironment.GetGame().GetChatManager().GetChatStyles().TryGetStyle(Bubble, out ChatStyle Style) || (Style.RequiredRight.Length > 0 && !Session.GetHabbo().GetPermissions().HasRight(Style.RequiredRight)))
            {
                Session.SendWhisper("Oops, No puede utilizar esta burbuja por los permisos de rangos [ Raros: 32, 28]!", 34);
                return;
            }

            User.LastBubble = Bubble;
            Session.GetHabbo().CustomBubbleId = Bubble;
            Session.SendWhisper("Bocadillo ajustado a: " + Bubble);
        }
    }
}