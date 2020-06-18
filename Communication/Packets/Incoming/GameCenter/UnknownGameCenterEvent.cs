using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.Games;
using Neon.Communication.Packets.Outgoing.GameCenter;
using System.Data;

using Neon.HabboHotel.Users;
using Neon.HabboHotel.GameClients;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Neon.Communication.Packets.Incoming.GameCenter
{
    class UnknownGameCenterEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int GameId = Packet.PopInt();
            int UserId = Packet.PopInt();

            GameData GameData = null;
            if (NeonEnvironment.GetGame().GetGameDataManager().TryGetGame(GameId, out GameData))
            {
               // Session.SendMessage(new Game2WeeklyLeaderboardComposer(GameId)); Comentado y funciona
            }
        }
    }
}
