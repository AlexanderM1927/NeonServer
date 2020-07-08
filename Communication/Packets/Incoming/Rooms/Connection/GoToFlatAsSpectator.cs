using Neon.Communication.Packets.Outgoing.Rooms.Session;
using Neon.HabboHotel.GameClients;

namespace Neon.Communication.Packets.Incoming.Rooms.Connection
{
    internal class GoToFlatAsSpectatorEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
            {
                return;
            }

            if (!Session.GetHabbo().EnterRoom(Session.GetHabbo().CurrentRoom))
            {
                Session.SendMessage(new CloseConnectionComposer());
            }
        }
    }
}
