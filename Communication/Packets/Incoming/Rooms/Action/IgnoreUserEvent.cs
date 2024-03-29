﻿using Neon.Communication.Packets.Outgoing.Rooms.Action;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Users;

namespace Neon.Communication.Packets.Incoming.Rooms.Action
{
    internal class IgnoreUserEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
            {
                return;
            }

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
            {
                return;
            }

            string Username = Packet.PopString();
            Habbo User = NeonEnvironment.GetHabboByUsername(Username);
            if (User == null || Session.GetHabbo().MutedUsers.Contains(User.Id) || User.GetPermissions().HasRight("mod_tool"))
            {
                return;
            }

            Session.GetHabbo().MutedUsers.Add(User.Id);
            Session.SendMessage(new IgnoreStatusComposer(1, Username));

            NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_SelfModIgnoreSeen", 1);
        }
    }
}
