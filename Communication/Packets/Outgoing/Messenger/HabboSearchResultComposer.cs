﻿using Neon.HabboHotel.Users.Messenger;
using System.Collections.Generic;
using System.Linq;

namespace Neon.Communication.Packets.Outgoing.Messenger
{
    internal class HabboSearchResultComposer : ServerPacket
    {
        public HabboSearchResultComposer(List<SearchResult> Friends, List<SearchResult> OtherUsers)
            : base(ServerPacketHeader.HabboSearchResultMessageComposer)
        {
            base.WriteInteger(Friends.Count);
            foreach (SearchResult Friend in Friends.ToList())
            {
                bool Online = (NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(Friend.UserId) != null);

                base.WriteInteger(Friend.UserId);
                base.WriteString(Friend.Username);
                base.WriteString(Friend.Motto);
                base.WriteBoolean(Online);
                base.WriteBoolean(false);
                base.WriteString(string.Empty);
                base.WriteInteger(0);
                base.WriteString(Online ? Friend.Figure : "");
                base.WriteString(Friend.LastOnline);
            }

            base.WriteInteger(OtherUsers.Count);
            foreach (SearchResult OtherUser in OtherUsers.ToList())
            {
                bool Online = (NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(OtherUser.UserId) != null);

                base.WriteInteger(OtherUser.UserId);
                base.WriteString(OtherUser.Username);
                base.WriteString(OtherUser.Motto);
                base.WriteBoolean(Online);
                base.WriteBoolean(false);
                base.WriteString(string.Empty);
                base.WriteInteger(0);
                base.WriteString(Online ? OtherUser.Figure : "");
                base.WriteString(OtherUser.LastOnline);
            }
        }
    }
}
