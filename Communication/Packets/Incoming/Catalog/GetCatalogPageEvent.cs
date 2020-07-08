using Neon.Communication.Packets.Outgoing.Catalog;
using Neon.HabboHotel.Catalog;
using Neon.HabboHotel.GameClients;

namespace Neon.Communication.Packets.Incoming.Catalog
{
    public class GetCatalogPageEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int PageId = Packet.PopInt();
            _ = Packet.PopInt();
            string CataMode = Packet.PopString();

            CatalogPage Page = null;
            if (CataMode == "NORMAL")
            {
                if (!NeonEnvironment.GetGame().GetCatalog().TryGetPage(PageId, out Page))
                {
                    return;
                }

                if (!Page.Enabled || !Page.Visible || Page.MinimumRank > Session.GetHabbo().CatRank || (Page.MinimumVIP > Session.GetHabbo().VIPRank && Session.GetHabbo().Rank == 1))
                {
                    return;
                }

                Session.SendMessage(new CatalogPageComposer(Page, CataMode, Session));
            }

            if (CataMode == "BUILDERS_CLUB")
            {
                if (!NeonEnvironment.GetGame().GetCatalog().TryGetBCPage(PageId, out BCCatalogPage BCPage))
                {
                    return;
                }

                if (!BCPage.Enabled || !BCPage.Visible || BCPage.MinimumRank > Session.GetHabbo().CatRank || (BCPage.MinimumVIP > Session.GetHabbo().VIPRank && Session.GetHabbo().Rank == 1))
                {
                    return;
                }

                Session.SendMessage(new BCCatalogPageComposer(BCPage, CataMode));
            }

            Session.GetHabbo().lastLayout = Page.Template;

        }
    }
}