using Neon.Communication.Packets.Incoming;
using Neon.HabboHotel.GameClients;

namespace Neon.Communication.Packets
{
    public interface IPacketEvent
    {
        void Parse(GameClient Session, ClientPacket Packet);
    }
}