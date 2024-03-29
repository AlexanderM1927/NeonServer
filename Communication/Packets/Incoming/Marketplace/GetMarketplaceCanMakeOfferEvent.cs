﻿
using Neon.Communication.Packets.Outgoing.Marketplace;

namespace Neon.Communication.Packets.Incoming.Marketplace
{
    internal class GetMarketplaceCanMakeOfferEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int ErrorCode = (Session.GetHabbo().TradingLockExpiry > 0 ? 6 : 1);

            Session.SendMessage(new MarketplaceCanMakeOfferResultComposer(ErrorCode));
        }
    }
}