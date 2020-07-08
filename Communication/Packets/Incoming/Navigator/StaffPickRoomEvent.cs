using Neon.Communication.Packets.Outgoing.Navigator;
using Neon.Communication.Packets.Outgoing.Rooms.Settings;
using Neon.Database.Interfaces;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Navigator;
using Neon.HabboHotel.Rooms;

namespace Neon.Communication.Packets.Incoming.Navigator
{
    internal class StaffPickRoomEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            GameClient TargetClient = NeonEnvironment.GetGame().GetClientManager().GetClientByUsername(session.GetHabbo().CurrentRoom.OwnerName);

            if (!session.GetHabbo().GetPermissions().HasRight("room.staff_picks.management"))
            {
                return;
            }

            if (!NeonEnvironment.GetGame().GetRoomManager().TryGetRoom(packet.PopInt(), out Room room))
            {
                return;
            }

            if (!NeonEnvironment.GetGame().GetNavigator().TryGetStaffPickedRoom(room.Id, out StaffPick staffPick))
            {
                if (NeonEnvironment.GetGame().GetNavigator().TryAddStaffPickedRoom(room.Id))
                {
                    using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("INSERT INTO `navigator_staff_picks` (`room_id`,`image`) VALUES (@roomId, null)");
                        dbClient.AddParameter("roomId", room.Id);
                        dbClient.RunQuery();
                    }
                    if (TargetClient != null)
                    {
                        NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(TargetClient, "ACH_Spr", 1, false);
                    }
                }
            }

            else
            {
                if (NeonEnvironment.GetGame().GetNavigator().TryRemoveStaffPickedRoom(room.Id))
                {
                    using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("DELETE FROM `navigator_staff_picks` WHERE `room_id` = @roomId LIMIT 1");
                        dbClient.AddParameter("roomId", room.Id);
                        dbClient.RunQuery();
                    }
                }
            }

            room.SendMessage(new RoomSettingsSavedComposer(room.RoomId));
            room.SendMessage(new RoomInfoUpdatedComposer(room.RoomId));
        }
    }
}