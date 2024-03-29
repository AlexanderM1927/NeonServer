﻿using Neon.Communication.Packets.Outgoing.Groups;
using Neon.HabboHotel.GameClients;

namespace Neon.Communication.Packets.Incoming.Groups
{
    internal class UpdateForumSettingsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int ForumId = Packet.PopInt();
            int WhoCanRead = Packet.PopInt();
            int WhoCanReply = Packet.PopInt();
            int WhoCanPost = Packet.PopInt();
            int WhoCanMod = Packet.PopInt();


            HabboHotel.Groups.Forums.GroupForum forum = NeonEnvironment.GetGame().GetGroupForumManager().GetForum(ForumId);

            if (forum == null)
            {
                Session.SendNotification(("forums.not.found"));
                return;
            }

            if (forum.Settings.GetReasonForNot(Session, forum.Settings.WhoCanModerate) != "")
            {
                Session.SendNotification(("forums.settings.update.error.rights"));
                return;
            }

            forum.Settings.WhoCanRead = WhoCanRead;
            forum.Settings.WhoCanModerate = WhoCanMod;
            forum.Settings.WhoCanPost = WhoCanReply;
            forum.Settings.WhoCanInitDiscussions = WhoCanPost;

            forum.Settings.Save();

            Session.SendMessage(new GetGroupForumsMessageEvent(forum, Session));
            Session.SendMessage(new ThreadsListDataComposer(forum, Session));

        }
    }


}
