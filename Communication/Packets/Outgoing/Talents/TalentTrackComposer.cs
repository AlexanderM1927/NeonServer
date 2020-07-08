using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Talents;
using System.Collections.Generic;

namespace Neon.Communication.Packets.Outgoing.Talents
{
    internal class TalentTrackComposer : ServerPacket
    {
        public TalentTrackComposer(ICollection<TalentTrackLevel> Levels, string Type, GameClient Session)
            : base(ServerPacketHeader.TalentTrackMessageComposer)
        {
            bool flag = false;
            Dictionary<string, int> achievementGroup = new Dictionary<string, int>();
            List<string> badge = new List<string>();
            achievementGroup.Add("ACH_AvatarLooks", 1);
            badge.Add("ACH_AvatarLooks1");
            achievementGroup.Add("ACH_RespectGiven", 1);
            badge.Add("ACH_RespectGiven1");
            achievementGroup.Add("ACH_AllTimeHotelPresence", 1);
            badge.Add("ACH_AllTimeHotelPresence1");
            achievementGroup.Add("ACH_RoomEntry", 1);
            badge.Add("ACH_RoomEntry1");
            if (NeonEnvironment.GetGame().GetAchievementManager().ContainsGroupAch(achievementGroup, Session) || Session.GetHabbo().GetBadgeComponent().HasBadgeList(badge))
            {
                flag = true;
            }
            bool flag2 = false;
            Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
            List<string> list2 = new List<string>();
            dictionary2.Add("ACH_GuideAdvertisementReader", 1);
            list2.Add("ACH_GuideAdvertisementReader1");
            dictionary2.Add("ACH_RegistrationDuration", 1);
            list2.Add("ACH_RegistrationDuration1");
            dictionary2.Add("ACH_AllTimeHotelPresence", 2);
            list2.Add("ACH_AllTimeHotelPresence2");
            dictionary2.Add("ACH_RoomEntry", 2);
            list2.Add("ACH_RoomEntry2");
            if (NeonEnvironment.GetGame().GetAchievementManager().ContainsGroupAch(dictionary2, Session) || Session.GetHabbo().GetBadgeComponent().HasBadgeList(list2))
            {
                flag2 = true;
            }
            bool flag3 = false;
            Dictionary<string, int> dictionary3 = new Dictionary<string, int>();
            List<string> list3 = new List<string>();
            dictionary3.Add("ACH_GuideAdvertisementReader", 1);
            list3.Add("ACH_GuideAdvertisementReader1");
            dictionary3.Add("ACH_RegistrationDuration", 2);
            list3.Add("ACH_RegistrationDuration2");
            dictionary3.Add("ACH_AllTimeHotelPresence", 3);
            list3.Add("ACH_AllTimeHotelPresence3");
            dictionary3.Add("ACH_RoomEntry", 3);
            list3.Add("ACH_RoomEntry3");
            if (NeonEnvironment.GetGame().GetAchievementManager().ContainsGroupAch(dictionary3, Session) || Session.GetHabbo().GetBadgeComponent().HasBadgeList(list3))
            {
                flag3 = true;
            }

            base.WriteString("citizenship");
            base.WriteInteger(5);
            base.WriteInteger(0);
            base.WriteInteger(Session.GetHabbo().PassedQuiz ? 2 : 1);
            base.WriteInteger(1);
            base.WriteInteger(0x7d);
            base.WriteInteger(1);
            base.WriteString("ACH_SafetyQuizGraduate1");
            base.WriteInteger(Session.GetHabbo().PassedQuiz ? 2 : 1);
            base.WriteInteger(Session.GetHabbo().PassedQuiz ? 1 : 0);
            base.WriteInteger(1);
            base.WriteInteger(0);
            base.WriteInteger(1);
            base.WriteString("A1 KUMIANKKA");
            base.WriteInteger(0);
            base.WriteInteger(1);
            base.WriteInteger(flag ? 2 : (Session.GetHabbo().PassedQuiz ? 1 : 0));
            base.WriteInteger(4);
            base.WriteInteger(6);
            base.WriteInteger(1);
            base.WriteString("ACH_AvatarLooks1");
            base.WriteInteger((NeonEnvironment.GetGame().GetAchievementManager().ContainsAchievement("ACH_AvatarLooks", 1, Session) || Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_AvatarLooks1")) ? 2 : (Session.GetHabbo().PassedQuiz ? 1 : 0));
            base.WriteInteger(0);
            base.WriteInteger(1);
            base.WriteInteger(0x12);
            base.WriteInteger(1);
            base.WriteString("ACH_RespectGiven1");
            base.WriteInteger((NeonEnvironment.GetGame().GetAchievementManager().ContainsAchievement("ACH_RespectGiven", 1, Session) || Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_RespectGiven1")) ? 2 : (Session.GetHabbo().PassedQuiz ? 1 : 0));
            base.WriteInteger(0);
            base.WriteInteger(2);
            base.WriteInteger(0x13);
            base.WriteInteger(1);
            base.WriteString("ACH_AllTimeHotelPresence1");
            base.WriteInteger((NeonEnvironment.GetGame().GetAchievementManager().ContainsAchievement("ACH_AllTimeHotelPresence", 1, Session) || Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_AllTimeHotelPresence1")) ? 2 : (Session.GetHabbo().PassedQuiz ? 1 : 0));
            base.WriteInteger(0);
            base.WriteInteger(30);
            base.WriteInteger(8);
            base.WriteInteger(1);
            base.WriteString("ACH_RoomEntry1");
            base.WriteInteger((NeonEnvironment.GetGame().GetAchievementManager().ContainsAchievement("ACH_RoomEntry", 1, Session) || Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_RoomEntry1")) ? 2 : (Session.GetHabbo().PassedQuiz ? 1 : 0));
            base.WriteInteger(0);
            base.WriteInteger(5);
            base.WriteInteger(0);
            base.WriteInteger(1);
            base.WriteString("A1 KUMIANKKA");
            base.WriteInteger(flag2 ? 2 : (flag ? 1 : 0));
            base.WriteInteger(2);
            base.WriteInteger(0);
            base.WriteInteger(4);
            base.WriteInteger(0x91);
            base.WriteInteger(1);
            base.WriteString("ACH_GuideAdvertisementReader1");
            base.WriteInteger((NeonEnvironment.GetGame().GetAchievementManager().ContainsAchievement("ACH_GuideAdvertisementReader", 1, Session) || (Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_GuideAdvertisementReader1") && flag)) ? 2 : (flag ? 1 : 0));
            base.WriteInteger(0);
            base.WriteInteger(1);
            base.WriteInteger(11);
            base.WriteInteger(1);
            base.WriteString("ACH_RegistrationDuration1");
            base.WriteInteger((NeonEnvironment.GetGame().GetAchievementManager().ContainsAchievement("ACH_RegistrationDuration", 1, Session) || (Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_RegistrationDuration1") && flag)) ? 2 : (flag ? 1 : 0));
            base.WriteInteger(0);
            base.WriteInteger(1);
            base.WriteInteger(0x13);
            base.WriteInteger(2);
            base.WriteString("ACH_AllTimeHotelPresence2");
            base.WriteInteger((NeonEnvironment.GetGame().GetAchievementManager().ContainsAchievement("ACH_AllTimeHotelPresence", 2, Session) || (Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_AllTimeHotelPresence2") && flag)) ? 2 : (flag ? 1 : 0));
            base.WriteInteger(0);
            base.WriteInteger(60);
            base.WriteInteger(8);
            base.WriteInteger(2);
            base.WriteString("ACH_RoomEntry2");
            base.WriteInteger((NeonEnvironment.GetGame().GetAchievementManager().ContainsAchievement("ACH_RoomEntry", 2, Session) || (Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_RoomEntry2") && flag)) ? 2 : (flag ? 1 : 0));
            base.WriteInteger(0);
            base.WriteInteger(20);
            base.WriteInteger(0);
            base.WriteInteger(1);
            base.WriteString("A1 KUMIANKKA");
            base.WriteInteger(flag3 ? 2 : (flag2 ? 1 : 0));
            base.WriteInteger(3);
            base.WriteInteger(0);
            base.WriteInteger(4);
            base.WriteInteger(11);
            base.WriteInteger(2);
            base.WriteString("ACH_RegistrationDuration2");
            base.WriteInteger((NeonEnvironment.GetGame().GetAchievementManager().ContainsAchievement("ACH_RegistrationDuration", 2, Session) || (Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_RegistrationDuration2") && flag2)) ? 2 : (flag2 ? 1 : 0));
            base.WriteInteger(0);
            base.WriteInteger(3);
            base.WriteInteger(0x5e);
            base.WriteInteger(1);
            base.WriteString("ACH_HabboWayGraduate1");
            base.WriteInteger((NeonEnvironment.GetGame().GetAchievementManager().ContainsAchievement("ACH_HabboWayGraduate", 1, Session) || (Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_HabboWayGraduate1") && flag2)) ? 2 : (flag2 ? 1 : 0));
            base.WriteInteger(0);
            base.WriteInteger(1);
            base.WriteInteger(0x13);
            base.WriteInteger(3);
            base.WriteString("ACH_AllTimeHotelPresence3");
            base.WriteInteger((NeonEnvironment.GetGame().GetAchievementManager().ContainsAchievement("ACH_AllTimeHotelPresence", 3, Session) || (Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_AllTimeHotelPresence3") && flag2)) ? 2 : (flag2 ? 1 : 0));
            base.WriteInteger(0);
            base.WriteInteger(120);
            base.WriteInteger(0x8e);
            base.WriteInteger(1);
            base.WriteString("ACH_FriendListSize1");
            base.WriteInteger((NeonEnvironment.GetGame().GetAchievementManager().ContainsAchievement("ACH_FriendListSize", 1, Session) || (Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_FriendListSize1") && flag2)) ? 2 : (flag2 ? 1 : 0));
            base.WriteInteger(0);
            base.WriteInteger(2);
            base.WriteInteger(1);
            base.WriteString("TRADE");
            base.WriteInteger(1);
            base.WriteString("A1 KUMIANKKA");
            base.WriteInteger(flag3 ? 1 : 0);
            base.WriteInteger(4);
            base.WriteInteger(0);
            base.WriteInteger(0);
            base.WriteInteger(1);
            base.WriteString("CITIZEN");
            base.WriteInteger(2);
            base.WriteString("A1 KUMIANKKA");
            base.WriteInteger(0);
            base.WriteString("HABBO_CLUB_CITIZENSHIP_VIP_REWARD");
            base.WriteInteger(7);

        }
    }
}