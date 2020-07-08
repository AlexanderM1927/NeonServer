using Neon.Communication.Packets.Outgoing.Rooms.Engine;
using Neon.HabboHotel.GameClients;

namespace Neon.Communication.Packets.Incoming.Rooms.Engine
{
    internal class GetFurnitureAliasesEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new FurnitureAliasesComposer());
        }
    }
}
