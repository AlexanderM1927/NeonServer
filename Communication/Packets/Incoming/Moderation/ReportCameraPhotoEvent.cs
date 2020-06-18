using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.Support;
using Neon.HabboHotel.Rooms.Chat.Moderation;
using Neon.Communication.Packets.Outgoing.Moderation;
using Neon.HabboHotel.Moderation;

namespace Neon.Communication.Packets.Incoming.Moderation
{
    class ReportCameraPhotoEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;

            //if (NeonEnvironment.GetGame().GetModerationManager().(Session.GetHabbo().Id))
            //{
            //    Session.SendMessage(new BroadcastMessageAlertComposer("You currently already have a pending ticket, please wait for a response from a moderator."));
            //    return;
            //}

            int photoId;

            if (!int.TryParse(Packet.PopString(), out photoId))
            {
                return;
            }

            int roomId = Packet.PopInt();
            int creatorId = Packet.PopInt();
            int categoryId = Packet.PopInt();

           // NeonEnvironment.GetGame().GetModerationTool().SendNewTicket(Session, categoryId, creatorId, "", new List<string>(), (int) ModerationSupportTicketType.PHOTO, photoId);
            NeonEnvironment.GetGame().GetClientManager().ModAlert("A new support ticket has been submitted!");
        }
    }
}