using Neon.HabboHotel.GameClients;
using Neon.Communication.Packets.Incoming;

namespace Neon.Communication.Packets.Incoming.Handshake
{
    public class GetClientVersionEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string Build = Packet.PopString();

            if (NeonEnvironment.SWFRevision != Build)
                NeonEnvironment.SWFRevision = Build;
        }
    }
}