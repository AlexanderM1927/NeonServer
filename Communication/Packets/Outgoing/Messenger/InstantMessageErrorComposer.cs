﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.Users.Messenger;

namespace Neon.Communication.Packets.Outgoing.Messenger
{
    class InstantMessageErrorComposer : ServerPacket
    {
        public InstantMessageErrorComposer(MessengerMessageErrors Error, int Target)
            : base(ServerPacketHeader.InstantMessageErrorMessageComposer)
        {
            base.WriteInteger(MessengerMessageErrorsUtility.GetMessageErrorPacketNum(Error));
            base.WriteInteger(Target);
           base.WriteString("");
        }
    }
}