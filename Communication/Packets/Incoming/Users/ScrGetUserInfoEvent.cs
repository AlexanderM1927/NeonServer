using System;
using Neon.Communication.Packets.Incoming;
using Neon.HabboHotel.GameClients;
using Neon.Communication.Packets.Outgoing.Users;
using Neon.Communication.Packets.Outgoing.Handshake;
using Neon.HabboHotel.Rooms;

namespace Neon.Communication.Packets.Incoming.Users
{
    class ScrGetUserInfoEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new ScrSendUserInfoComposer(Session.GetHabbo()));
            Session.SendMessage(new UserRightsComposer(Session.GetHabbo()));

        }
    }
}
