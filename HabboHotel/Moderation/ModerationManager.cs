﻿using log4net;
using Neon.Database.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Neon.HabboHotel.Moderation
{
    public sealed class ModerationManager
    {
        private static readonly ILog log = LogManager.GetLogger("Neon.HabboHotel.Moderation.ModerationManager");

        private int _ticketCount = 1;
        private readonly List<string> _userPresets = new List<string>();
        private readonly List<string> _roomPresets = new List<string>();
        private readonly Dictionary<string, ModerationBan> _bans = new Dictionary<string, ModerationBan>();
        private readonly Dictionary<int, string> _userActionPresetCategories = new Dictionary<int, string>();
        private readonly Dictionary<int, List<ModerationPresetActionMessages>> _userActionPresetMessages = new Dictionary<int, List<ModerationPresetActionMessages>>();
        private readonly ConcurrentDictionary<int, ModerationTicket> _modTickets = new ConcurrentDictionary<int, ModerationTicket>();


        private readonly Dictionary<int, string> _moderationCFHTopics = new Dictionary<int, string>();
        private readonly Dictionary<int, List<ModerationPresetActions>> _moderationCFHTopicActions = new Dictionary<int, List<ModerationPresetActions>>();

        public ModerationManager()
        {

        }

        public void Init()
        {
            if (_userPresets.Count > 0)
            {
                _userPresets.Clear();
            }

            if (_moderationCFHTopics.Count > 0)
            {
                _moderationCFHTopics.Clear();
            }

            if (_moderationCFHTopicActions.Count > 0)
            {
                _moderationCFHTopicActions.Clear();
            }

            if (_bans.Count > 0)
            {
                _bans.Clear();
            }

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                DataTable PresetsTable = null;
                dbClient.SetQuery("SELECT * FROM `moderation_presets`;");
                PresetsTable = dbClient.getTable();

                if (PresetsTable != null)
                {
                    foreach (DataRow Row in PresetsTable.Rows)
                    {
                        string Type = Convert.ToString(Row["type"]).ToLower();
                        switch (Type)
                        {
                            case "user":
                                _userPresets.Add(Convert.ToString(Row["message"]));
                                break;

                            case "room":
                                _roomPresets.Add(Convert.ToString(Row["message"]));
                                break;
                        }
                    }
                }
            }

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                DataTable ModerationTopics = null;
                dbClient.SetQuery("SELECT * FROM `moderation_topics`;");
                ModerationTopics = dbClient.getTable();

                if (ModerationTopics != null)
                {
                    foreach (DataRow Row in ModerationTopics.Rows)
                    {
                        if (!_moderationCFHTopics.ContainsKey(Convert.ToInt32(Row["id"])))
                        {
                            _moderationCFHTopics.Add(Convert.ToInt32(Row["id"]), Convert.ToString(Row["caption"]));
                        }
                    }
                }
            }

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                DataTable ModerationTopicsActions = null;
                dbClient.SetQuery("SELECT * FROM `moderation_topic_actions`;");
                ModerationTopicsActions = dbClient.getTable();

                if (ModerationTopicsActions != null)
                {
                    foreach (DataRow Row in ModerationTopicsActions.Rows)
                    {
                        int ParentId = Convert.ToInt32(Row["parent_id"]);

                        if (!_moderationCFHTopicActions.ContainsKey(ParentId))
                        {
                            _moderationCFHTopicActions.Add(ParentId, new List<ModerationPresetActions>());
                        }

                        _moderationCFHTopicActions[ParentId].Add(new ModerationPresetActions(Convert.ToInt32(Row["id"]), Convert.ToInt32(Row["parent_id"]), Convert.ToString(Row["type"]), Convert.ToString(Row["caption"]), Convert.ToString(Row["message_text"]),
                            Convert.ToInt32(Row["mute_time"]), Convert.ToInt32(Row["ban_time"]), Convert.ToInt32(Row["ip_time"]), Convert.ToInt32(Row["trade_lock_time"]), Convert.ToString(Row["default_sanction"])));
                    }
                }
            }

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                DataTable PresetsActionCats = null;
                dbClient.SetQuery("SELECT * FROM `moderation_preset_action_categories`;");
                PresetsActionCats = dbClient.getTable();

                if (PresetsActionCats != null)
                {
                    foreach (DataRow Row in PresetsActionCats.Rows)
                    {
                        _userActionPresetCategories.Add(Convert.ToInt32(Row["id"]), Convert.ToString(Row["caption"]));
                    }
                }
            }

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                DataTable PresetsActionMessages = null;
                dbClient.SetQuery("SELECT * FROM `moderation_preset_action_messages`;");
                PresetsActionMessages = dbClient.getTable();

                if (PresetsActionMessages != null)
                {
                    foreach (DataRow Row in PresetsActionMessages.Rows)
                    {
                        int ParentId = Convert.ToInt32(Row["parent_id"]);

                        if (!_userActionPresetMessages.ContainsKey(ParentId))
                        {
                            _userActionPresetMessages.Add(ParentId, new List<ModerationPresetActionMessages>());
                        }

                        _userActionPresetMessages[ParentId].Add(new ModerationPresetActionMessages(Convert.ToInt32(Row["id"]), Convert.ToInt32(Row["parent_id"]), Convert.ToString(Row["caption"]), Convert.ToString(Row["message_text"]),
                            Convert.ToInt32(Row["mute_hours"]), Convert.ToInt32(Row["ban_hours"]), Convert.ToInt32(Row["ip_ban_hours"]), Convert.ToInt32(Row["trade_lock_days"]), Convert.ToString(Row["notice"])));
                    }
                }
            }

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                DataTable GetBans = null;
                dbClient.SetQuery("SELECT `bantype`,`value`,`reason`,`expire` FROM `bans` WHERE `bantype` = 'machine' OR `bantype` = 'user'");
                GetBans = dbClient.getTable();

                if (GetBans != null)
                {
                    foreach (DataRow dRow in GetBans.Rows)
                    {
                        string value = Convert.ToString(dRow["value"]);
                        string reason = Convert.ToString(dRow["reason"]);
                        double expires = (double)dRow["expire"];
                        string type = Convert.ToString(dRow["bantype"]);

                        ModerationBan Ban = new ModerationBan(BanTypeUtility.GetModerationBanType(type), value, reason, expires);
                        if (Ban != null)
                        {
                            if (expires > NeonEnvironment.GetUnixTimestamp())
                            {
                                if (!_bans.ContainsKey(value))
                                {
                                    _bans.Add(value, Ban);
                                }
                            }
                            else
                            {
                                dbClient.SetQuery("DELETE FROM `bans` WHERE `bantype` = '" + BanTypeUtility.FromModerationBanType(Ban.Type) + "' AND `value` = @Key LIMIT 1");
                                dbClient.AddParameter("Key", value);
                                dbClient.RunQuery();
                            }
                        }
                    }
                }
            }

            /*log.Info("Loaded " + (this._userPresets.Count + this._roomPresets.Count) + " moderation presets.");
            log.Info("Loaded " + this._userActionPresetCategories.Count + " moderation categories.");
            log.Info("Loaded " + this._userActionPresetMessages.Count + " moderation action preset messages.");
            log.Info("Cached " + this._bans.Count + " username and machine bans.");*/
            log.Info(">> Moderation Manager -> READY!");
        }

        public void ReCacheBans()
        {
            if (_bans.Count > 0)
            {
                _bans.Clear();
            }

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                DataTable GetBans = null;
                dbClient.SetQuery("SELECT `bantype`,`value`,`reason`,`expire` FROM `bans` WHERE `bantype` = 'machine' OR `bantype` = 'user'");
                GetBans = dbClient.getTable();

                if (GetBans != null)
                {
                    foreach (DataRow dRow in GetBans.Rows)
                    {
                        string value = Convert.ToString(dRow["value"]);
                        string reason = Convert.ToString(dRow["reason"]);
                        double expires = (double)dRow["expire"];
                        string type = Convert.ToString(dRow["bantype"]);

                        ModerationBan Ban = new ModerationBan(BanTypeUtility.GetModerationBanType(type), value, reason, expires);
                        if (Ban != null)
                        {
                            if (expires > NeonEnvironment.GetUnixTimestamp())
                            {
                                if (!_bans.ContainsKey(value))
                                {
                                    _bans.Add(value, Ban);
                                }
                            }
                            else
                            {
                                dbClient.SetQuery("DELETE FROM `bans` WHERE `bantype` = '" + BanTypeUtility.FromModerationBanType(Ban.Type) + "' AND `value` = @Key LIMIT 1");
                                dbClient.AddParameter("Key", value);
                                dbClient.RunQuery();
                            }
                        }
                    }
                }
            }

            //log.Info("Cached " + this._bans.Count + " username and machine bans.");
            log.Info(">> Ban Manager -> READY!");
        }

        public void BanUser(string Mod, ModerationBanType Type, string BanValue, string Reason, double ExpireTimestamp)
        {
            string BanType = (Type == ModerationBanType.IP ? "ip" : Type == ModerationBanType.MACHINE ? "machine" : "user");
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("REPLACE INTO `bans` (`bantype`, `value`, `reason`, `expire`, `added_by`,`added_date`) VALUES ('" + BanType + "', '" + BanValue + "', @reason, " + ExpireTimestamp + ", '" + Mod + "', '" + NeonEnvironment.GetUnixTimestamp() + "');");
                dbClient.AddParameter("reason", Reason);
                dbClient.RunQuery();
            }

            if (Type == ModerationBanType.MACHINE || Type == ModerationBanType.USERNAME)
            {
                if (!_bans.ContainsKey(BanValue))
                {
                    _bans.Add(BanValue, new ModerationBan(Type, BanValue, Reason, ExpireTimestamp));
                }
            }
        }

        public ICollection<string> UserMessagePresets => _userPresets;

        public ICollection<string> RoomMessagePresets => _roomPresets;

        public ICollection<ModerationTicket> GetTickets => _modTickets.Values;

        public Dictionary<string, List<ModerationPresetActions>> UserActionPresets
        {
            get
            {
                Dictionary<string, List<ModerationPresetActions>> Result = new Dictionary<string, List<ModerationPresetActions>>();
                foreach (KeyValuePair<int, string> Category in _moderationCFHTopics.ToList())
                {
                    Result.Add(Category.Value, new List<ModerationPresetActions>());

                    if (_moderationCFHTopicActions.ContainsKey(Category.Key))
                    {
                        foreach (ModerationPresetActions Data in _moderationCFHTopicActions[Category.Key])
                        {
                            Result[Category.Value].Add(Data);
                        }
                    }
                }
                return Result;
            }
        }

        public bool TryAddTicket(ModerationTicket Ticket)
        {
            Ticket.Id = _ticketCount++;
            return _modTickets.TryAdd(Ticket.Id, Ticket);
        }

        public bool TryGetTicket(int TicketId, out ModerationTicket Ticket)
        {
            return _modTickets.TryGetValue(TicketId, out Ticket);
        }

        public bool UserHasTickets(int userId)
        {
            return _modTickets.Count(x => x.Value.Sender.Id == userId && x.Value.Answered == false) > 0;
        }

        public ModerationTicket GetTicketBySenderId(int userId)
        {
            return _modTickets.FirstOrDefault(x => x.Value.Sender.Id == userId).Value;
        }

        /// <summary>
        /// Runs a quick check to see if a ban record is cached in the server.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Ban"></param>
        /// <returns></returns>
        public bool IsBanned(string Key, out ModerationBan Ban)
        {
            if (_bans.TryGetValue(Key, out Ban))
            {
                if (!Ban.Expired)
                {
                    return true;
                }

                //This ban has expired, let us quickly remove it here.
                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("DELETE FROM `bans` WHERE `bantype` = '" + BanTypeUtility.FromModerationBanType(Ban.Type) + "' AND `value` = @Key LIMIT 1");
                    dbClient.AddParameter("Key", Key);
                    dbClient.RunQuery();
                }

                //And finally, let us remove the ban record from the cache.
                if (_bans.ContainsKey(Key))
                {
                    _bans.Remove(Key);
                }

                return false;
            }
            return false;
        }

        /// <summary>
        /// Run a quick database check to see if this ban exists in the database.
        /// </summary>
        /// <param name="MachineId">The value of the ban.</param>
        /// <returns></returns>
        public bool MachineBanCheck(string MachineId)
        {
            if (NeonEnvironment.GetGame().GetModerationManager().IsBanned(MachineId, out ModerationBan MachineBanRecord))
            {
                DataRow BanRow = null;
                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT * FROM `bans` WHERE `bantype` = 'machine' AND `value` = @value LIMIT 1");
                    dbClient.AddParameter("value", MachineId);
                    BanRow = dbClient.getRow();

                    //If there is no more ban record, then we can simply remove it from our cache!
                    if (BanRow == null)
                    {
                        NeonEnvironment.GetGame().GetModerationManager().RemoveBan(MachineId);
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Run a quick database check to see if this ban exists in the database.
        /// </summary>
        /// <param name="Username">The value of the ban.</param>
        /// <returns></returns>
        public bool UsernameBanCheck(string Username)
        {
            if (NeonEnvironment.GetGame().GetModerationManager().IsBanned(Username, out ModerationBan UsernameBanRecord))
            {
                DataRow BanRow = null;
                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT * FROM `bans` WHERE `bantype` = 'user' AND `value` = @value LIMIT 1");
                    dbClient.AddParameter("value", Username);
                    BanRow = dbClient.getRow();

                    //If there is no more ban record, then we can simply remove it from our cache!
                    if (BanRow == null)
                    {
                        NeonEnvironment.GetGame().GetModerationManager().RemoveBan(Username);
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Remove a ban from the cache based on a given value.
        /// </summary>
        /// <param name="Value"></param>
        public void RemoveBan(string Value)
        {
            _bans.Remove(Value);
        }
    }
}
