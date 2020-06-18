#region

using System;
using System.Data;
using System.Text;
using Neon.Communication.Packets.Outgoing.GameCenter;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Games;

#endregion

namespace Neon.Communication.Packets.Incoming.GameCenter
{
    internal class JoinPlayerQueueEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if ((Session == null) || (Session.GetHabbo() == null))
                return;

            var GameId = Packet.PopInt();

            GameData GameData = null;
            if (NeonEnvironment.GetGame().GetGameDataManager().TryGetGame(GameId, out GameData))
            {
                Session.SendMessage(new JoinQueueComposer(GameData.GameId));
                var HabboID = Session.GetHabbo().Id;
                using (var dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    DataTable data;
                    dbClient.SetQuery("SELECT user_id FROM user_auth_ticket WHERE user_id = '" + HabboID + "'");
                    data = dbClient.getTable();
                    var count = 0;
                    foreach (DataRow row in data.Rows)
                    {
                        if (Convert.ToInt32(row["userid"]) == HabboID)
                            count++;
                    }
                    if (count == 0)
                    {
                        var SSOTicket = "Fastfood-" + GenerateSSO(32) + "-" + Session.GetHabbo().Id;
                        dbClient.RunQuery("INSERT INTO user_tickets(userid, sessionticket) VALUES ('" + HabboID +
                                          "', '" +
                                          SSOTicket + "')");
                        Session.SendMessage(new LoadGameComposer(GameData, SSOTicket));
                    }
                    else
                    {
                        dbClient.SetQuery("SELECT user_id,sessionticket FROM user_tickets WHERE userid = " + HabboID);
                        data = dbClient.getTable();
                        foreach (DataRow dRow in data.Rows)
                        {
                            var SSOTicket = dRow["sessionticket"];
                            Session.SendMessage(new LoadGameComposer(GameData, (string)SSOTicket));
                        }
                    }
                }
            }
        }

        private string GenerateSSO(int length)
        {
            var random = new Random();
            var characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var result = new StringBuilder(length);
            for (var i = 0; i < length; i++)
                result.Append(characters[random.Next(characters.Length)]);
            return result.ToString();
        }
    }
}