using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Neon.HabboHotel.Users;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.GameClients;
using Neon.Communication.Packets.Outgoing.Handshake;

namespace Neon.Communication.Packets.Incoming.Misc
{
    class GetAdsOfferEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new VideoOffersRewardsComposer());
        }
    }
}
