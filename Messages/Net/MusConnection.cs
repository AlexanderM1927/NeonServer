using log4net;
using Neon.Communication.Packets.Outgoing.Catalog;
using Neon.Communication.Packets.Outgoing.Inventory.Badges;
using Neon.Communication.Packets.Outgoing.Inventory.Purse;
using Neon.Communication.Packets.Outgoing.Moderation;
using Neon.Communication.Packets.Outgoing.Rooms.Engine;
using Neon.Communication.Packets.Outgoing.Rooms.Session;
using Neon.Core;
using Neon.Database.Interfaces;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Rooms;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Neon.Messages.Net
{

    public class MusConnection
    {
        private Socket _socket;
        private byte[] _buffer = new byte[1024];

        private static readonly ILog log = LogManager.GetLogger("Neon.Messages.Net.MusConnection");

        public MusConnection(Socket Socket)
        {
            _socket = Socket;

            try
            {
                _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, OnEvent_RecieveData, _socket);
            }
            catch { tryClose(); }
        }

        public void tryClose()
        {
            try
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
                _socket.Dispose();
            }
            catch
            {
            }

            _socket = null;
            _buffer = null;
        }

        public void OnEvent_RecieveData(IAsyncResult iAr)
        {
            try
            {
                int bytes = 0;

                try
                {
                    bytes = _socket.EndReceive(iAr);
                }
                catch
                {
                    tryClose();
                    return;
                }

                string data = Encoding.Default.GetString(_buffer, 0, bytes);

                if (data.Length > 0)
                {
                    processCommand(data);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            tryClose();
        }

        public void processCommand(string data)
        {
            GameClient Client = null;

            string header = data.Split()[0];
            string param = "";

            for (int i = 1; i < data.Split().Length; i++)
            {
                param += data.Split()[i];
                if (i < data.Split().Length - 1)
                {
                    param += " ";
                }
            }

            string[] Params = param.Split(':');

            switch (header.ToLower())
            {
                #region User Related
                #region :reload_credits <UserID>
                case "reload_credits":
                    {
                        int UserId = Convert.ToInt32(Params[0]);
                        Client = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
                        if (Client == null || Client.GetHabbo() == null)
                        {
                            break;
                        }

                        int Credits = 0;
                        using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("SELECT `credits` FROM `users` WHERE `id` = @id LIMIT 1");
                            dbClient.AddParameter("id", UserId);
                            Credits = dbClient.getInteger();
                        }

                        Client.GetHabbo().Credits = Credits;
                        Client.SendMessage(new CreditBalanceComposer(Client.GetHabbo().Credits));
                        break;
                    }
                #endregion
                #region :reload_pixels <UserID>
                case "reload_pixels":
                    {
                        int UserId = Convert.ToInt32(Params[0]);
                        Client = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
                        if (Client == null || Client.GetHabbo() == null)
                        {
                            break;
                        }

                        int Pixels = 0;
                        using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("SELECT `activity_points` FROM `users` WHERE `id` = @id LIMIT 1");
                            dbClient.AddParameter("id", UserId);
                            Pixels = dbClient.getInteger();
                        }

                        Client.GetHabbo().Duckets = Pixels;
                        Client.SendMessage(new HabboActivityPointNotificationComposer(Client.GetHabbo().Duckets, Pixels));
                        break;
                    }
                #endregion
                #region :reload_diamonds <UserID>
                case "reload_diamonds":
                    {
                        int UserId = Convert.ToInt32(Params[0]);
                        Client = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
                        if (Client == null || Client.GetHabbo() == null)
                        {
                            break;
                        }

                        int Diamonds = 0;
                        using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("SELECT `vip_points` FROM `users` WHERE `id` = @id LIMIT 1");
                            dbClient.AddParameter("id", UserId);
                            Diamonds = dbClient.getInteger();
                        }

                        Client.GetHabbo().Diamonds = Diamonds;
                        Client.SendMessage(new HabboActivityPointNotificationComposer(Diamonds, 0, 5));
                        break;
                    }
                #endregion
                #region :reload_gotw <UserID>
                case "reload_gotw":
                    {
                        int UserId = Convert.ToInt32(Params[0]);
                        Client = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
                        if (Client == null || Client.GetHabbo() == null)
                        {
                            break;
                        }

                        int GOTWPoints = 0;
                        using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("SELECT `gotw_points` FROM `users` WHERE `id` = @id LIMIT 1");
                            dbClient.AddParameter("id", UserId);
                            GOTWPoints = dbClient.getInteger();
                        }

                        Client.GetHabbo().GOTWPoints = GOTWPoints;
                        Client.SendMessage(new HabboActivityPointNotificationComposer(GOTWPoints, 0, 103));
                        break;
                    }
                #endregion
                #region :reload_user_rank userID
                case "reload_user_rank":
                    {
                        int UserId = Convert.ToInt32(Params[0]);

                        Client = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
                        if (Client == null || Client.GetHabbo() == null)
                        {
                            break;
                        }

                        using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("SELECT `rank` FROM `users` WHERE `id` = @userID LIMIT 1");
                            dbClient.AddParameter("userID", UserId);
                            Client.GetHabbo().Rank = dbClient.getInteger();
                        }
                        break;
                    }
                #endregion
                #region :reload_user_vip userID
                case "reload_user_vip":
                    {
                        int UserId = Convert.ToInt32(Params[0]);

                        Client = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
                        if (Client == null || Client.GetHabbo() == null)
                        {
                            break;
                        }

                        using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("SELECT `rank_vip` FROM `users` WHERE `id` = @userID LIMIT 1");
                            dbClient.AddParameter("userID", UserId);
                            Client.GetHabbo().VIPRank = dbClient.getInteger();
                        }
                        break;
                    }
                #endregion
                #region :reload_motto userID
                case "reload_motto":
                    {
                        int UserId = Convert.ToInt32(Params[0]);

                        Client = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
                        if (Client == null || Client.GetHabbo() == null)
                        {
                            break;
                        }

                        using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("SELECT `motto` FROM `users` WHERE `id` = @userID LIMIT 1");
                            dbClient.AddParameter("userID", UserId);
                            Client.GetHabbo().Motto = dbClient.getString();
                        }

                        if (Client.GetHabbo().InRoom)
                        {
                            Room Room = Client.GetHabbo().CurrentRoom;
                            if (Room == null)
                            {
                                return;
                            }

                            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Client.GetHabbo().Id);
                            if (User == null || User.GetClient() == null)
                            {
                                return;
                            }

                            Room.SendMessage(new UserChangeComposer(User, false));
                        }
                        break;
                    }
                #endregion
                #region :alert_user <userid> <message>
                case "alert":
                case "alert_user":
                    {
                        int UserId = Convert.ToInt32(Params[0]);
                        string alertMessage = Convert.ToString(Params[1]);

                        Client = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
                        if (Client == null || Client.GetHabbo() == null)
                        {
                            break;
                        }

                        Client.SendMessage(new BroadcastMessageAlertComposer(alertMessage));
                        break;
                    }
                #endregion
                #region :reload_badges <UserID>
                case "update_badges":
                case "reload_badges":
                    {
                        int UserId = Convert.ToInt32(Params[0]);
                        Client = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);

                        if (Client != null)
                        {
                            if (Client.GetHabbo() != null)
                            {
                                Client.SendMessage(new BadgesComposer(Client));
                            }
                        }
                        break;
                    }
                #endregion
                #region :givebadge <UserID> <badge>
                case "givebadge":
                    {
                        int UserId = Convert.ToInt32(Params[0]);
                        string badgeCode = Convert.ToString(Params[1]);
                        Client = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);

                        if (Client != null)
                        {
                            if (Client.GetHabbo() != null)
                            {
                                Client.GetHabbo().GetBadgeComponent().GiveBadge(badgeCode, true, Client);
                            }
                        }
                        break;
                    }
                #endregion
                #region :disconnect <username>
                case "disconnect":
                    {
                        try
                        {
                            GameClient TargetClient = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(Convert.ToInt32(Params[0]));
                            if (TargetClient != null && TargetClient.GetConnection() != null)
                            {
                                TargetClient.GetConnection().Dispose();
                            }
                        }
                        catch
                        {
                            log.Error("Error disconnecting user using MUS");
                        }
                        return;
                    }
                #endregion
                #region :reload_last_change userID
                case "reload_last_change":
                    {
                        int UserId = Convert.ToInt32(Params[0]);

                        Client = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
                        if (Client == null || Client.GetHabbo() == null)
                        {
                            break;
                        }

                        using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("SELECT `last_change` FROM `users` WHERE `id` = @userID LIMIT 1");
                            dbClient.AddParameter("userID", UserId);
                            Client.GetHabbo().LastNameChange = dbClient.getInteger();
                        }
                        break;
                    }
                #endregion
                #region :goto <UserID> <RoomID>
                case "goto":
                    {
                        int UserId = Convert.ToInt32(Params[0]);
                        int RoomId = Convert.ToInt32(Params[1]);

                        Client = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
                        if (Client == null || Client.GetHabbo() == null)
                        {
                            break;
                        }

                        if (!int.TryParse(Params[1], out RoomId))
                        {
                            break;
                        }
                        else
                        {
                            Room _room = NeonEnvironment.GetGame().GetRoomManager().LoadRoom(RoomId);
                            if (_room == null)
                            {
                                Client.SendNotification("Failed to find the requested room!");
                            }
                            else
                            {
                                if (!Client.GetHabbo().InRoom)
                                {
                                    Client.SendMessage(new RoomForwardComposer(_room.Id));
                                }
                                else
                                {
                                    Client.GetHabbo().PrepareRoom(_room.Id, "");
                                }
                            }
                        }
                    }
                    break;
                #endregion
                #endregion

                #region Fastfood
                #region :progress_achievement
                case "progress_achievement":
                    {
                        int UserId = Convert.ToInt32(Params[0]);
                        Client = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
                        if (Client == null || Client.GetHabbo() == null)
                        {
                            break;
                        }

                        string Achievement = Convert.ToString(Params[1]);
                        int Progress = Convert.ToInt32(Params[2]);

                        NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Client, Achievement, Progress);
                        break;
                    }
                #endregion
                #endregion

                #region Settings related
                #region :reload_filter/:update_filter
                case "update_filter":
                case "reload_filter":
                case "recache_filter":
                case "refresh_filter":
                    {
                        NeonEnvironment.GetGame().GetChatManager().GetFilter().InitWords();
                        NeonEnvironment.GetGame().GetChatManager().GetFilter().InitCharacters();
                        break;
                    }
                #endregion
                #region :reload_catalog/:reload_catalog
                case "update_catalog":
                case "reload_catalog":
                case "recache_catalog":
                case "refresh_catalog":
                case "update_catalogue":
                case "reload_catalogue":
                case "recache_catalogue":
                case "refresh_catalogue":
                    {
                        NeonEnvironment.GetGame().GetCatalog().Init(NeonEnvironment.GetGame().GetItemManager());
                        NeonEnvironment.GetGame().GetClientManager().SendMessage(new CatalogUpdatedComposer());
                        break;
                    }
                #endregion
                #region :reload_items/:update_items
                case "update_items":
                case "reload_items":
                case "recache_items":
                case "refresh_items":
                    {
                        NeonEnvironment.GetGame().GetItemManager().Init();
                        break;
                    }
                #endregion
                #region :reload_navigator/:update_navigator
                case "update_navigator":
                case "reload_navigator":
                case "recache_navigator":
                case "refresh_navigator":
                    {
                        NeonEnvironment.GetGame().GetNavigator().Init();
                        break;
                    }
                #endregion
                #region :reload_ranks/:update_ranks
                case "update_ranks":
                case "reload_ranks":
                case "recache_ranks":
                case "refresh_ranks":
                    {
                        NeonEnvironment.GetGame().GetPermissionManager().Init();

                        foreach (GameClient C in NeonEnvironment.GetGame().GetClientManager().GetClients.ToList())
                        {
                            if (C == null || C.GetHabbo() == null || C.GetHabbo().GetPermissions() == null)
                            {
                                continue;
                            }

                            C.GetHabbo().GetPermissions().Init(Client.GetHabbo());
                        }
                        break;
                    }
                #endregion
                #region :reload_settings/:update_settings
                case "update_settings":
                case "reload_settings":
                case "recache_settings":
                case "refresh_settings":
                    {
                        NeonEnvironment.ConfigData = new ConfigData();
                        break;
                    }
                #endregion
                #region :reload_quests/:update_quests
                case "reload_quests":
                case "update_quests":
                    {
                        NeonEnvironment.GetGame().GetQuestManager().Init();
                        break;
                    }
                #endregion
                #region :reload_vouchers/:update_vouchers
                case "reload_vouchers":
                case "update_vouchers":
                    {
                        NeonEnvironment.GetGame().GetCatalog().GetVoucherManager().Init();
                        break;
                    }
                #endregion
                #region :reload_bans/:update_bans
                case "update_bans":
                case "reload_bans":
                    {
                        NeonEnvironment.GetGame().GetModerationManager().ReCacheBans();
                        break;
                    }
                #endregion
                #endregion


                //#region Camera related

                //    #region :add_preview <photo_id> <user_id> <created_at>
                //    case "add_preview":
                //    {
                //        int PhotoId = Convert.ToInt32(Params[0]);
                //        int UserId = Convert.ToInt32(Params[1]);
                //        long CreatedAt = Convert.ToInt64(Params[2]);

                //        Client = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);

                //                    if (Client == null || Client.GetHabbo() == null || Client.GetHabbo().CurrentRoomId < 1)
                //                           break;

                //        NeonEnvironment.GetGame().GetCameraManager().AddPreview(new CameraPhotoPreview(PhotoId, UserId, CreatedAt));
                //                    break;
                //    }

                //    #endregion

                //    #endregion

                default:
                    {
                        log.Error("Unrecognized MUS packet: '" + header + "'");
                        return;
                    }
            }

            log.Info("Successfully Parsed MUS command: '" + header + "'");
        }
    }
}