using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.GameClients;

namespace Neon.Communication.Packets.Incoming.Rooms.Connection
{
    public class OpenFlatConnectionEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
            {
                return;
            }

            int RoomId = Packet.PopInt();
            string Password = Packet.PopString();

            if (Session.GetHabbo().Rank > 8 && !Session.GetHabbo().StaffOk)
            {
                Session.SendMessage(new RoomCustomizedAlertComposer("No te has autentificado como Staff del hotel."));
            }

            Session.GetHabbo().PrepareRoom(RoomId, Password);

        }
    }
}