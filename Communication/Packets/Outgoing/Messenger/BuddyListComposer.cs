﻿using Neon.HabboHotel.Users;
using Neon.HabboHotel.Users.Messenger;
using Neon.HabboHotel.Users.Relationships;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Neon.Communication.Packets.Outgoing.Messenger
{
    internal class BuddyListComposer : ServerPacket
    {
        public BuddyListComposer(ICollection<MessengerBuddy> Friends, Habbo Player)
            : base(ServerPacketHeader.BuddyListMessageComposer)
        {
            int friendCount = Friends.Count;
            if (Player._guidelevel >= 1)
            {
                friendCount++;
            }

            if (Player.Rank >= 5)
            {
                friendCount++;
            }

            base.WriteInteger(1);
            base.WriteInteger(0);
            List<HabboHotel.Groups.Group> groups = NeonEnvironment.GetGame().GetGroupManager().GetGroupsForUser(Player.Id).Where(c => c.HasChat).ToList();
            base.WriteInteger(friendCount + groups.Count);

            foreach (HabboHotel.Groups.Group gp in groups.ToList())
            {
                base.WriteInteger(int.MinValue + gp.Id);
                base.WriteString(gp.Name);
                base.WriteInteger(1);//Gender.
                base.WriteBoolean(true);
                base.WriteBoolean(false);
                base.WriteString(gp.Badge);
                base.WriteInteger(1); // category id
                base.WriteString(string.Empty);
                base.WriteString("Chat de Grupo");//Alternative name?
                base.WriteString(string.Empty);
                base.WriteBoolean(true);
                base.WriteBoolean(false);
                base.WriteBoolean(false);//Pocket Habbo user.
                base.WriteShort(0);

                ServerPacket group = new ServerPacket(ServerPacketHeader.FriendListUpdateMessageComposer);
                group.WriteInteger(1);//Category Count
                group.WriteInteger(1);
                group.WriteString("Chat de Grupos");
                group.WriteInteger(1);//Updates Count
                group.WriteInteger(-1);//Update
                group.WriteInteger(gp.Id);
                Player.GetClient().SendMessage(group);
            }

            foreach (MessengerBuddy Friend in Friends.ToList())
            {
                Relationship Relationship = Player.Relationships.FirstOrDefault(x => x.Value.UserId == Convert.ToInt32(Friend.UserId)).Value;

                base.WriteInteger(Friend.Id);
                base.WriteString(Friend.mUsername);
                base.WriteInteger(1);//Gender.
                base.WriteBoolean(Friend.IsOnline);
                base.WriteBoolean(Friend.IsOnline && Friend.InRoom);
                base.WriteString(Friend.IsOnline ? Friend.mLook : string.Empty);
                base.WriteInteger(0); // category id
                base.WriteString(string.Empty);
                base.WriteString(string.Empty);//Alternative name?
                base.WriteString(string.Empty);
                base.WriteBoolean(true);
                base.WriteBoolean(false);
                base.WriteBoolean(false);//Pocket Habbo user.
                base.WriteShort(Relationship == null ? 0 : Relationship.Type);
            }

            #region Custom Chats
            if (Player.Rank >= 5)
            {
                base.WriteInteger(int.MinValue);
                base.WriteString("Administración");
                base.WriteInteger(1);
                base.WriteBoolean(true);
                base.WriteBoolean(false);
                base.WriteString("badgeADM");
                base.WriteInteger(1);
                base.WriteString(string.Empty);
                base.WriteString("Administración");
                base.WriteString(string.Empty);
                base.WriteBoolean(true);
                base.WriteBoolean(false);
                base.WriteBoolean(false);
                base.WriteShort(0);
            }

            if (Player._guidelevel >= 1)
            {
                base.WriteInteger(int.MinValue + 1);
                base.WriteString("Guías");
                base.WriteInteger(1);
                base.WriteBoolean(true);
                base.WriteBoolean(false);
                base.WriteString("badgeGUIAS");
                base.WriteInteger(1);
                base.WriteString(string.Empty);
                base.WriteString("Departamento de Soporte");
                base.WriteString(string.Empty);
                base.WriteBoolean(true);
                base.WriteBoolean(false);
                base.WriteBoolean(false);
                base.WriteShort(0);
            }
            #endregion

        }
    }
}
