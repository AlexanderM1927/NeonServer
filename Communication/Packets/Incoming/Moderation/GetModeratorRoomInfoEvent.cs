
using Neon.Communication.Packets.Outgoing.Moderation;
using Neon.HabboHotel.Rooms;

namespace Neon.Communication.Packets.Incoming.Moderation
{
    internal class GetModeratorRoomInfoEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
            {
                return;
            }

            int RoomId = Packet.PopInt();

            RoomData Data = NeonEnvironment.GetGame().GetRoomManager().GenerateRoomData(RoomId);
            if (Data == null)
            {
                return;
            }


            if (!NeonEnvironment.GetGame().GetRoomManager().TryGetRoom(RoomId, out Room Room))
            {
                return;
            }

            Session.SendMessage(new ModeratorRoomInfoComposer(Data, (Room.GetRoomUserManager().GetRoomUserByHabbo(Data.OwnerName) != null)));
        }
    }
}
