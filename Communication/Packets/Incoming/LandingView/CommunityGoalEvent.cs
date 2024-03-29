﻿using Neon.Communication.Packets.Outgoing.LandingView;

namespace Neon.Communication.Packets.Incoming.LandingView
{
    internal class CommunityGoalEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new CommunityGoalComposer());
            Session.SendMessage(new DynamicPollLandingComposer(false)); //false pa q pueda votar
        }
    }
}
