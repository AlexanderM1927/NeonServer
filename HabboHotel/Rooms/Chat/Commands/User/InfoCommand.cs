using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.GameClients;
using System;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class InfoCommand : IChatCommand
    {
        public string PermissionRequired => "command_info";

        public string Parameters => "";

        public string Description => "Información de Neon.";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(NeonEnvironment.GetUnixTimestamp()).ToLocalTime();

            //string Page = Params[1];

            //ServerPacket packet = new ServerPacket(3);
            //packet.WriteString("Youtube");
            //packet.WriteString("<iframe id=\"youtube-player\" frameborder=\"0\" allowfullscreen=\"1\" allow=\"autoplay; encrypted - media\" title=\"YouTube video player\" width=\"480\" height=\"270\" src=\"https://www.youtube.com/embed/2b1TJbLcneM?autoplay=1&amp;fs=0&amp;modestbranding=1&amp;rel=0&amp;enablejsapi=1&amp;origin=http%3A%2F%2Fhabblive.in&amp;widgetid=1\"></iframe>");

            //if (Session.wsSession != null)
            //    Session.wsSession.send(packet);

            TimeSpan Uptime = DateTime.Now - NeonEnvironment.ServerStarted;
            int OnlineUsers = NeonEnvironment.GetGame().GetClientManager().Count;
            int RoomCount = NeonEnvironment.GetGame().GetRoomManager().Count;

            //Random Random = new Random();

            //int RandomNumber = Random.Next(1, 1000000);
            //Session.SendMessage(new MassEventComposer("habbopages/" + Page + ".txt?" + RandomNumber + ""));

            //Habbo ReportedUser = NeonEnvironment.GetHabboById(Session.GetHabbo().Id);
            //List<string> Chats = new List<string>();

            //Chats.Add("Hola que tal soy un delincuente.");
            //ModerationTicket Ticket = new ModerationTicket(1, 4, 1, UnixTimestamp.GetNow(), 1, Session.GetHabbo(), ReportedUser, "Men esto es una prueba de cojones que procede", Session.GetHabbo().CurrentRoom, Chats);
            //if (!NeonEnvironment.GetGame().GetModerationManager().TryAddTicket(Ticket))
            //    return;

            //Session.SendMessage(new NewYearResolutionCompletedComposer("ADM"));
            Session.SendMessage(new RoomNotificationComposer("Información del servidor",
                "<font color=\"#0489B1\" size=\"18\">[¶] Neon Server:</font>\n\n" +
                "<b>Agradecimientos a:</b>\n" +
                "\t- Nillus\n" +
                "\t- Sledmore\n" +
                "<b>Neon Developers:</b>\n" +
                "\t- DjAlexander\n" +
                "\t- Javas\n" +
                "<b>Informacion Actual</b>:\n" +
                "\t- Usuarios en linea: " + OnlineUsers + "\n" +
                "\t- Salas cargadas: " + RoomCount + "\n" +
                "\t- Tiempo: " + Uptime.Days + " día(s), " + Uptime.Hours + " hora(s) y " + Uptime.Minutes +
                " minuto(s).\n\n \n\n<font size =\"12\" color=\"#0B4C5F\">Echa un vistazo <b> :changelog</b> para las últimas actualizaciones.</font>", "neon", ""));
        }
    }
}