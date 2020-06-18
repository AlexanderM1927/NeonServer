using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Data;
using Neon.Database.Interfaces;
using Neon.HabboHotel.GameClients;

namespace Neon.Communication.Packets.Outgoing.Users
{
    class UserTagsComposer : ServerPacket
    {
        public UserTagsComposer(int UserId, GameClient Session)
            : base(ServerPacketHeader.UserTagsMessageComposer)
        {

            base.WriteInteger(UserId);
            base.WriteInteger(Session.GetHabbo().Tags.Count);//Count of the tags.
            foreach (string tag in Session.GetHabbo().Tags.ToArray())
            {
                base.WriteString(tag);
            }
        }
    }
}
