using Neon.Communication.Packets.Outgoing;
using Neon.Communication.Packets.Outgoing.Groups;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Groups.Forums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neon.Communication.Packets.Incoming.Groups
{
    class GetForumStatsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var GroupForumId = Packet.PopInt();

            GroupForum Forum;
            if (!NeonEnvironment.GetGame().GetGroupForumManager().TryGetForum(GroupForumId, out Forum))
            {
                Session.SendNotification("Opss, Forum inexistente!");
                return;
            }

            Session.SendMessage(new GetGroupForumsMessageEvent(Forum, Session));

        }
    }
}
