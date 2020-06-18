using System;
using Neon.Database.Interfaces;
using Neon.HabboHotel.GameClients;
using Neon.Communication.Packets.Outgoing.Handshake;

namespace Neon.Communication.Packets.Incoming.Handshake
{
    public class UniqueIDEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string Junk = Packet.PopString();
            string MachineId = Packet.PopString();

            Session.MachineId = MachineId;

            Session.SendMessage(new SetUniqueIdComposer(MachineId));
        }
    }
}