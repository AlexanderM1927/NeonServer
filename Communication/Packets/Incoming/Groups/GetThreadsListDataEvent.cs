using Neon.Communication.Packets.Outgoing;
using Neon.Communication.Packets.Outgoing.Groups;
using Neon.HabboHotel.GameClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neon.Communication.Packets.Incoming.Groups
{
    class GetThreadsListDataEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var ForumId = Packet.PopInt(); //Forum ID
            var Int2 = Packet.PopInt(); //Start Index of Thread Count
            var Int3 = Packet.PopInt(); //Length of Thread Count

            var Forum = NeonEnvironment.GetGame().GetGroupForumManager().GetForum(ForumId);
            if (Forum == null)
            {
                return;
            }

            Session.SendMessage(new ThreadsListDataComposer(Forum, Session, Int2, Int3));
        }
    }
}

