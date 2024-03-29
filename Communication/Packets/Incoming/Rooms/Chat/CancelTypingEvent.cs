﻿using Neon.Communication.Packets.Outgoing.Rooms.Chat;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Rooms;

namespace Neon.Communication.Packets.Incoming.Rooms.Chat
{
    public class CancelTypingEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
            {
                return;
            }

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
            {
                return;
            }

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Username);
            if (User == null)
            {
                return;
            }

            Session.GetHabbo().CurrentRoom.SendMessage(new UserTypingComposer(User.VirtualId, false));
        }
    }
}