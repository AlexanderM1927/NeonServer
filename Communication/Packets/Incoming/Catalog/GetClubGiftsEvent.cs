using Neon.HabboHotel.GameClients;
using Neon.Communication.Packets.Outgoing.Catalog;

namespace Neon.Communication.Packets.Incoming.Catalog
{
    class GetClubGiftsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new ClubGiftsComposer(Session));
        }
    }
}
