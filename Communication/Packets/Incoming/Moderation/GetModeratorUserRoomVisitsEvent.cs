using Neon.Communication.Packets.Outgoing.Moderation;
using Neon.Database.Interfaces;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Rooms;
using System;
using System.Collections.Generic;
using System.Data;


namespace Neon.Communication.Packets.Incoming.Moderation
{
    internal class GetModeratorUserRoomVisitsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
            {
                return;
            }

            int UserId = Packet.PopInt();
            GameClient Target = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
            if (Target == null)
            {
                return;
            }

            DataTable Table = null;
            Dictionary<double, RoomData> Visits = new Dictionary<double, RoomData>();
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT room_id, `entry_timestamp` FROM `user_roomvisits` WHERE `user_id` =@id ORDER BY `entry_timestamp` DESC LIMIT 50");
                dbClient.AddParameter("id", UserId);
                Table = dbClient.getTable();

                if (Table != null)
                {
                    foreach (DataRow Row in Table.Rows)
                    {
                        RoomData RData = NeonEnvironment.GetGame().GetRoomManager().GenerateRoomData(Convert.ToInt32(Row["room_id"]));
                        if (RData == null)
                        {
                            return;
                        }

                        if (!Visits.ContainsKey(Convert.ToDouble(Row["entry_timestamp"])))
                        {
                            Visits.Add(Convert.ToDouble(Row["entry_timestamp"]), RData);
                        }
                    }
                }
            }

            Session.SendMessage(new ModeratorUserRoomVisitsComposer(Target.GetHabbo(), Visits));
        }
    }
}