using Neon.Communication.Packets.Outgoing.BuildersClub;
using Neon.Communication.Packets.Outgoing.Catalog;
using Neon.HabboHotel.GameClients;

namespace Neon.Communication.Packets.Incoming.Catalog
{
    public class GetCatalogIndexEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {

            Session.SendMessage(new CatalogIndexComposer(Session, NeonEnvironment.GetGame().GetCatalog().GetPages(), "NORMAL"));
            Session.SendMessage(new CatalogIndexComposer(Session, NeonEnvironment.GetGame().GetCatalog().GetBCPages(), "BUILDERS_CLUB"));

            Session.SendMessage(new CatalogItemDiscountComposer());
            Session.SendMessage(new BCBorrowedItemsComposer());
        }
    }
}