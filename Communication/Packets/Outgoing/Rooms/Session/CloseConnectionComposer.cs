﻿using Neon.HabboHotel.GameClients;

namespace Neon.Communication.Packets.Outgoing.Rooms.Session
{
    internal class CloseConnectionComposer : ServerPacket
    {
        public CloseConnectionComposer(GameClient Session)
            : base(ServerPacketHeader.CloseConnectionMessageComposer)
        {
            Session.GetHabbo().IsTeleporting = false;
            Session.GetHabbo().TeleportingRoomID = 0;
            Session.GetHabbo().TeleporterId = 0;
            Session.GetHabbo().CurrentRoomId = 0;
        }

        public CloseConnectionComposer()
            : base(ServerPacketHeader.CloseConnectionMessageComposer)
        {

        }
    }
}
