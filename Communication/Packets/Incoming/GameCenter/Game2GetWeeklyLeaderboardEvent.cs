using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Games;
using System;
using System.Globalization;

namespace Neon.Communication.Packets.Incoming.GameCenter
{
    internal class Game2GetWeeklyLeaderboardEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int GameId = Packet.PopInt();
            int weekNum = new GregorianCalendar(GregorianCalendarTypes.Localized).GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            int lastWeekNum = 0;

            if (weekNum == 1) { lastWeekNum = 52; } else { lastWeekNum = weekNum - 1; }



            if (NeonEnvironment.GetGame().GetGameDataManager().TryGetGame(GameId, out GameData GameData))
            {

            }
        }
    }
}
