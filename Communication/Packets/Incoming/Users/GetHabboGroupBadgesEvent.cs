﻿using Neon.Communication.Packets.Outgoing.Users;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Groups;
using Neon.HabboHotel.Rooms;
using System.Collections.Generic;
using System.Linq;

namespace Neon.Communication.Packets.Incoming.Users
{
    internal class GetHabboGroupBadgesEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().InRoom)
            {
                return;
            }

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
            {
                return;
            }

            Dictionary<int, string> Badges = new Dictionary<int, string>();
            foreach (RoomUser User in Room.GetRoomUserManager().GetRoomUsers().ToList())
            {
                if (User.IsBot || User.IsPet || User.GetClient() == null || User.GetClient().GetHabbo() == null)
                {
                    continue;
                }

                if (User.GetClient().GetHabbo().GetStats().FavouriteGroupId == 0 || Badges.ContainsKey(User.GetClient().GetHabbo().GetStats().FavouriteGroupId))
                {
                    continue;
                }

                if (!NeonEnvironment.GetGame().GetGroupManager().TryGetGroup(User.GetClient().GetHabbo().GetStats().FavouriteGroupId, out Group Group))
                {
                    continue;
                }

                if (!Badges.ContainsKey(Group.Id))
                {
                    Badges.Add(Group.Id, Group.Badge);
                }
            }

            if (Session.GetHabbo().GetStats().FavouriteGroupId > 0)
            {
                if (NeonEnvironment.GetGame().GetGroupManager().TryGetGroup(Session.GetHabbo().GetStats().FavouriteGroupId, out Group Group))
                {
                    if (!Badges.ContainsKey(Group.Id))
                    {
                        Badges.Add(Group.Id, Group.Badge);
                    }
                }
            }

            Room.SendMessage(new HabboGroupBadgesComposer(Badges));
            Session.SendMessage(new HabboGroupBadgesComposer(Badges));
        }
    }
}