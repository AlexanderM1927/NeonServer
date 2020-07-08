using Neon.HabboHotel.Cache;
using Neon.HabboHotel.Users.Messenger;
using System.Collections.Generic;

namespace Neon.Communication.Packets.Outgoing.Messenger
{
    internal class BuddyRequestsComposer : ServerPacket
    {
        public BuddyRequestsComposer(ICollection<MessengerRequest> Requests)
            : base(ServerPacketHeader.BuddyRequestsMessageComposer)
        {
            base.WriteInteger(Requests.Count);
            base.WriteInteger(Requests.Count);

            foreach (MessengerRequest Request in Requests)
            {
                base.WriteInteger(Request.From);
                base.WriteString(Request.Username);

                UserCache User = NeonEnvironment.GetGame().GetCacheManager().GenerateUser(Request.From);
                base.WriteString(User != null ? User.Look : "");
            }
        }
    }
}
