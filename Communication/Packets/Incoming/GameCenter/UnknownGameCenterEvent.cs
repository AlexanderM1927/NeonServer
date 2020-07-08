using Neon.HabboHotel.GameClients;

namespace Neon.Communication.Packets.Incoming.GameCenter
{
    internal class UnknownGameCenterEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int GameId = Packet.PopInt();
            int UserId = Packet.PopInt();
            if (NeonEnvironment.GetGame().GetGameDataManager().TryGetGame(GameId, out _))
            {
                // Session.SendMessage(new Game2WeeklyLeaderboardComposer(GameId)); Comentado y funciona
            }
        }
    }
}
