﻿using ConnectionManager;
using Fleck;
using log4net;
using Neon.Communication.Packets.Outgoing;
using Neon.Communication.Packets.Outgoing.Handshake;
using Neon.Communication.Packets.Outgoing.Notifications;
using Neon.Core;
using Neon.Database.Interfaces;
using Neon.HabboHotel.Groups;
using Neon.HabboHotel.Items;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Users.Messenger;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Neon.HabboHotel.GameClients
{
    public class GameClientManager
    {
        private static readonly ILog log = LogManager.GetLogger("Neon.HabboHotel.GameClients.GameClientManager");

        public ConcurrentDictionary<int, GameClient> _clients;

        private readonly Dictionary<int, GameClient> guides;
        private readonly Dictionary<int, GameClient> alphas;

        private readonly ConcurrentDictionary<int, GameClient> _userIDRegister;
        private readonly ConcurrentDictionary<string, GameClient> _usernameRegister;

        private readonly ConcurrentDictionary<Guid, Session> _sessionRegister;

        private readonly Queue timedOutConnections;

        private readonly Stopwatch clientPingStopwatch;

        public GameClientManager()
        {
            _clients = new ConcurrentDictionary<int, GameClient>();
            _userIDRegister = new ConcurrentDictionary<int, GameClient>();
            _usernameRegister = new ConcurrentDictionary<string, GameClient>();

            _sessionRegister = new ConcurrentDictionary<Guid, Session>();

            guides = new Dictionary<int, GameClient>();
            alphas = new Dictionary<int, GameClient>();
            timedOutConnections = new Queue();

            clientPingStopwatch = new Stopwatch();
            clientPingStopwatch.Start();
        }

        public void OnCycle()
        {
            TestClientConnections();
            HandleTimeouts();
        }

        public GameClient GetClientByUserID(int userID)
        {
            if (_userIDRegister.ContainsKey(userID))
            {
                return _userIDRegister[userID];
            }

            return null;
        }

        internal Dictionary<int, GameClient> getGuides()
        {
            return guides;
        }

        internal Dictionary<int, GameClient> getAlphas()
        {
            return alphas;
        }

        internal void addToAlphas(int id, GameClient client)
        {
            //visaUsers[id] = client;
            alphas[id] = client;
        }

        internal void modifyGuide(bool online, GameClient c)
        {
            if (c == null || c.GetHabbo() == null)
            {
                return;
            }

            if (online)
            {
                guides[c.GetHabbo().Id] = c;
            }
            else
            {
                guides.Remove(c.GetHabbo().Id);
            }
        }

        public GameClient GetClientByUsername(string username)
        {
            if (_usernameRegister.ContainsKey(username.ToLower()))
            {
                return _usernameRegister[username.ToLower()];
            }

            return null;
        }

        public void sessionHandleMessage(IWebSocketConnection socket, byte[] message)
        {

            if (_sessionRegister.TryGetValue(socket.ConnectionInfo.Id, out Session session))
            {
                session.handleMessage(message);
            }
        }

        public void closeSession(IWebSocketConnection socket)
        {
            if (!_sessionRegister.TryRemove(socket.ConnectionInfo.Id, out Session session))
            {
                socket.Close();
            }
        }

        public void registerSession(IWebSocketConnection socket)
        {
            Session session = new Session(socket);
            if (!_sessionRegister.TryAdd(socket.ConnectionInfo.Id, session))
            {
                socket.Close();
            }
        }

        public bool TryGetClient(int ClientId, out GameClient Client)
        {
            return _clients.TryGetValue(ClientId, out Client);
        }

        public bool UpdateClientUsername(GameClient Client, string OldUsername, string NewUsername)
        {
            if (Client == null || !_usernameRegister.ContainsKey(OldUsername.ToLower()))
            {
                return false;
            }

            _usernameRegister.TryRemove(OldUsername.ToLower(), out Client);
            _usernameRegister.TryAdd(NewUsername.ToLower(), Client);
            return true;
        }

        public string GetNameById(int Id)
        {
            GameClient client = GetClientByUserID(Id);

            if (client != null)
            {
                return client.GetHabbo().Username;
            }

            string username;
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT username FROM users WHERE id = @id LIMIT 1");
                dbClient.AddParameter("id", Id);
                username = dbClient.getString();
            }

            return username;
        }

        public int GetUserIdByUsername(string Username)
        {
            GameClient client = GetClientByUsername(Username);

            if (client != null)
            {
                return client.GetHabbo().Id;
            }

            int userid;
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT id FROM users WHERE username = @username LIMIT 1");
                dbClient.AddParameter("username", Username);
                userid = dbClient.getInteger();
            }

            return userid;
        }

        public IEnumerable<GameClient> GetClientsById(Dictionary<int, MessengerBuddy>.KeyCollection users)
        {
            foreach (int id in users)
            {
                GameClient client = GetClientByUserID(id);
                if (client != null)
                {
                    yield return client;
                }
            }
        }

        public void RepeatAlert(ServerPacket Message, int Exclude = 0)
        {
            foreach (GameClient client in GetClients.ToList())
            {
                if (client == null || client.GetHabbo() == null)
                {
                    continue;
                }

                if (client.GetHabbo().Rank < 4 || client.GetHabbo().Id == Exclude)
                {
                    continue;
                }

                client.SendMessage(Message);
            }
        }

        public void StaffAlert1(ServerPacket Message, int Exclude = 0)
        {
            foreach (GameClient client in GetClients.ToList())
            {
                if (client == null || client.GetHabbo() == null)
                {
                    continue;
                }

                if (client.GetHabbo().Rank < 4 || client.GetHabbo().Id == Exclude || client.GetHabbo()._alerttype == "1")
                {
                    continue;
                }

                client.SendMessage(Message);
            }
        }

        public void StaffAlert2(ServerPacket Message, int Exclude = 0)
        {
            foreach (GameClient client in GetClients.ToList())
            {
                if (client == null || client.GetHabbo() == null)
                {
                    continue;
                }

                if (client.GetHabbo().Rank < 4 || client.GetHabbo().Id == Exclude || client.GetHabbo()._alerttype == "2")
                {
                    continue;
                }

                client.SendMessage(Message);
            }
        }

        public void StaffAlert3(string Message)
        {
            foreach (GameClient client in GetClients.ToList())
            {
                if (client == null || client.GetHabbo() == null)
                {
                    continue;
                }

                if (client.GetHabbo().Rank < 4)
                {
                    continue;
                }

                client.SendWhisper(Message, 23);
            }
        }

        public void StaffAlert(ServerPacket Message, int Exclude = 0)
        {
            foreach (GameClient client in GetClients.ToList())
            {
                if (client == null || client.GetHabbo() == null)
                {
                    continue;
                }

                if (client.GetHabbo().Rank < 4 || client.GetHabbo().Id == Exclude)
                {
                    continue;
                }

                client.SendMessage(Message);
            }
        }

        public void QuizzAlert(ServerPacket Message, Item Item, Room room, int Exclude = 0)
        {
            foreach (RoomUser RoomUser in room.GetRoomUserManager().GetRoomUsers())
            {
                if (RoomUser == null || RoomUser.GetClient().GetHabbo() == null)
                {
                    continue;
                }

                RoomUser Human = room.GetRoomUserManager().GetRoomUserByHabbo(RoomUser.GetClient().GetHabbo().Id);

                if (Human.X != Item.GetX && Human.Y != Item.GetY || RoomUser.GetClient().GetHabbo().Id == Exclude)
                {
                    continue;
                }

                RoomUser.GetClient().SendMessage(Message);
            }
        }

        public void ManagerAlert(ServerPacket Message, int Exclude = 0)
        {
            foreach (GameClient client in GetClients.ToList())
            {
                if (client == null || client.GetHabbo() == null)
                {
                    continue;
                }

                if (client.GetHabbo().Rank < 8 || client.GetHabbo().Id == Exclude)
                {
                    continue;
                }

                client.SendMessage(Message);
            }
        }

        public void GroupChatAlert(ServerPacket Message, Group Group, int Exclude = 0)
        {
            foreach (GameClient client in GetClients.ToList())
            {
                if (client == null || client.GetHabbo() == null)
                {
                    continue;
                }

                if (!Group.IsMember(client.GetHabbo().Id) || client.GetHabbo().Id == Exclude)
                {
                    continue;
                }

                client.SendMessage(Message);
            }
        }

        public void GuideAlert(ServerPacket Message, int Exclude = 0)
        {
            foreach (GameClient client in GetClients.ToList())
            {
                if (client == null || client.GetHabbo() == null)
                {
                    continue;
                }

                if (client.GetHabbo()._guidelevel < 1 || client.GetHabbo().Id == Exclude)
                {
                    continue;
                }

                client.SendMessage(Message);
            }
        }

        //public void Friend(ServerPacket Message, int Exclude = 0)
        //{
        //    foreach (GameClient client in this.GetClients.ToList())
        //    {
        //        if (client == null || client.GetHabbo() == null)
        //            continue;

        //        if (client.GetHabbo().GetMessenger().GetFriends().Count < 0 || client.GetHabbo().Id == Exclude)
        //            continue;

        //        client.GetHabbo().GetMessenger().GetFriends().SendMessage(Message);
        //    }
        //}

        public void SupportMessage(ServerPacket Message, int Exclude = 0)
        {
            foreach (GameClient client in GetClients.ToList())
            {
                if (client == null || client.GetHabbo() == null)
                {
                    continue;
                }

                if (client.GetHabbo()._guidelevel < 1 || client.GetHabbo().Id == Exclude)
                {
                    continue;
                }
                else if (client.GetHabbo().Rank < 3 || client.GetHabbo().Id == Exclude)
                {
                    continue;
                }

                client.SendMessage(Message);
            }
        }

        public void ModAlert(string Message)
        {
            foreach (GameClient client in GetClients.ToList())
            {
                if (client == null || client.GetHabbo() == null)
                {
                    continue;
                }

                if (client.GetHabbo().GetPermissions().HasRight("mod_tool") && !client.GetHabbo().GetPermissions().HasRight("staff_ignore_mod_alert"))
                {
                    try { client.SendWhisper(Message, 5); }
                    catch { }
                }
            }
        }

        public void DoAdvertisingReport(GameClient Reporter, GameClient Target)
        {
            if (Reporter == null || Target == null || Reporter.GetHabbo() == null || Target.GetHabbo() == null)
            {
                return;
            }

            StringBuilder Builder = new StringBuilder();
            Builder.Append("Nuevo Informe!\r\r");
            Builder.Append("Reportador: " + Reporter.GetHabbo().Username + "\r");
            Builder.Append("Usuario Reportado: " + Target.GetHabbo().Username + "\r\r");
            Builder.Append(Target.GetHabbo().Username + "Ultimos 10 msj:\r\r");

            DataTable GetLogs = null;
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `message` FROM `chatlogs` WHERE `user_id` = '" + Target.GetHabbo().Id + "' ORDER BY `id` DESC LIMIT 10");
                GetLogs = dbClient.getTable();

                if (GetLogs != null)
                {
                    int Number = 11;
                    foreach (DataRow Log in GetLogs.Rows)
                    {
                        Number -= 1;
                        Builder.Append(Number + ": " + Convert.ToString(Log["message"]) + "\r");
                    }
                }
            }

            foreach (GameClient Client in GetClients.ToList())
            {
                if (Client == null || Client.GetHabbo() == null)
                {
                    continue;
                }

                if (Client.GetHabbo().GetPermissions().HasRight("mod_tool") && !Client.GetHabbo().GetPermissions().HasRight("staff_ignore_advertisement_reports"))
                {
                    Client.SendMessage(new MOTDNotificationComposer(Builder.ToString()));
                }
            }
        }

        internal IEnumerable<GameClient> GetClientsById(List<int> getAllMembers)
        {
            throw new NotImplementedException();
        }

        public void SendMessage(ServerPacket Packet, string fuse = "")
        {
            foreach (GameClient Client in _clients.Values.ToList())
            {
                if (Client == null || Client.GetHabbo() == null)
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(fuse))
                {
                    if (!Client.GetHabbo().GetPermissions().HasRight(fuse))
                    {
                        continue;
                    }
                }

                Client.SendMessage(Packet);
            }
        }

        public void SendEventType1(ServerPacket Packet, string fuse = "")
        {
            foreach (GameClient Client in _clients.Values.ToList())
            {
                if (Client == null || Client.GetHabbo() == null || Client.GetHabbo()._eventtype == "1")
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(fuse))
                {
                    if (!Client.GetHabbo().GetPermissions().HasRight(fuse))
                    {
                        continue;
                    }
                }

                Client.SendMessage(Packet);
            }
        }

        public void SendEventType2(ServerPacket Packet, string fuse = "")
        {
            foreach (GameClient Client in _clients.Values.ToList())
            {
                if (Client == null || Client.GetHabbo() == null || Client.GetHabbo()._eventtype == "2")
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(fuse))
                {
                    if (!Client.GetHabbo().GetPermissions().HasRight(fuse))
                    {
                        continue;
                    }
                }

                Client.SendMessage(Packet);
            }
        }

        public void CreateAndStartClient(int clientID, ConnectionInformation connection)
        {
            GameClient Client = new GameClient(clientID, connection);
            if (_clients.TryAdd(Client.ConnectionID, Client))
            {
                Client.StartConnection();
            }
            else
            {
                connection.Dispose();
            }
        }

        public void DisposeConnection(int clientID)
        {
            if (!TryGetClient(clientID, out GameClient Client))
            {
                return;
            }

            if (Client != null)
            {
                Client.Dispose();
            }

            _clients.TryRemove(clientID, out Client);
        }

        public void LogClonesOut(int UserID)
        {
            GameClient client = GetClientByUserID(UserID);
            if (client != null)
            {
                client.Disconnect();
            }
        }

        public void RegisterClient(GameClient client, int userID, string username)
        {
            if (_usernameRegister.ContainsKey(username.ToLower()))
            {
                _usernameRegister[username.ToLower()] = client;
            }
            else
            {
                _usernameRegister.TryAdd(username.ToLower(), client);
            }

            if (_userIDRegister.ContainsKey(userID))
            {
                _userIDRegister[userID] = client;
            }
            else
            {
                _userIDRegister.TryAdd(userID, client);
            }
        }

        public void UnregisterClient(int userid, string username)
        {
            _userIDRegister.TryRemove(userid, out GameClient Client);
            _usernameRegister.TryRemove(username.ToLower(), out Client);
        }

        public void CloseAll()
        {
            foreach (GameClient client in GetClients.ToList())
            {
                if (client == null)
                {
                    continue;
                }

                if (client.GetHabbo() != null)
                {
                    try
                    {
                        using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.RunQuery(client.GetHabbo().GetQueryString);
                        }
                        Console.Clear();
                        log.Info("<<- SERVER SHUTDOWN ->> GUARDANDO INVENTARIO");
                    }
                    catch
                    {
                    }
                }
            }

            log.Info("Guardando los datos de los usuarios!");
            log.Info("Cerrando todas las conexiones.");
            try
            {
                foreach (GameClient client in GetClients.ToList())
                {
                    if (client == null || client.GetConnection() == null)
                    {
                        continue;
                    }

                    try
                    {
                        client.GetConnection().Dispose();
                    }
                    catch { }

                    Console.Clear();
                    log.Info("<<- SERVER SHUTDOWN ->> CERRANDO CONEXIONES");

                }
            }
            catch (Exception e)
            {
                Logging.LogCriticalException(e.ToString());
            }

            if (_clients.Count > 0)
            {
                _clients.Clear();
            }

            log.Info("Cerrando Conexion!");
        }

        private void TestClientConnections()
        {
            if (clientPingStopwatch.ElapsedMilliseconds >= 30000)
            {
                clientPingStopwatch.Restart();

                try
                {
                    List<GameClient> ToPing = new List<GameClient>();

                    foreach (GameClient client in _clients.Values.ToList())
                    {
                        if (client.PingCount < 6)
                        {
                            client.PingCount++;

                            ToPing.Add(client);
                        }
                        else
                        {
                            lock (timedOutConnections.SyncRoot)
                            {
                                timedOutConnections.Enqueue(client);
                            }
                        }
                    }

                    DateTime start = DateTime.Now;

                    foreach (GameClient Client in ToPing.ToList())
                    {
                        try
                        {
                            Client.SendMessage(new PongComposer());
                        }
                        catch
                        {
                            lock (timedOutConnections.SyncRoot)
                            {
                                timedOutConnections.Enqueue(Client);
                            }
                        }
                    }

                }
                catch (Exception)
                {

                }
            }
        }

        private void HandleTimeouts()
        {
            if (timedOutConnections.Count > 0)
            {
                lock (timedOutConnections.SyncRoot)
                {
                    while (timedOutConnections.Count > 0)
                    {
                        GameClient client = null;

                        if (timedOutConnections.Count > 0)
                        {
                            client = (GameClient)timedOutConnections.Dequeue();
                        }

                        if (client != null)
                        {
                            client.Disconnect();
                        }
                    }
                }
            }
        }

        public int Count => _clients.Count;

        public ICollection<GameClient> GetClients => _clients.Values;
    }
}