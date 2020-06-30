using Neon.HabboHotel.Catalog;
using Neon.Communication.Packets.Outgoing.Catalog;

namespace Neon.Communication.Packets.Incoming.Catalog
{
    class GetCatalogOfferEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int OfferId = Packet.PopInt();
            if (!NeonEnvironment.GetGame().GetCatalog().ItemOffers.ContainsKey(OfferId))
                return;

            int PageId = NeonEnvironment.GetGame().GetCatalog().ItemOffers[OfferId];

            if (!NeonEnvironment.GetGame().GetCatalog().TryGetPage(PageId, out CatalogPage Page))
                return;

            if (!Page.Enabled || !Page.Visible || Page.MinimumRank > Session.GetHabbo().Rank || (Page.MinimumVIP > Session.GetHabbo().VIPRank && Session.GetHabbo().Rank == 1))
                return;
            if (!Page.ItemOffers.ContainsKey(OfferId))
                return;

            CatalogItem Item = Page.ItemOffers[OfferId];
            if (Item != null)
                Session.SendMessage(new CatalogOfferComposer(Item));
        }
    }
}
