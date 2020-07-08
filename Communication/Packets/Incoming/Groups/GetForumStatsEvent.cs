using Neon.Communication.Packets.Outgoing.Groups;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Groups.Forums;

namespace Neon.Communication.Packets.Incoming.Groups
{
    internal class GetForumStatsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int GroupForumId = Packet.PopInt();

            if (!NeonEnvironment.GetGame().GetGroupForumManager().TryGetForum(GroupForumId, out GroupForum Forum))
            {
                Session.SendNotification("Opss, Forum inexistente!");
                return;
            }

            Session.SendMessage(new GetGroupForumsMessageEvent(Forum, Session));

        }
    }
}
