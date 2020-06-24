using Neon.HabboHotel.GameClients;
using Neon.Communication.Packets.Outgoing.Rooms.Camera;

namespace Neon.Communication.Packets.Incoming.Catalog
{
    class GetCameraPriceEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new CameraPriceComposer(1, 1, 0));
        }
    }
}
