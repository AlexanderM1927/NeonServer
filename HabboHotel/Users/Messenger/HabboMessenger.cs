﻿using Neon.Communication.Packets.Outgoing;
using Neon.Communication.Packets.Outgoing.Messenger;
using Neon.Database.Interfaces;
using Neon.HabboHotel.Cache;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Quests;
using Neon.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Neon.HabboHotel.Users.Messenger
{
    public class HabboMessenger
    {
        public bool AppearOffline;
        private readonly int _userId;

        private Dictionary<int, MessengerBuddy> _friends;
        private Dictionary<int, MessengerRequest> _requests;

        public HabboMessenger(int UserId)
        {
            _userId = UserId;

            _requests = new Dictionary<int, MessengerRequest>();
            _friends = new Dictionary<int, MessengerBuddy>();
        }


        public void Init(Dictionary<int, MessengerBuddy> friends, Dictionary<int, MessengerRequest> requests)
        {
            _requests = new Dictionary<int, MessengerRequest>(requests);
            _friends = new Dictionary<int, MessengerBuddy>(friends);
        }

        public bool TryGetRequest(int senderID, out MessengerRequest Request)
        {
            return _requests.TryGetValue(senderID, out Request);
        }

        public bool TryGetFriend(int UserId, out MessengerBuddy Buddy)
        {
            return _friends.TryGetValue(UserId, out Buddy);
        }

        public void ProcessOfflineMessages()
        {
            DataTable GetMessages = null;
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `messenger_offline_messages` WHERE `to_id` = @id;");
                dbClient.AddParameter("id", _userId);
                GetMessages = dbClient.getTable();

                if (GetMessages != null)
                {
                    GameClient Client = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(_userId);
                    if (Client == null)
                    {
                        return;
                    }

                    foreach (DataRow Row in GetMessages.Rows)
                    {
                        Client.SendMessage(new NewConsoleMessageComposer(Convert.ToInt32(Row["from_id"]), Convert.ToString(Row["message"]), (int)(UnixTimestamp.GetNow() - Convert.ToInt32(Row["timestamp"]))));
                    }

                    dbClient.SetQuery("DELETE FROM `messenger_offline_messages` WHERE `to_id` = @id");
                    dbClient.AddParameter("id", _userId);
                    dbClient.RunQuery();
                }
            }
        }

        public void Destroy()
        {
            IEnumerable<GameClient> onlineUsers = NeonEnvironment.GetGame().GetClientManager().GetClientsById(_friends.Keys);

            foreach (GameClient client in onlineUsers)
            {
                if (client.GetHabbo() == null || client.GetHabbo().GetMessenger() == null)
                {
                    continue;
                }

                client.GetHabbo().GetMessenger().UpdateFriend(_userId, null, true);
            }
        }

        public void OnStatusChanged(bool notification)
        {
            if (GetClient() == null || GetClient().GetHabbo() == null || GetClient().GetHabbo().GetMessenger() == null)
            {
                return;
            }

            if (_friends == null)
            {
                return;
            }

            IEnumerable<GameClient> onlineUsers = NeonEnvironment.GetGame().GetClientManager().GetClientsById(_friends.Keys);
            if (onlineUsers.Count() == 0)
            {
                return;
            }

            foreach (GameClient client in onlineUsers.ToList())
            {
                try
                {
                    if (client == null || client.GetHabbo() == null || client.GetHabbo().GetMessenger() == null)
                    {
                        continue;
                    }

                    client.GetHabbo().GetMessenger().UpdateFriend(_userId, client, true);

                    if (this == null || client == null || client.GetHabbo() == null)
                    {
                        continue;
                    }

                    UpdateFriend(client.GetHabbo().Id, client, notification);
                }
                catch
                {
                    continue;
                }
            }
        }

        public void UpdateFriend(int userid, GameClient client, bool notification)
        {
            if (_friends.ContainsKey(userid))
            {
                _friends[userid].UpdateUser(client);

                if (notification)
                {
                    GameClient Userclient = GetClient();
                    if (Userclient != null)
                    {
                        Userclient.SendMessage(SerializeUpdate(_friends[userid]));
                    }
                }
            }
        }

        public void HandleAllRequests()
        {
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("DELETE FROM messenger_requests WHERE from_id = " + _userId + " OR to_id = " + _userId);
            }

            ClearRequests();
        }

        public void HandleRequest(int sender)
        {
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("DELETE FROM messenger_requests WHERE (from_id = " + _userId + " AND to_id = " + sender + ") OR (to_id = " + _userId + " AND from_id = " + sender + ")");
            }

            _requests.Remove(sender);
        }

        public void CreateFriendship(int friendID)
        {
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("REPLACE INTO messenger_friendships (user_one_id,user_two_id) VALUES (" + _userId + "," + friendID + ")");
            }

            OnNewFriendship(friendID);

            GameClient User = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(friendID);

            if (User != null && User.GetHabbo().GetMessenger() != null)
            {
                User.GetHabbo().GetMessenger().OnNewFriendship(_userId);
            }

            if (User != null)
            {
                NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(User, "ACH_FriendListSize", 1);
            }

            GameClient thisUser = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(_userId);
            if (thisUser != null)
            {
                NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(thisUser, "ACH_FriendListSize", 1);
            }
        }

        public void DestroyFriendship(int friendID)
        {
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("DELETE FROM messenger_friendships WHERE (user_one_id = " + _userId + " AND user_two_id = " + friendID + ") OR (user_two_id = " + _userId + " AND user_one_id = " + friendID + ")");

            }

            OnDestroyFriendship(friendID);

            GameClient User = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(friendID);

            if (User != null && User.GetHabbo().GetMessenger() != null)
            {
                User.GetHabbo().GetMessenger().OnDestroyFriendship(_userId);
            }
        }

        public void OnNewFriendship(int friendID)
        {
            GameClient friend = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(friendID);

            MessengerBuddy newFriend;
            if (friend == null || friend.GetHabbo() == null)
            {
                DataRow dRow;
                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT id,username,motto,look,last_online,hide_inroom,hide_online FROM users WHERE `id` = @friendid LIMIT 1");
                    dbClient.AddParameter("friendid", friendID);
                    dRow = dbClient.getRow();
                }

                newFriend = new MessengerBuddy(friendID, Convert.ToString(dRow["username"]), Convert.ToString(dRow["look"]), Convert.ToString(dRow["motto"]), Convert.ToInt32(dRow["last_online"]),
                    NeonEnvironment.EnumToBool(dRow["hide_online"].ToString()), NeonEnvironment.EnumToBool(dRow["hide_inroom"].ToString()));
            }
            else
            {
                Habbo user = friend.GetHabbo();


                newFriend = new MessengerBuddy(friendID, user.Username, user.Look, user.Motto, 0, user.AppearOffline, user.AllowPublicRoomStatus);
                newFriend.UpdateUser(friend);
            }

            if (!_friends.ContainsKey(friendID))
            {
                _friends.Add(friendID, newFriend);
            }

            GetClient().SendMessage(SerializeUpdate(newFriend));
        }

        public bool RequestExists(int requestID)
        {
            if (_requests.ContainsKey(requestID))
            {
                return true;
            }

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery(
                    "SELECT user_one_id FROM messenger_friendships WHERE user_one_id = @myID AND user_two_id = @friendID");
                dbClient.AddParameter("myID", Convert.ToInt32(_userId));
                dbClient.AddParameter("friendID", Convert.ToInt32(requestID));
                return dbClient.findsResult();
            }
        }

        public bool FriendshipExists(int friendID)
        {
            return _friends.ContainsKey(friendID);
        }

        public void OnDestroyFriendship(int Friend)
        {
            if (_friends.ContainsKey(Friend))
            {
                _friends.Remove(Friend);
            }

            GetClient().SendMessage(new FriendListUpdateComposer(Friend));
        }

        public bool RequestBuddy(string UserQuery)
        {
            int userID;
            bool hasFQDisabled;

            GameClient client = NeonEnvironment.GetGame().GetClientManager().GetClientByUsername(UserQuery);
            if (client == null)
            {
                DataRow Row = null;
                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT `id`,`block_newfriends` FROM `users` WHERE `username` = @query LIMIT 1");
                    dbClient.AddParameter("query", UserQuery.ToLower());
                    Row = dbClient.getRow();
                }

                if (Row == null)
                {
                    return false;
                }

                userID = Convert.ToInt32(Row["id"]);
                hasFQDisabled = NeonEnvironment.EnumToBool(Row["block_newfriends"].ToString());
            }
            else
            {
                userID = client.GetHabbo().Id;
                hasFQDisabled = client.GetHabbo().AllowFriendRequests;
            }

            if (hasFQDisabled)
            {
                GetClient().SendMessage(new MessengerErrorComposer(39, 3));
                return false;
            }

            int ToId = userID;
            if (RequestExists(ToId))
            {
                return true;
            }

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("REPLACE INTO `messenger_requests` (`from_id`,`to_id`) VALUES ('" + _userId + "','" + ToId + "')");
            }

            NeonEnvironment.GetGame().GetQuestManager().ProgressUserQuest(GetClient(), QuestType.ADD_FRIENDS);

            GameClient ToUser = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(ToId);
            if (ToUser == null || ToUser.GetHabbo() == null)
            {
                return true;
            }

            MessengerRequest Request = new MessengerRequest(ToId, _userId, NeonEnvironment.GetGame().GetClientManager().GetNameById(_userId));

            ToUser.GetHabbo().GetMessenger().OnNewRequest(_userId);

            UserCache ThisUser = NeonEnvironment.GetGame().GetCacheManager().GenerateUser(_userId);

            if (ThisUser != null)
            {
                ToUser.SendMessage(new NewBuddyRequestComposer(ThisUser));
            }

            _requests.Add(ToId, Request);
            return true;
        }

        public void OnNewRequest(int friendID)
        {
            if (!_requests.ContainsKey(friendID))
            {
                _requests.Add(friendID, new MessengerRequest(_userId, friendID, NeonEnvironment.GetGame().GetClientManager().GetNameById(friendID)));
            }
        }

        public void SendInstantMessage(int ToId, string Message)
        {
            if (ToId == 0)
            {
                return;
            }

            if (GetClient() == null)
            {
                return;
            }

            if (GetClient().GetHabbo() == null)
            {
                return;
            }

            #region Custom Chats
            List<Groups.Group> Group = NeonEnvironment.GetGame().GetGroupManager().GetGroupsForUser(GetClient().GetHabbo().Id).Where(c => c.HasChat).ToList();
            foreach (Groups.Group gp in Group)
            {
                if (ToId == int.MinValue + gp.Id) // int.MaxValue
                {
                    //NeonEnvironment.GetGame().GetClientManager().SendMessaget(new FuckingConsoleMessageComposer(ToId, Message, GetClient().GetHabbo().Username + "/" + GetClient().GetHabbo().Look + "/" + GetClient().GetHabbo().Id), GetClient().GetHabbo().Id);
                    NeonEnvironment.GetGame().GetClientManager().GroupChatAlert(new FuckingConsoleMessageComposer(int.MinValue + gp.Id, Message, GetClient().GetHabbo().Username + "/" + GetClient().GetHabbo().Look + "/" + GetClient().GetHabbo().Id), gp, GetClient().GetHabbo().Id);
                    return;
                }

            }
            if (GetClient().GetHabbo().Rank >= 5 && ToId == int.MinValue) // int.MaxValue
            {
                NeonEnvironment.GetGame().GetClientManager().StaffAlert(new FuckingConsoleMessageComposer(ToId, Message, GetClient().GetHabbo().Username + "/" + GetClient().GetHabbo().Look + "/" + GetClient().GetHabbo().Id), GetClient().GetHabbo().Id);
                return;
            }
            else if (GetClient().GetHabbo()._guidelevel >= 1 && ToId == (int.MinValue + 1))
            {
                NeonEnvironment.GetGame().GetClientManager().GuideAlert(new FuckingConsoleMessageComposer(ToId, Message, GetClient().GetHabbo().Username + "/" + GetClient().GetHabbo().Look + "/" + GetClient().GetHabbo().Id), GetClient().GetHabbo().Id);
                return;
            }
            #endregion

            if (!FriendshipExists(ToId))
            {
                GetClient().SendMessage(new InstantMessageErrorComposer(MessengerMessageErrors.YOUR_NOT_FRIENDS, ToId));
                return;
            }

            if (GetClient().GetHabbo().MessengerSpamCount >= 12)
            {
                GetClient().GetHabbo().MessengerSpamTime = NeonEnvironment.GetUnixTimestamp() + 60;
                GetClient().GetHabbo().MessengerSpamCount = 0;
                GetClient().SendNotification("You cannot send a message, you have flooded the console.\n\nYou can send a message in 60 seconds.");
                return;
            }
            else if (GetClient().GetHabbo().MessengerSpamTime > NeonEnvironment.GetUnixTimestamp())
            {
                double Time = GetClient().GetHabbo().MessengerSpamTime - NeonEnvironment.GetUnixTimestamp();
                GetClient().SendNotification("You cannot send a message, you have flooded the console.\n\nYou can send a message in " + Time + " seconds.");
                return;
            }


            GetClient().GetHabbo().MessengerSpamCount++;

            GameClient Client = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(ToId);
            if (Client == null || Client.GetHabbo() == null || Client.GetHabbo().GetMessenger() == null)
            {
                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("INSERT INTO `messenger_offline_messages` (`to_id`, `from_id`, `message`, `timestamp`) VALUES (@tid, @fid, @msg, UNIX_TIMESTAMP())");
                    dbClient.AddParameter("tid", ToId);
                    dbClient.AddParameter("fid", GetClient().GetHabbo().Id);
                    dbClient.AddParameter("msg", Message);
                    dbClient.RunQuery();
                }
                return;
            }

            if (!Client.GetHabbo().AllowConsoleMessages || Client.GetHabbo().MutedUsers.Contains(GetClient().GetHabbo().Id))
            {
                GetClient().SendMessage(new InstantMessageErrorComposer(MessengerMessageErrors.FRIEND_BUSY, ToId));
                return;
            }

            if (GetClient().GetHabbo().TimeMuted > 0)
            {
                GetClient().SendMessage(new InstantMessageErrorComposer(MessengerMessageErrors.YOUR_MUTED, ToId));
                return;
            }

            if (Client.GetHabbo().TimeMuted > 0)
            {
                GetClient().SendMessage(new InstantMessageErrorComposer(MessengerMessageErrors.FRIEND_MUTED, ToId));
            }

            if (string.IsNullOrEmpty(Message))
            {
                return;
            }

            Client.SendMessage(new NewConsoleMessageComposer(_userId, Message));
            LogPM(_userId, ToId, Message);
        }

        public void LogPM(int From_Id, int ToId, string Message)
        {
            int MyId = GetClient().GetHabbo().Id;
            DateTime Now = DateTime.Now;
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO chatlogs_console VALUES (NULL, " + From_Id + ", " + ToId + ", @message, UNIX_TIMESTAMP())");
                dbClient.AddParameter("message", Message);
                dbClient.RunQuery();
            }
        }

        public ServerPacket SerializeUpdate(MessengerBuddy friend)
        {
            ServerPacket Packet = new ServerPacket(ServerPacketHeader.FriendListUpdateMessageComposer);
            Packet.WriteInteger(1);//Category Count
            Packet.WriteInteger(1);
            Packet.WriteString("Chats de grupo");

            Packet.WriteInteger(1); // number of updates
            Packet.WriteInteger(0); // don't know

            friend.Serialize(Packet, GetClient());
            return Packet;
        }

        public void BroadcastAchievement(int UserId, MessengerEventTypes Type, string Data)
        {
            IEnumerable<GameClient> MyFriends = NeonEnvironment.GetGame().GetClientManager().GetClientsById(_friends.Keys);

            foreach (GameClient Client in MyFriends.ToList())
            {
                if (Client.GetHabbo() != null && Client.GetHabbo().GetMessenger() != null)
                {
                    Client.SendMessage(new FriendNotificationComposer(UserId, Type, Data));
                    Client.GetHabbo().GetMessenger().OnStatusChanged(true);
                }
            }
        }

        public void ClearRequests()
        {
            _requests.Clear();
        }

        private GameClient GetClient()
        {
            return NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(_userId);
        }

        public ICollection<MessengerRequest> GetRequests()
        {
            return _requests.Values;
        }

        public ICollection<MessengerBuddy> GetFriends()
        {
            return _friends.Values;
        }
    }
}