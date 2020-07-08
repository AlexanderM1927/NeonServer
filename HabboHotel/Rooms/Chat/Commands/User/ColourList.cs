using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class ColourList : IChatCommand
    {
        public string PermissionRequired => "command_info";

        public string Parameters => "";

        public string Description => "Información de Neon.";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            Session.SendMessage(new RoomNotificationComposer("Lista de colores:",
                 "<font color='#FF8000'><b>COLORES:</b>\n" +
                 "<font size=\"12\" color=\"#1C1C1C\">El comando :color te permitirá fijar un color que tu desees en tu bocadillo de chat, para poder seleccionar el color deberás especificarlo después de hacer el comando, como por ejemplo:<br><i>:color red</i></font>" +
                 "<font size =\"13\" color=\"#0B4C5F\"><b>Stats:</b></font>\n" +
                 "<font size =\"11\" color=\"#1C1C1C\">  <b> · Users: </b> \r  <b> · Rooms: </b> \r  <b> · Uptime: </b>minutes.</font>\n\n" +
                 "", "quantum", ""));
        }
    }
}