﻿
using Neon.Communication.Packets.Outgoing.Pets;
using Neon.Communication.Packets.Outgoing.Rooms.Avatar;
using Neon.HabboHotel.Rooms;

namespace Neon.Communication.Packets.Incoming.Rooms.AI.Pets
{
    internal class RespectPetEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().InRoom || Session.GetHabbo().GetStats() == null || Session.GetHabbo().GetStats().DailyPetRespectPoints == 0)
            {
                return;
            }


            if (!NeonEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room Room))
            {
                return;
            }

            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (ThisUser == null)
            {
                return;
            }

            int PetId = Packet.PopInt();

            if (!Session.GetHabbo().CurrentRoom.GetRoomUserManager().TryGetPet(PetId, out RoomUser Pet))
            {
                RoomUser TargetUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(PetId);
                if (TargetUser == null)
                {
                    return;
                }

                if (TargetUser.GetClient() == null || TargetUser.GetClient().GetHabbo() == null)
                {
                    return;
                }

                if (TargetUser.GetClient().GetHabbo().Id == Session.GetHabbo().Id)
                {
                    Session.SendWhisper("A ver vale que quieras Duckets, pero respetarte a tí mismo ya es pasarse, ¿no?", 34);
                    return;
                }

                NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_RespectGiven", 1);
                NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(TargetUser.GetClient(), "ACH_RespectEarned", 1);

                Session.GetHabbo().GetStats().DailyPetRespectPoints -= 1;
                Session.GetHabbo().GetStats().RespectGiven += 1;
                TargetUser.GetClient().GetHabbo().GetStats().Respect += 1;

                ThisUser.CarryItemID = 999999999;
                ThisUser.CarryTimer = 5;

                if (Room.RespectNotificationsEnabled)
                {
                    Room.SendMessage(new RespectPetNotificationMessageComposer(TargetUser.GetClient().GetHabbo(), TargetUser));
                }

                Room.SendMessage(new CarryObjectComposer(ThisUser.VirtualId, ThisUser.CarryItemID));
                return;
            }

            if (Pet == null || Pet.PetData == null || Pet.RoomId != Session.GetHabbo().CurrentRoomId)
            {
                return;
            }

            Session.GetHabbo().GetStats().DailyPetRespectPoints -= 1;
            NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_PetRespectGiver", 1, false);

            ThisUser.CarryItemID = 999999999;
            ThisUser.CarryTimer = 5;
            Pet.PetData.OnRespect();
            Room.SendMessage(new CarryObjectComposer(ThisUser.VirtualId, ThisUser.CarryItemID));

            if (NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(Pet.PetData.OwnerId) != null && Session.GetHabbo().Id != Pet.PetData.OwnerId)
            {
                NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(Pet.PetData.OwnerId), "ACH_PetRespectReceiver", 1);
            }
        }
    }
}