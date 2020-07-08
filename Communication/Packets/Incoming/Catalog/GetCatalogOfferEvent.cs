using Neon.Communication.Packets.Outgoing.Catalog;
using Neon.HabboHotel.Catalog;

namespace Neon.Communication.Packets.Incoming.Catalog
{
    internal class GetCatalogOfferEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int OfferId = Packet.PopInt();
            if (!NeonEnvironment.GetGame().GetCatalog().ItemOffers.ContainsKey(OfferId))
            {
                return;
            }

            int PageId = NeonEnvironment.GetGame().GetCatalog().ItemOffers[OfferId];

            if (!NeonEnvironment.GetGame().GetCatalog().TryGetPage(PageId, out CatalogPage Page))
            {
                return;
            }

            if (!Page.Enabled || !Page.Visible || Page.MinimumRank > Session.GetHabbo().CatRank || (Page.MinimumVIP > Session.GetHabbo().VIPRank && Session.GetHabbo().Rank == 1))
            {
                return;
            }

            if (!Page.ItemOffers.ContainsKey(OfferId))
            {
                return;
            }

            CatalogItem Item = Page.ItemOffers[OfferId];
            if (Item != null)
            {
                Session.SendMessage(new CatalogOfferComposer(Item));
            }
        }
    }
}
