﻿using log4net;
using Neon.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Neon.HabboHotel.Navigator
{
    public sealed class NavigatorManager
    {
        private static readonly ILog log = LogManager.GetLogger("Neon.HabboHotel.Navigator.NavigatorManager");

        private readonly Dictionary<int, FeaturedRoom> _featuredRooms;
        private readonly Dictionary<int, StaffPick> _staffPicks;

        private readonly Dictionary<int, TopLevelItem> _topLevelItems;
        private readonly Dictionary<int, SearchResultList> _searchResultLists;

        public NavigatorManager()
        {
            _topLevelItems = new Dictionary<int, TopLevelItem>();
            _searchResultLists = new Dictionary<int, SearchResultList>();
            _staffPicks = new Dictionary<int, StaffPick>();

            //Does this need to be dynamic?
            _topLevelItems.Add(1, new TopLevelItem(1, "official_view", "", ""));
            _topLevelItems.Add(2, new TopLevelItem(2, "hotel_view", "", ""));
            _topLevelItems.Add(3, new TopLevelItem(3, "roomads_view", "", ""));
            _topLevelItems.Add(4, new TopLevelItem(4, "myworld_view", "", ""));

            _featuredRooms = new Dictionary<int, FeaturedRoom>();

            Init();
        }

        public void Init()
        {
            #region oldnav
            /*
            if (this._searchResultLists.Count > 0)
                this._searchResultLists.Clear();

            if (this._featuredRooms.Count > 0)
                this._featuredRooms.Clear();

            DataTable Table = null;
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `navigator_categories` ORDER BY `id` ASC");
                Table = dbClient.getTable();

                if (Table != null)
                {
                    foreach (DataRow Row in Table.Rows)
                    {
                        if (Convert.ToInt32(Row["enabled"]) == 1)
                        {
                            if (!this._searchResultLists.ContainsKey(Convert.ToInt32(Row["id"])))
                                this._searchResultLists.Add(Convert.ToInt32(Row["id"]), new SearchResultList(Convert.ToInt32(Row["id"]), Convert.ToString(Row["category"]), Convert.ToString(Row["category_identifier"]), Convert.ToString(Row["public_name"]), true, -1, Convert.ToInt32(Row["required_rank"]), NavigatorViewModeUtility.GetViewModeByString(Convert.ToString(Row["view_mode"])), Convert.ToString(Row["category_type"]), Convert.ToString(Row["search_allowance"]), Convert.ToInt32(Row["order_id"])));
                        }
                    }
                }

                dbClient.SetQuery("SELECT `room_id`,`caption`,`description`,`image_url`,`enabled` FROM `navigator_publics` ORDER BY `order_num` ASC");
                DataTable GetPublics = dbClient.getTable();

                if (GetPublics != null)
                {
                    foreach (DataRow Row in GetPublics.Rows)
                    {
                        if (Convert.ToInt32(Row["enabled"]) == 1)
                        {
                            if (!this._featuredRooms.ContainsKey(Convert.ToInt32(Row["room_id"])))
                                this._featuredRooms.Add(Convert.ToInt32(Row["room_id"]), new FeaturedRoom(Convert.ToInt32(Row["room_id"]), Convert.ToString(Row["caption"]), Convert.ToString(Row["description"]), Convert.ToString(Row["image_url"])));
                        }
                    }
                }
            }

            log.Info(">> Navigator -> Ready!");
            */
            #endregion
            if (_searchResultLists.Count > 0)
            {
                _searchResultLists.Clear();
            }

            if (_featuredRooms.Count > 0)
            {
                _featuredRooms.Clear();
            }

            if (_staffPicks.Count > 0)
            {
                _staffPicks.Clear();
            }

            DataTable Table = null;
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `navigator_categories` ORDER BY `id` ASC");
                Table = dbClient.getTable();

                if (Table != null)
                {
                    foreach (DataRow Row in Table.Rows)
                    {
                        if (Convert.ToInt32(Row["enabled"]) == 1)
                        {
                            if (!_searchResultLists.ContainsKey(Convert.ToInt32(Row["id"])))
                            {
                                _searchResultLists.Add(Convert.ToInt32(Row["id"]), new SearchResultList(Convert.ToInt32(Row["id"]), Convert.ToString(Row["category"]), Convert.ToString(Row["category_identifier"]), Convert.ToString(Row["public_name"]), true, -1, Convert.ToInt32(Row["required_rank"]), NavigatorViewModeUtility.GetViewModeByString(Convert.ToString(Row["view_mode"])), Convert.ToString(Row["category_type"]), Convert.ToString(Row["search_allowance"]), Convert.ToInt32(Row["order_id"])));
                            }
                        }
                    }
                }

                dbClient.SetQuery("SELECT `id`,`room_id`,`caption`,`description`,`image_url`,`enabled`,`category_id` FROM `navigator_publics` ORDER BY `order_num` ASC");
                DataTable GetPublics = dbClient.getTable();

                if (GetPublics != null)
                {
                    foreach (DataRow Row in GetPublics.Rows)
                    {
                        if (Convert.ToInt32(Row["enabled"]) == 1)
                        {
                            if (!_featuredRooms.ContainsKey(Convert.ToInt32(Row["id"])))
                            {
                                _featuredRooms.Add(Convert.ToInt32(Row["id"]), new FeaturedRoom(Convert.ToInt32(Row["id"]), Convert.ToInt32(Row["room_id"]), Convert.ToString(Row["caption"]), Convert.ToString(Row["description"]), Convert.ToString(Row["image_url"]), Convert.ToInt32(Row["category_id"])));
                            }
                        }
                    }
                }

                dbClient.SetQuery("SELECT `room_id`,`image` FROM `navigator_staff_picks`");
                DataTable GetPicks = dbClient.getTable();

                if (GetPicks != null)
                {
                    foreach (DataRow Row in GetPicks.Rows)
                    {
                        if (!_staffPicks.ContainsKey(Convert.ToInt32(Row["room_id"])))
                        {
                            _staffPicks.Add(Convert.ToInt32(Row["room_id"]), new StaffPick(Convert.ToInt32(Row["room_id"]), Convert.ToString(Row["image"])));
                        }
                    }
                }
            }

            log.Info("» Navigator Manager -> READY!");
        }

        public List<SearchResultList> GetCategorysForSearch(string Category)
        {
            IEnumerable<SearchResultList> Categorys =
                (from Cat in _searchResultLists
                 where Cat.Value.Category == Category
                 orderby Cat.Value.OrderId ascending
                 select Cat.Value);
            return Categorys.ToList();
        }

        public ICollection<SearchResultList> GetResultByIdentifier(string Category)
        {
            IEnumerable<SearchResultList> Categorys =
                (from Cat in _searchResultLists
                 where Cat.Value.CategoryIdentifier == Category
                 orderby Cat.Value.OrderId ascending
                 select Cat.Value);
            return Categorys.ToList();
        }

        public ICollection<SearchResultList> GetFlatCategories()
        {
            IEnumerable<SearchResultList> Categorys =
                (from Cat in _searchResultLists
                 where Cat.Value.CategoryType == NavigatorCategoryType.CATEGORY
                 orderby Cat.Value.OrderId ascending
                 select Cat.Value);
            return Categorys.ToList();
        }

        public ICollection<SearchResultList> GetEventCategories()
        {
            IEnumerable<SearchResultList> Categorys =
                (from Cat in _searchResultLists
                 where Cat.Value.CategoryType == NavigatorCategoryType.PROMOTION_CATEGORY
                 orderby Cat.Value.OrderId ascending
                 select Cat.Value);
            return Categorys.ToList();
        }

        public ICollection<TopLevelItem> GetTopLevelItems()
        {
            return _topLevelItems.Values;
        }

        public ICollection<SearchResultList> GetSearchResultLists()
        {
            return _searchResultLists.Values;
        }

        public bool TryGetTopLevelItem(int Id, out TopLevelItem TopLevelItem)
        {
            return _topLevelItems.TryGetValue(Id, out TopLevelItem);
        }

        public bool TryGetSearchResultList(int Id, out SearchResultList SearchResultList)
        {
            return _searchResultLists.TryGetValue(Id, out SearchResultList);
        }

        public bool TryAddStaffPickedRoom(int roomId)
        {
            if (_staffPicks.ContainsKey(roomId))
            {
                return false;
            }

            _staffPicks.Add(roomId, new StaffPick(roomId, ""));
            return true;
        }

        public bool TryRemoveStaffPickedRoom(int roomId)
        {
            if (!_staffPicks.ContainsKey(roomId))
            {
                return false;
            }

            return _staffPicks.Remove(roomId);
        }

        public bool TryGetFeaturedRoom(int RoomId, out FeaturedRoom PublicRoom)
        {
            return _featuredRooms.TryGetValue(RoomId, out PublicRoom);
        }


        public bool TryGetStaffPickedRoom(int roomId, out StaffPick room)
        {
            return _staffPicks.TryGetValue(roomId, out room);
        }

        public ICollection<FeaturedRoom> GetFeaturedRooms(int CategoryId)
        {
            List<FeaturedRoom> FeaturedRooms = new List<FeaturedRoom>();
            foreach (KeyValuePair<int, FeaturedRoom> featuredRoom in _featuredRooms)
            {
                if (featuredRoom.Value.CategoryId == CategoryId)
                {
                    FeaturedRooms.Add(featuredRoom.Value);
                }
            }
            return FeaturedRooms;
        }

        public ICollection<StaffPick> GetStaffPicks()
        {
            return _staffPicks.Values;
        }
    }
}
