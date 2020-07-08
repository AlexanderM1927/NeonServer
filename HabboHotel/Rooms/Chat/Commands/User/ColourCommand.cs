namespace Neon.HabboHotel.Rooms.Chat.Commands.User.Fun
{
    internal class ColourCommand : IChatCommand
    {

        public string PermissionRequired => "command_dnd";
        public string Parameters => "";
        public string Description => "off/red/green/blue/purple";
        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Debes seleccionar el color que quieres.");
                return;
            }
            string chatColour = Params[1];
            string Colour = chatColour.ToLower();
            switch (chatColour)
            {
                case "none":
                case "black":
                case "off":
                    Session.GetHabbo().chatColour = "";
                    Session.SendWhisper("Tu chat de color ha sido desactivado.");
                    break;
                case "blue":
                case "red":
                case "cyan":
                case "purple":
                case "green":
                    Session.GetHabbo().chatColour = chatColour;
                    Session.SendWhisper("@" + Colour + "@Has establecido tu color a: " + Colour + "");
                    break;
                default:
                    Session.SendWhisper("El color: " + Colour + " no existe.");
                    break;
            }
            return;
        }
    }
}