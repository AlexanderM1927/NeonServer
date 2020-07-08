
using Neon.Communication.Packets.Outgoing.Help;

namespace Neon.Communication.Packets.Incoming.Help
{
    internal class SendBullyReportEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new SendBullyReportComposer());
        }
    }
}
