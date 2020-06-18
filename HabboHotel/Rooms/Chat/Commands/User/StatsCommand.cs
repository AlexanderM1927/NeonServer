using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    class StatsCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_stats"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Revisar tus estadísticas."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            double Minutes = Session.GetHabbo().GetStats().OnlineTime / 60;
            double Hours = Minutes / 60;
            int OnlineTime = Convert.ToInt32(Hours);
            string s = OnlineTime == 1 ? "" : "s";

            StringBuilder HabboInfo = new StringBuilder();
            HabboInfo.Append("Estadistica de tu cuenta:\r\r");

            HabboInfo.Append("Info Monetaria:\r");
            HabboInfo.Append("Creditos: " + Session.GetHabbo().Credits + "\r");
            HabboInfo.Append("Duckets: " + Session.GetHabbo().Duckets + "\r");
            HabboInfo.Append("Diamantes: " + Session.GetHabbo().Diamonds + "\r");
            HabboInfo.Append("Tiempo ON: " + OnlineTime + " Horas" + s + "\r");
            HabboInfo.Append("Respetos: " + Session.GetHabbo().GetStats().Respect + "\r");
            HabboInfo.Append("Puntos de Juego: " + Session.GetHabbo().GOTWPoints + "\r\r");


            Session.SendNotification(HabboInfo.ToString());
        }
    }
}
