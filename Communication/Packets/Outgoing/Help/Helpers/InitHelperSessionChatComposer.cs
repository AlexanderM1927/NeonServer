﻿using Neon.HabboHotel.Users;

namespace Neon.Communication.Packets.Outgoing.Help.Helpers
{
    internal class InitHelperSessionChatComposer : ServerPacket
    {

        public InitHelperSessionChatComposer(Habbo Habbo1, Habbo Habbo2)
            : base(ServerPacketHeader.InitHelperSessionChatMessageComposer)
        {
            base.WriteInteger(Habbo1.Id);
            base.WriteString(Habbo1.Username);
            base.WriteString(Habbo1.Look);

            base.WriteInteger(Habbo2.Id);
            base.WriteString(Habbo2.Username);
            base.WriteString(Habbo2.Look);




        }
    }
}
