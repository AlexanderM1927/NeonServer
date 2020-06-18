using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.Global;
using Neon.HabboHotel.Catalog;
using Neon.Communication.Packets.Outgoing;

namespace Neon.Communication.Packets.Incoming.Inventory.Purse
{
   class GetHabboClubWindowEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int _page = 5;

            if (Session.GetHabbo().lastLayout.Equals("loyalty_vip_buy"))
                _page = int.Parse(NeonEnvironment.GetDBConfig().DBData["catalog.hcbuy.id"]);

            CatalogPage page = null;
            if (!NeonEnvironment.GetGame().GetCatalog().TryGetPage(_page, out page))
            
                return;

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
