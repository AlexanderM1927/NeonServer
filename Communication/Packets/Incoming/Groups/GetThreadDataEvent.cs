using Neon.Communication.Packets.Outgoing.Groups;
using Neon.HabboHotel.GameClients;

namespace Neon.Communication.Packets.Incoming.Groups
{
    internal class GetThreadDataEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int ForumId = Packet.PopInt(); //Maybe Forum ID
            int ThreadId = Packet.PopInt(); //Maybe Thread ID
            int StartIndex = Packet.PopInt(); //Start index
            int length = Packet.PopInt(); //List Length

            HabboHotel.Groups.Forums.GroupForum Forum = NeonEnvironment.GetGame().GetGroupForumManager().GetForum(ForumId);

            if (Forum == null)
            {
                Session.SendNotification(";forum.thread.open.error.forumnotfound");
                return;
            }

            HabboHotel.Groups.Forums.GroupForumThread Thread = Forum.GetThread(ThreadId);
            if (Thread == null)
            {
                Session.SendNotification(";forum.thread.open.error.threadnotfound");
                return;
            }

            if (Thread.DeletedLevel > 1 && (Forum.Settings.GetReasonForNot(Session, Forum.Settings.WhoCanModerate) != ""))
            {
                Session.SendNotification((";forum.thread.open.error.deleted"));
                return;
            }


            Session.SendMessage(new ThreadDataComposer(Thread, StartIndex, length));

        }
    }
}
