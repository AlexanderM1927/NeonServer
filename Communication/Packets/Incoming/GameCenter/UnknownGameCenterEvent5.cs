using Neon.HabboHotel.GameClients;

namespace Neon.Communication.Packets.Incoming.GameCenter
{
    internal class UnknownGameCenterEvent5 : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            _ = Packet.PopInt();
            _ = Packet.PopInt();
            _ = Packet.PopInt();
            _ = Packet.PopInt();
            _ = Packet.PopInt();

        }
    }
}
