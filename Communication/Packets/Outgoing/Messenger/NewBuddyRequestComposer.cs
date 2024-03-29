﻿using Neon.HabboHotel.Cache;

namespace Neon.Communication.Packets.Outgoing.Messenger
{
    internal class NewBuddyRequestComposer : ServerPacket
    {
        public NewBuddyRequestComposer(UserCache Habbo)
            : base(ServerPacketHeader.NewBuddyRequestMessageComposer)
        {
            base.WriteInteger(Habbo.Id);
            base.WriteString(Habbo.Username);
            base.WriteString(Habbo.Look);
        }
    }
}
