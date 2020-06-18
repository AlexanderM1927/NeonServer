using System;

using Neon.Communication.Packets.Incoming;
using Neon.HabboHotel.Groups;
using Neon.HabboHotel.GameClients;
using Neon.Communication.Packets.Outgoing.Handshake;

namespace Neon.Communication.Packets.Incoming.Handshake
{
    public class InfoRetrieveEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new UserObjectComposer(Session.GetHabbo()));
            Session.SendMessage(new UserPerksComposer(Session.GetHabbo()));
        }
    }
}