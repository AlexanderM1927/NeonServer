
using Neon.Communication.Packets.Outgoing.Help;
using Neon.HabboHotel.GameClients;

namespace Neon.Communication.Packets.Incoming.Help
{
    internal class GetSanctionStatusEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new SanctionStatusComposer());
        }
    }
}
