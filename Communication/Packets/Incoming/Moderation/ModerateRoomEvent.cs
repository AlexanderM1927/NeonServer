using Neon.Communication.Packets.Outgoing.Navigator;
using Neon.Communication.Packets.Outgoing.Rooms.Settings;
using Neon.Database.Interfaces;
using Neon.HabboHotel.Rooms;
using System.Linq;


namespace Neon.Communication.Packets.Incoming.Moderation
{
    internal class ModerateRoomEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
            {
                return;
            }

            if (!NeonEnvironment.GetGame().GetRoomManager().TryGetRoom(Packet.PopInt(), out Room Room))
            {
                return;
            }

            bool SetLock = Packet.PopInt() == 1;
            bool SetName = Packet.PopInt() == 1;
            bool KickAll = Packet.PopInt() == 1;

            if (SetName)
            {
                Room.RoomData.Name = "Sala inapropiada, no cumple la manera";
                Room.RoomData.Description = "Esta sala ha infringido la manera de Habbo hotel.";
            }

            if (SetLock)
            {
                Room.RoomData.Access = RoomAccess.DOORBELL;
            }

            if (Room.Tags.Count > 0)
            {
                Room.ClearTags();
            }

            if (Room.RoomData.HasActivePromotion)
            {
                Room.RoomData.EndPromotion();
            }

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                if (SetName && SetLock)
                {
                    dbClient.RunQuery("UPDATE `rooms` SET `caption` = 'Inappropriate to Hotel Management', `description` = 'Inappropriate to Hotel Management', `tags` = '', `state` = '1' WHERE `id` = '" + Room.RoomId + "' LIMIT 1");
                }
                else if (SetName && !SetLock)
                {
                    dbClient.RunQuery("UPDATE `rooms` SET `caption` = 'Inappropriate to Hotel Management', `description` = 'Inappropriate to Hotel Management', `tags` = '' WHERE `id` = '" + Room.RoomId + "' LIMIT 1");
                }
                else if (!SetName && SetLock)
                {
                    dbClient.RunQuery("UPDATE `rooms` SET `state` = '1', `tags` = '' WHERE `id` = '" + Room.RoomId + "' LIMIT 1");
                }
            }

            Room.SendMessage(new RoomSettingsSavedComposer(Room.RoomId));
            Room.SendMessage(new RoomInfoUpdatedComposer(Room.RoomId));

            if (KickAll)
            {
                foreach (RoomUser RoomUser in Room.GetRoomUserManager().GetUserList().ToList())
                {
                    if (RoomUser == null || RoomUser.IsBot)
                    {
                        continue;
                    }

                    if (RoomUser.GetClient() == null || RoomUser.GetClient().GetHabbo() == null)
                    {
                        continue;
                    }

                    if (RoomUser.GetClient().GetHabbo().Rank >= Session.GetHabbo().Rank || RoomUser.GetClient().GetHabbo().Id == Session.GetHabbo().Id)
                    {
                        continue;
                    }

                    Room.GetRoomUserManager().RemoveUserFromRoom(RoomUser.GetClient(), true, false);
                }
            }
        }
    }
}
