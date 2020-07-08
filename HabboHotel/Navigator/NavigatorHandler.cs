using Neon.Communication.Packets.Outgoing;
using Neon.Database.Interfaces;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Groups;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Users.Messenger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Neon.HabboHotel.Navigator
{
    internal static class NavigatorHandler
    {
        public static void Search(ServerPacket Message, SearchResultList SearchResult, string SearchData,
           GameClient Session, int FetchLimit)
        {
            //Switching by categorys.
            switch (SearchResult.CategoryType)
            {
                default:
                    Message.WriteInteger(0);
                    break;

                case NavigatorCategoryType.QUERY:
                    {
                        #region Query

                        if (SearchData.ToLower().StartsWith("owner:"))
                        {
                            if (SearchData.Length > 0)
                            {
                                //  var UserId = 0;
                                DataTable GetRooms = null;
                                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                                {
                                    if (SearchData.ToLower().StartsWith("owner:"))
                                    {
                                        dbClient.SetQuery(
                                            "SELECT r.* FROM rooms r, users u WHERE u.username = @username AND r.owner = u.id AND r.state != 'invisible' ORDER BY r.users_now DESC LIMIT 50;");
                                        dbClient.AddParameter("username", SearchData.Remove(0, 6));
                                        GetRooms = dbClient.getTable();
                                    }
                                }

                                List<RoomData> Results = new List<RoomData>();
                                if (GetRooms != null)
                                {
                                    foreach (
                                        RoomData RoomData in GetRooms.Rows.Cast<DataRow>().Select(Row => NeonEnvironment.GetGame()
                                                .GetRoomManager()
                                                .FetchRoomData(Convert.ToInt32(Row["id"]), Row))
                                            .Where(RoomData => RoomData != null && !Results.Contains(RoomData)))
                                    {
                                        Results.Add(RoomData);
                                    }
                                }

                                Message.WriteInteger(Results.Count);
                                foreach (RoomData Data in Results.ToList())
                                {
                                    RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                                }
                            }
                        }
                        else if (SearchData.ToLower().StartsWith("tag:"))
                        {
                            SearchData = SearchData.Remove(0, 4);
                            ICollection<RoomData> TagMatches =
                                NeonEnvironment.GetGame().GetRoomManager().SearchTaggedRooms(SearchData);

                            Message.WriteInteger(TagMatches.Count);
                            foreach (RoomData Data in TagMatches.ToList())
                            {
                                RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                            }
                        }
                        else if (SearchData.ToLower().StartsWith("group:"))
                        {
                            SearchData = SearchData.Remove(0, 6);
                            ICollection<RoomData> GroupRooms =
                                NeonEnvironment.GetGame().GetRoomManager().SearchGroupRooms(SearchData);

                            Message.WriteInteger(GroupRooms.Count);
                            foreach (RoomData Data in GroupRooms.ToList())
                            {
                                RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                            }
                        }
                        else
                        {
                            if (SearchData.Length > 0)
                            {
                                DataTable Table;
                                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                                {
                                    dbClient.SetQuery(
                                        "SELECT `id`,`caption`,`description`,`roomtype`,`owner`,`state`,`category`,`users_now`,`users_max`,`model_name`,`score`,`allow_pets`,`allow_pets_eat`,`room_blocking_disabled`,`allow_hidewall`,`password`,`wallpaper`,`floor`,`landscape`,`floorthick`,`wallthick`,`mute_settings`,`kick_settings`,`ban_settings`,`chat_mode`,`chat_speed`,`chat_size`,`trade_settings`,`group_id`,`tags` FROM rooms WHERE `caption` LIKE @query ORDER BY `users_now` DESC LIMIT 50");
                                    if (SearchData.ToLower().StartsWith("roomname:"))
                                    {
                                        dbClient.AddParameter("query", "%" + SearchData.Split(new[] { ':' }, 2)[1] + "%");
                                    }
                                    else
                                    {
                                        dbClient.AddParameter("query", "%" + SearchData + "%");
                                    }

                                    Table = dbClient.getTable();
                                }

                                List<RoomData> Results = new List<RoomData>();
                                if (Table != null)
                                {
                                    foreach (RoomData RData in from DataRow Row in Table.Rows
                                                               where Convert.ToString(Row["state"]) != "invisible"
                                                               select NeonEnvironment.GetGame()
                                                                   .GetRoomManager()
                                                                   .FetchRoomData(Convert.ToInt32(Row["id"]), Row)
                                        into RData
                                                               where RData != null && !Results.Contains(RData)
                                                               select RData)
                                    {
                                        Results.Add(RData);
                                    }
                                }

                                Message.WriteInteger(Results.Count);
                                foreach (RoomData Data in Results.ToList())
                                {
                                    RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                                }
                            }
                        }

                        #endregion

                        break;
                    }

                case NavigatorCategoryType.FEATURED:
                    {
                        #region Featured
                        List<RoomData> Rooms = new List<RoomData>();
                        ICollection<FeaturedRoom> Featured = NeonEnvironment.GetGame().GetNavigator().GetFeaturedRooms(SearchResult.Id);
                        foreach (FeaturedRoom FeaturedItem in Featured.ToList())
                        {
                            if (FeaturedItem == null)
                            {
                                continue;
                            }

                            RoomData Data = NeonEnvironment.GetGame().GetRoomManager().GenerateRoomData(FeaturedItem.RoomId);
                            if (Data == null)
                            {
                                continue;
                            }

                            if (!Rooms.Contains(Data))
                            {
                                Rooms.Add(Data);
                            }
                        }
                        Message.WriteInteger(Rooms.Count);
                        foreach (RoomData Data in Rooms.ToList())
                        {
                            RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                        }
                        #endregion
                        break;
                    }

                case NavigatorCategoryType.STAFF_PICKS:
                    {
                        #region Featured
                        List<RoomData> rooms = new List<RoomData>();
                        ICollection<StaffPick> picks = NeonEnvironment.GetGame().GetNavigator().GetStaffPicks();
                        foreach (StaffPick pick in picks.ToList())
                        {
                            if (pick == null)
                            {
                                continue;
                            }

                            RoomData Data = NeonEnvironment.GetGame().GetRoomManager().GenerateRoomData(pick.RoomId);
                            if (Data == null)
                            {
                                continue;
                            }

                            if (!rooms.Contains(Data))
                            {
                                rooms.Add(Data);
                            }
                        }
                        Message.WriteInteger(rooms.Count);
                        foreach (RoomData data in rooms.ToList())
                        {
                            RoomAppender.WriteRoom(Message, data, data.Promotion);
                        }
                        #endregion
                        break;
                    }

                case NavigatorCategoryType.POPULAR:
                    {
                        List<RoomData> PopularRooms = NeonEnvironment.GetGame().GetRoomManager().GetPopularRooms(-1, FetchLimit);

                        Message.WriteInteger(PopularRooms.Count);
                        foreach (RoomData Data in PopularRooms.ToList())
                        {
                            RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                        }
                        break;
                    }

                case NavigatorCategoryType.RECOMMENDED:
                    {
                        List<RoomData> RecommendedRooms = NeonEnvironment.GetGame().GetRoomManager().GetRecommendedRooms(FetchLimit);

                        Message.WriteInteger(RecommendedRooms.Count);
                        foreach (RoomData Data in RecommendedRooms.ToList())
                        {
                            RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                        }
                        break;
                    }

                case NavigatorCategoryType.CATEGORY:
                    {
                        List<RoomData> GetRoomsByCategory = NeonEnvironment.GetGame().GetRoomManager().GetRoomsByCategory(SearchResult.Id, FetchLimit);

                        Message.WriteInteger(GetRoomsByCategory.Count);
                        foreach (RoomData Data in GetRoomsByCategory.ToList())
                        {
                            RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                        }
                        break;
                    }

                case NavigatorCategoryType.MY_ROOMS:

                    Message.WriteInteger(Session.GetHabbo().UsersRooms.Count);
                    foreach (RoomData Data in Session.GetHabbo().UsersRooms.ToList())
                    {
                        RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                    }
                    break;

                case NavigatorCategoryType.MY_FAVORITES:
                    List<RoomData> Favourites = new List<RoomData>();
                    foreach (int Id in Session.GetHabbo().FavoriteRooms.ToArray())
                    {
                        RoomData Room = NeonEnvironment.GetGame().GetRoomManager().GenerateRoomData(Id);
                        if (Room == null)
                        {
                            continue;
                        }

                        if (!Favourites.Contains(Room))
                        {
                            Favourites.Add(Room);
                        }
                    }

                    Favourites = Favourites.Take(FetchLimit).ToList();

                    Message.WriteInteger(Favourites.Count);
                    foreach (RoomData Data in Favourites.ToList())
                    {
                        RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                    }
                    break;

                case NavigatorCategoryType.MY_GROUPS:
                    List<RoomData> MyGroups = new List<RoomData>();

                    foreach (Group Group in NeonEnvironment.GetGame().GetGroupManager().GetGroupsForUser(Session.GetHabbo().Id).ToList())
                    {
                        if (Group == null)
                        {
                            continue;
                        }

                        RoomData Data = NeonEnvironment.GetGame().GetRoomManager().GenerateRoomData(Group.RoomId);
                        if (Data == null)
                        {
                            continue;
                        }

                        if (!MyGroups.Contains(Data))
                        {
                            MyGroups.Add(Data);
                        }
                    }

                    MyGroups = MyGroups.Take(FetchLimit).ToList();

                    Message.WriteInteger(MyGroups.Count);
                    foreach (RoomData Data in MyGroups.ToList())
                    {
                        RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                    }
                    break;

                case NavigatorCategoryType.MY_FRIENDS_ROOMS:
                    List<RoomData> MyFriendsRooms = new List<RoomData>();
                    foreach (MessengerBuddy buddy in Session.GetHabbo().GetMessenger().GetFriends().Where(p => p.InRoom))
                    {
                        if (buddy == null || !buddy.InRoom || buddy.UserId == Session.GetHabbo().Id)
                        {
                            continue;
                        }

                        if (!MyFriendsRooms.Contains(buddy.CurrentRoom.RoomData))
                        {
                            MyFriendsRooms.Add(buddy.CurrentRoom.RoomData);
                        }
                    }

                    Message.WriteInteger(MyFriendsRooms.Count);
                    foreach (RoomData Data in MyFriendsRooms.ToList())
                    {
                        RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                    }
                    break;

                case NavigatorCategoryType.MY_RIGHTS:
                    List<RoomData> MyRights = new List<RoomData>();

                    DataTable GetRights = null;
                    using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("SELECT `room_id` FROM `room_rights` WHERE `user_id` = @UserId LIMIT @FetchLimit");
                        dbClient.AddParameter("UserId", Session.GetHabbo().Id);
                        dbClient.AddParameter("FetchLimit", FetchLimit);
                        GetRights = dbClient.getTable();

                        foreach (DataRow Row in GetRights.Rows)
                        {
                            RoomData Data = NeonEnvironment.GetGame().GetRoomManager().GenerateRoomData(Convert.ToInt32(Row["room_id"]));
                            if (Data == null)
                            {
                                continue;
                            }

                            if (!MyRights.Contains(Data))
                            {
                                MyRights.Add(Data);
                            }
                        }
                    }

                    Message.WriteInteger(MyRights.Count);
                    foreach (RoomData Data in MyRights.ToList())
                    {
                        RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                    }
                    break;

                case NavigatorCategoryType.TOP_PROMOTIONS:
                    {
                        List<RoomData> GetPopularPromotions = NeonEnvironment.GetGame().GetRoomManager().GetOnGoingRoomPromotions(16, FetchLimit);

                        Message.WriteInteger(GetPopularPromotions.Count);
                        foreach (RoomData Data in GetPopularPromotions.ToList())
                        {
                            RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                        }
                        break;
                    }

                case NavigatorCategoryType.PROMOTION_CATEGORY:
                    {
                        List<RoomData> GetPromotedRooms = NeonEnvironment.GetGame().GetRoomManager().GetPromotedRooms(SearchResult.Id, FetchLimit);

                        Message.WriteInteger(GetPromotedRooms.Count);
                        foreach (RoomData Data in GetPromotedRooms.ToList())
                        {
                            RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                        }
                        break;
                    }
            }
        }
    }
}