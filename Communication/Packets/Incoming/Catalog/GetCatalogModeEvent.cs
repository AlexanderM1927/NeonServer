using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.Catalog;
using Neon.Communication.Packets.Outgoing.Catalog;
using Neon.Communication.Packets.Outgoing.BuildersClub;

namespace Neon.Communication.Packets.Incoming.Catalog
{
    class GetCatalogModeEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            string PageMode = Packet.PopString();

            if (PageMode == "NORMAL")
                Session.SendMessage(new CatalogIndexComposer(Session, NeonEnvironment.GetGame().GetCatalog().GetPages(), PageMode));//, Sub));
            else if (PageMode == "BUILDERS_CLUB")
                Session.SendMessage(new CatalogIndexComposer(Session, NeonEnvironment.GetGame().GetCatalog().GetBCPages(), PageMode));
        }
    }
}
