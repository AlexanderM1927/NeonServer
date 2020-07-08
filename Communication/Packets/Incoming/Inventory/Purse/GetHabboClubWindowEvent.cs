using Neon.Communication.Packets.Outgoing;
using Neon.HabboHotel.Catalog;

namespace Neon.Communication.Packets.Incoming.Inventory.Purse
{
    internal class GetHabboClubWindowEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int _page = 5;

            if (Session.GetHabbo().lastLayout.Equals("loyalty_vip_buy"))
            {
                _page = int.Parse(NeonEnvironment.GetDBConfig().DBData["catalog.hcbuy.id"]);
            }

            if (!NeonEnvironment.GetGame().GetCatalog().TryGetPage(_page, out CatalogPage page))
            {
                return;
            }

            ServerPacket Message = new ServerPacket(ServerPacketHeader.GetClubComposer);
            Message.WriteInteger(page.Items.Values.Count);

            foreach (CatalogItem catalogItem in page.Items.Values)
            {
                catalogItem.SerializeClub(Message, Session);
            }

            Message.WriteInteger(Packet.PopInt());

            Session.SendMessage(Message);
        }
    }
}
