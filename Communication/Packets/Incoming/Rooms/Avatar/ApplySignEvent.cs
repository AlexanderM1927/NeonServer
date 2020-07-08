using Neon.HabboHotel.Rooms;
using System;

namespace Neon.Communication.Packets.Incoming.Rooms.Avatar
{
    internal class ApplySignEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int SignId = Packet.PopInt();

            if (!NeonEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room Room))
            {
                return;
            }

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
            {
                return;
            }

            User.UnIdle();

            User.AddStatus("sign", Convert.ToString(SignId));
            User.UpdateNeeded = true;
            User.SignTime = NeonEnvironment.GetUnixTimestamp() + 5;
        }
    }
}