
using Neon.Communication.Packets.Outgoing.Navigator;
using Neon.HabboHotel.Rooms;

namespace Neon.Communication.Packets.Incoming.Navigator
{
    internal class UpdateNavigatorSettingsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int roomID = Packet.PopInt();
            if (roomID == 0)
            {
                return;
            }

            RoomData Data = NeonEnvironment.GetGame().GetRoomManager().GenerateRoomData(roomID);
            if (Data == null)
            {
                return;
            }

            Session.GetHabbo().HomeRoom = roomID;
            Session.SendMessage(new NavigatorSettingsComposer(roomID));
        }
    }
}
