using Neon.Communication.Packets.Outgoing.Catalog;
using Neon.HabboHotel.GameClients;

namespace Neon.Communication.Packets.Incoming.Catalog
{
    internal class GetClubGiftsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new ClubGiftsComposer(Session));
        }
    }
}
