using log4net;
using Neon.Communication.Packets.Outgoing.Handshake;
using Neon.Communication.Packets.Outgoing.Inventory.Purse;
using Neon.Communication.Packets.Outgoing.LandingView;
using Neon.Communication.Packets.Outgoing.Navigator;
using Neon.Communication.Packets.Outgoing.Rooms.Engine;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.Communication.Packets.Outgoing.Rooms.Session;
using Neon.Core;
using Neon.Database.Interfaces;
using Neon.HabboHotel.Achievements;
using Neon.HabboHotel.Catalog;
using Neon.HabboHotel.Club;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Helpers;
using Neon.HabboHotel.Items;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Rooms.Chat.Commands;
using Neon.HabboHotel.Subscriptions;
using Neon.HabboHotel.Users.Badges;
using Neon.HabboHotel.Users.Clothing;
using Neon.HabboHotel.Users.Effects;
using Neon.HabboHotel.Users.Inventory;
using Neon.HabboHotel.Users.Messenger;
using Neon.HabboHotel.Users.Messenger.FriendBar;
using Neon.HabboHotel.Users.Navigator.SavedSearches;
using Neon.HabboHotel.Users.Permissions;
using Neon.HabboHotel.Users.Polls;
using Neon.HabboHotel.Users.Process;
using Neon.HabboHotel.Users.Relationships;
using Neon.HabboHotel.Users.UserDataManagement;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;

namespace Neon.HabboHotel.Users
{
    public class Habbo
    {
        private static readonly ILog log = LogManager.GetLogger("Neon.HabboHotel.Users");

        //Prefijos 
        public string _tag;
        public string _tagcolor;
        public string _nameColor;
        public int _bet;
        public bool _playingChess = false;
        public string _eColor = "";

        // Leaderboards
        public int _leaderPoints;
        public int _leaderRecord;

        // Bools Custom Settings
        public bool _isControlling = false;

        //Abilitys triggered by generic events.
        public string _alerttype = "2";
        public string _eventtype = "2";
        public int _eventsopened;
        internal string lastLayout;

        public long _lastTimeUsedHelpCommand;

        //Player saving.
        private bool _disconnected;
        private bool _habboSaved;
        public byte _changename;

        public Dictionary<string, int> WiredRewards;
        public byte _guidelevel;
        public byte _publicistalevel;
        public byte _builder;
        public byte _croupier;
        public bool _isFirstThrow;
        public bool _hisTurn = false;
        public byte _TargetedBuy;
        public bool Spectating = false;
        public string _Opponent;
        public string chatHTMLColour;
        public int chatHTMLSize;

        public bool isPasting = false;
        public bool isDeveloping = false;
        public int lastX;
        public int lastY;
        public int lastX2;
        public int lastY2;

        //Alfas
        internal bool onDuty;
        internal bool onService;
        internal uint userHelping;
#pragma warning disable CS0649 // Campo "Habbo.rankHelper" nunca é atribuído e sempre terá seu valor padrão
        internal TypeOfHelper rankHelper;
#pragma warning restore CS0649 // Campo "Habbo.rankHelper" nunca é atribuído e sempre terá seu valor padrão
        internal bool requestHelp;
        internal bool requestTour;
        internal bool reportsOfHarassment;
        public bool _SecureTradeEnabled = false;
        public bool _SecurityQuestion = false;
        public bool _IsBeingAsked = false;


        // Camara
        public string _lastPhotoPreview;
        public string lastPhotoPreview;
        public int LastSqlQuery = 0;
        public bool _sellingroom = false;

        public bool StaffOk = false;
        public int LastCraftingMachine = 0;
        public int LastEffect = 0;
        public int EventType = 1;
        public bool _NUX;
        public bool PassedNuxNavigator = false, PassedNuxDuckets = false, PassedNuxItems = false, PassedNuxChat = false, PassedNuxCatalog = false;
        public List<int> RatedRooms;
        public List<int> MutedUsers;
        public List<RoomUser> MultiWhispers;
        public List<RoomData> UsersRooms;
        public List<Item> TradeItems;
        public bool _isBettingDice = false;

        private GameClient _client;
        private readonly HabboStats _habboStats;
        private HabboMessenger Messenger;
        private ClubManager ClubManager;
        private ProcessComponent _process;
        public ArrayList FavoriteRooms;
        public ArrayList Tags;
        public ArrayList MysticKeys;
        public ArrayList MysticBoxes;
        public Dictionary<int, int> quests;
        private BadgeComponent BadgeComponent;
        private InventoryComponent InventoryComponent;
        public Dictionary<int, CatalogItem> _lastitems;
        public Dictionary<int, Relationship> Relationships;
        public ConcurrentDictionary<string, UserAchievement> Achievements;
        private PollsComponent _polls;

        private readonly DateTime _timeCached;
        private SearchesComponent _navigatorSearches;
        private EffectsComponent _fx;
        private ClothingComponent _clothing;
        private PermissionComponent _permissions;

        public double ForceHeight;
        public double StackHeight;
        public bool PassedQuiz;

        public bool _multiWhisper;
        public bool IsCitizen => CurrentTalentLevel > 4;
        internal List<int> _HabboQuizQuestions;

        internal string chatColour;
        public bool[] calendarGift;

        public Habbo(int Id, string Username, int Rank, string Motto, string Look, string Gender, int Credits, int ActivityPoints, int HomeRoom,
         bool HasFriendRequestsDisabled, int LastOnline, bool AppearOffline, bool HideInRoom, double CreateDate, int Diamonds,
         string machineID, string clientVolume, bool ChatPreference, bool FocusPreference, bool PetsMuted, bool BotsMuted, bool AdvertisingReportBlocked, double LastNameChange,
         int GOTWPoints, int UserPoints, bool IgnoreInvites, double TimeMuted, double TradingLock, bool AllowGifts, int FriendBarState, bool DisableForcedEffects, bool AllowMimic, int VIPRank,
         byte guidelevel, byte publicistalevel, byte builder, byte croupier, bool Nux, byte TargetedBuy, string NameColor, string Tag, string TagColor, byte NameChange, string PinClient, int CatRank)
        {
            this.Id = Id;
            this.Username = Username;
            this.Rank = Rank;
            this.CatRank = CatRank;
            this.Motto = Motto;
            this.Look = NeonEnvironment.GetGame().GetAntiMutant().RunLook(Look);
            this.Gender = Gender.ToLower();
            FootballLook = NeonEnvironment.FilterFigure(Look.ToLower());
            FootballGender = Gender.ToLower();
            this.Credits = Credits;
            Duckets = ActivityPoints;
            this.Diamonds = Diamonds;
            this.GOTWPoints = GOTWPoints;
            this.PinClient = PinClient;
            _NUX = Nux;
            this.UserPoints = UserPoints;
            this.HomeRoom = HomeRoom;
            this.LastOnline = LastOnline;
            _guidelevel = guidelevel;
            _publicistalevel = publicistalevel;
            _builder = builder;
            _croupier = croupier;
            _TargetedBuy = TargetedBuy;
            AccountCreated = CreateDate;
            ClientVolume = new List<int>();
            _nameColor = NameColor;
            _tag = Tag;
            _tagcolor = TagColor;
            _changename = NameChange;
            foreach (string Str in clientVolume.Split(','))
            {
                if (int.TryParse(Str, out _))
                {
                    ClientVolume.Add(int.Parse(Str));
                }
                else
                {
                    ClientVolume.Add(100);
                }
            }

            this.LastNameChange = LastNameChange;
            MachineId = machineID;
            this.ChatPreference = ChatPreference;
            this.FocusPreference = FocusPreference;
            IsExpert = IsExpert == true;

            this.AppearOffline = AppearOffline;
            AllowTradingRequests = true;//TODO
            AllowUserFollowing = true;//TODO
            AllowFriendRequests = HasFriendRequestsDisabled;//TODO
            AllowMessengerInvites = IgnoreInvites;
            AllowPetSpeech = PetsMuted;
            AllowBotSpeech = BotsMuted;
            AllowPublicRoomStatus = HideInRoom;
            AllowConsoleMessages = true;
            this.AllowGifts = AllowGifts;
            this.AllowMimic = AllowMimic;
            _lastPhotoPreview = lastPhotoPreview;
            ReceiveWhispers = true;
            IgnorePublicWhispers = false;
            PlayingFastFood = false;
            FriendbarState = FriendBarStateUtility.GetEnum(FriendBarState);
            ChristmasDay = ChristmasDay;
            WantsToRideHorse = 0;
            TimeAFK = 0;
            this.DisableForcedEffects = DisableForcedEffects;
            this.VIPRank = VIPRank;
            _bet = 0;

            onDuty = false;
            requestHelp = false;
            requestTour = false;
            userHelping = 0;
            reportsOfHarassment = false;
            onService = false;

            _disconnected = false;
            _habboSaved = false;
            ChangingName = false;

            FloodTime = 0;
            FriendCount = 0;
            this.TimeMuted = TimeMuted;
            _timeCached = DateTime.Now;

            _sellingroom = false;

            //this._CurrentTalentLevel = GetCurrentTalentLevel();

            TradingLockExpiry = TradingLock;
            if (TradingLockExpiry > 0 && NeonEnvironment.GetUnixTimestamp() > TradingLockExpiry)
            {
                TradingLockExpiry = 0;
                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunQuery("UPDATE `user_info` SET `trading_locked` = '0' WHERE `user_id` = '" + Id + "' LIMIT 1");
                }
            }

            BannedPhraseCount = 0;
            SessionStart = NeonEnvironment.GetUnixTimestamp();
            MessengerSpamCount = 0;
            MessengerSpamTime = 0;
            CreditsUpdateTick = NeonStaticGameSettings.UserCreditsUpdateTimer;

            TentId = 0;
            HopperId = 0;
            IsHopping = false;
            TeleporterId = 0;
            IsTeleporting = false;
            TeleportingRoomID = 0;
            RoomAuthOk = false;
            CurrentRoomId = 0;

            HasSpoken = false;
            LastAdvertiseReport = 0;
            AdvertisingReported = false;
            AdvertisingReportedBlocked = AdvertisingReportBlocked;

            _multiWhisper = false;
            WiredInteraction = false;
            QuestLastCompleted = 0;
            InventoryAlert = false;
            IgnoreBobbaFilter = false;
            WiredTeleporting = false;
            LastClothingUpdateTime = DateTime.Now;
            CustomBubbleId = 0;
            OnHelperDuty = false;
            FastfoodScore = 0;
            PetId = 0;
            TempInt = 0;

            LastGiftPurchaseTime = DateTime.Now;
            LastMottoUpdateTime = DateTime.Now;
            LastForumMessageUpdateTime = DateTime.Now;
            ClothingUpdateWarnings = 0;

            GiftPurchasingWarnings = 0;
            MottoUpdateWarnings = 0;

            SessionGiftBlocked = false;
            SessionMottoBlocked = false;
            _isFirstThrow = true;
            SessionClothingBlocked = false;

            FavoriteRooms = new ArrayList();
            MutedUsers = new List<int>();
            MultiWhispers = new List<RoomUser>();
            Achievements = new ConcurrentDictionary<string, UserAchievement>();
            Relationships = new Dictionary<int, Relationship>();
            RatedRooms = new List<int>();
            UsersRooms = new List<RoomData>();
            TradeItems = new List<Item>();

            //TODO: Nope.
            InitPermissions();

            #region Stats
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `id`,`roomvisits`,`onlinetime`,`respect`,`respectgiven`,`giftsgiven`,`giftsreceived`,`dailyrespectpoints`,`dailypetrespectpoints`,`achievementscore`,`quest_id`,`quest_progress`,`groupid`,`tickets_answered`,`respectstimestamp`,`forum_posts`, `PurchaseUsersConcurrent`, `vip_gifts` FROM `user_stats` WHERE `id` = @user_id LIMIT 1");
                dbClient.AddParameter("user_id", Id);

                DataRow StatRow = dbClient.getRow();

                if (StatRow == null)//No row, add it yo
                {
                    dbClient.RunQuery("INSERT INTO `user_stats` (`id`) VALUES ('" + Id + "')");
                    dbClient.SetQuery("SELECT `id`,`roomvisits`,`onlinetime`,`respect`,`respectgiven`,`giftsgiven`,`giftsreceived`,`dailyrespectpoints`,`dailypetrespectpoints`,`achievementscore`,`quest_id`,`quest_progress`,`groupid`,`tickets_answered`,`respectstimestamp`,`forum_posts`, `PurchaseUsersConcurrent`, `vip_gifts` FROM `user_stats` WHERE `id` = @user_id LIMIT 1");
                    dbClient.AddParameter("user_id", Id);
                    StatRow = dbClient.getRow();
                }

                try
                {
                    _habboStats = new HabboStats(Convert.ToInt32(StatRow["roomvisits"]), Convert.ToDouble(StatRow["onlineTime"]), Convert.ToInt32(StatRow["respect"]), Convert.ToInt32(StatRow["respectGiven"]), Convert.ToInt32(StatRow["giftsGiven"]),
                        Convert.ToInt32(StatRow["giftsReceived"]), Convert.ToInt32(StatRow["dailyRespectPoints"]), Convert.ToInt32(StatRow["dailyPetRespectPoints"]), Convert.ToInt32(StatRow["AchievementScore"]),
                        Convert.ToInt32(StatRow["quest_id"]), Convert.ToInt32(StatRow["quest_progress"]), Convert.ToInt32(StatRow["groupid"]), Convert.ToString(StatRow["respectsTimestamp"]), Convert.ToInt32(StatRow["forum_posts"]), Convert.ToBoolean(StatRow["PurchaseUsersConcurrent"]), Convert.ToInt32(StatRow["vip_gifts"]));

                    if (Convert.ToString(StatRow["respectsTimestamp"]) != DateTime.Today.ToString("MM/dd"))
                    {
                        _habboStats.RespectsTimestamp = DateTime.Today.ToString("MM/dd");
                        SubscriptionData SubData = null;

                        int DailyRespects = 3;

                        if (_permissions.HasRight("mod_tool"))
                        {
                            DailyRespects = 20;
                        }
                        else if (NeonEnvironment.GetGame().GetSubscriptionManager().TryGetSubscriptionData(VIPRank, out SubData))
                        {
                            DailyRespects = SubData.Respects;
                        }

                        _habboStats.DailyRespectPoints = DailyRespects;
                        _habboStats.DailyPetRespectPoints = DailyRespects;

                        dbClient.RunQuery("UPDATE `user_stats` SET `dailyRespectPoints` = '" + DailyRespects + "', `dailyPetRespectPoints` = '" + DailyRespects + "', `respectsTimestamp` = '" + DateTime.Today.ToString("MM/dd") + "' WHERE `id` = '" + Id + "' LIMIT 1");
                    }
                }
                catch (Exception e)
                {
                    Logging.LogException(e.ToString());
                }
            }

            if (!NeonEnvironment.GetGame().GetGroupManager().TryGetGroup(_habboStats.FavouriteGroupId, out _))
            {
                _habboStats.FavouriteGroupId = 0;
            }
            #endregion
        }



        internal ClubManager GetClubManager()
        {
            return ClubManager;
        }
        public string PrefixName
        {
            get => _tag;
            set => _tag = value;
        }

        public string EColor
        {
            get => _eColor;
            set => _eColor = value;
        }

        public string PrefixColor
        {
            get => _tagcolor;
            set => _tagcolor = value;
        }

        public string NameColor
        {
            get => _nameColor;
            set => _nameColor = value;
        }

        public int Id { get; set; }

        public int LastUserId { get; set; }

        public string Username { get; set; }

        public int Rank { get; set; }

        public int CatRank { get; set; }

        public string Motto { get; set; }

        public string Look { get; set; }

        public string Gender { get; set; }

        public string FootballLook { get; set; }

        private bool InitPolls()
        {
            _polls = new PollsComponent();

            return _polls.Init(this);
        }

        public PollsComponent GetPolls()
        {
            return _polls;
        }

        public string FootballGender { get; set; }

        public bool LastMovFGate { get; set; }

        // Dice System

        public bool FirstThrow
        {
            get => _isFirstThrow;
            set => _isFirstThrow = value;
        }

        public bool IsControlling
        {
            get => _isControlling;
            set => _isControlling = value;
        }

        public bool HisTurn
        {
            get => _hisTurn;
            set => _hisTurn = value;
        }

        public string Opponent
        {
            get => _Opponent;
            set => _Opponent = value;
        }

        public bool MultiWhisper
        {
            get => _multiWhisper;
            set => _multiWhisper = value;
        }
        public bool BlnInv { get; set; }

        public string BackupLook { get; set; }

        public string BackupGender { get; set; }

        public int Credits { get; set; }

        public int Duckets { get; set; }

        public int Diamonds { get; set; }

        public bool RigDice { get; set; }

        public int DiceNumber { get; set; }

        public string PinClient { get; set; }

        public int GOTWPoints { get; set; }

        public int BonusPoints { get; set; }

        public int UserPoints { get; set; }

        public int HomeRoom { get; set; }

        public double LastOnline { get; set; }

        public double AccountCreated { get; set; }

        public List<int> ClientVolume { get; set; }

        public double LastNameChange { get; set; }

        public string MachineId { get; set; }

        public bool ChatPreference { get; set; }
        public bool FocusPreference { get; set; }

        public bool IsExpert { get; set; }

        public bool AppearOffline { get; set; }

        public int VIPRank { get; set; }

        public int TempInt { get; set; }

        public bool AllowTradingRequests { get; set; }

        public bool AllowUserFollowing { get; set; }

        public bool AllowFriendRequests { get; set; }

        public bool AllowMessengerInvites { get; set; }

        public bool AllowPetSpeech { get; set; }

        public bool AllowBotSpeech { get; set; }

        public bool AllowPublicRoomStatus { get; set; }

        public bool AllowConsoleMessages { get; set; }

        public bool AllowGifts { get; set; }

        // CHESS SYSTEM
        public bool PlayingChess
        {
            get => _playingChess;
            set => _playingChess = value;
        }

        public bool AllowMimic { get; set; }

        public bool ReceiveWhispers { get; set; }

        public bool IgnorePublicWhispers { get; set; }

        public bool PlayingFastFood { get; set; }

        public FriendBarState FriendbarState { get; set; }

        public int ChristmasDay { get; set; }

        public int WantsToRideHorse { get; set; }

        public int TimeAFK { get; set; }

        public string LastMessage { get; set; }

        public int LastMessageCount { get; set; }

        public bool DisableForcedEffects { get; set; } = false;

        public bool ChangingName { get; set; }

        public int FriendCount { get; set; }

        public double FloodTime { get; set; }

        public int BannedPhraseCount { get; set; }

        public bool RoomAuthOk { get; set; }

        public int CurrentRoomId { get; set; }

        public int QuestLastCompleted { get; set; }

        public int MessengerSpamCount { get; set; }

        public double MessengerSpamTime { get; set; }

        public double TimeMuted { get; set; }

        public double TradingLockExpiry { get; set; }

        public double SessionStart { get; set; }

        public int TentId { get; set; }

        public int HopperId { get; set; }

        public bool IsHopping { get; set; }

        public int TeleporterId { get; set; }

        public bool IsTeleporting { get; set; }

        public int TeleportingRoomID { get; set; }

        public bool HasSpoken { get; set; }

        public double LastAdvertiseReport { get; set; }

        public bool AdvertisingReported { get; set; }

        public bool AdvertisingReportedBlocked { get; set; }

        public bool WiredInteraction { get; set; }

        public bool InventoryAlert { get; set; }

        public bool IgnoreBobbaFilter { get; set; }

        public bool WiredTeleporting { get; set; }

        public int CustomBubbleId { get; set; }

        public bool OnHelperDuty { get; set; }

        public int FastfoodScore { get; set; }

        public int PetId { get; set; }

        public int CreditsUpdateTick { get; set; }

        public int BonusUpdateTick { get; set; }

        public IChatCommand IChatCommand { get; set; }

        public DateTime LastGiftPurchaseTime { get; set; }

        public DateTime LastMottoUpdateTime { get; set; }

        public DateTime LastClothingUpdateTime { get; set; }


        public DateTime LastForumMessageUpdateTime { get; set; }

        public int GiftPurchasingWarnings { get; set; }

        public int MottoUpdateWarnings { get; set; }

        public int ClothingUpdateWarnings { get; set; }

        public int CurrentTalentLevel { get; set; }

        public bool SessionGiftBlocked { get; set; }

        public bool SecureTradeEnabled
        {
            get => _SecureTradeEnabled;
            set => _SecureTradeEnabled = value;
        }

        public bool SecurityQuestion
        {
            get => _SecurityQuestion;
            set => _SecurityQuestion = value;
        }

        public bool PlayingDice
        {
            get => _isBettingDice;
            set => _isBettingDice = value;
        }

        public bool IsBeingAsked
        {
            get => _IsBeingAsked;
            set => _IsBeingAsked = value;
        }

        public bool SessionMottoBlocked { get; set; }

        public bool SessionClothingBlocked { get; set; }

        public HabboStats GetStats()
        {
            return _habboStats;
        }

        public bool InRoom => CurrentRoomId >= 1 && CurrentRoom != null;

        public Room CurrentRoom
        {
            get
            {
                if (CurrentRoomId <= 0)
                {
                    return null;
                }

                if (NeonEnvironment.GetGame().GetRoomManager().TryGetRoom(CurrentRoomId, out Room _room))
                {
                    return _room;
                }

                return null;
            }
        }

        public bool CacheExpired()
        {
            TimeSpan Span = DateTime.Now - _timeCached;
            return (Span.TotalMinutes >= 30);
        }

        public string GetQueryString
        {
            get
            {
                _habboSaved = true;
                return "UPDATE `users` SET `online` = '0', `last_online` = '" + NeonEnvironment.GetUnixTimestamp() + "', `activity_points` = '" + Duckets + "', `credits` = '" + Credits + "', `vip_points` = '" + Diamonds + "' ,  `bonus_points` = '" + BonusPoints + "', `home_room` = '" + HomeRoom + "', `gotw_points` = '" + GOTWPoints + "', `user_points` = '" + UserPoints + "', `publi` = '" + _publicistalevel + "', `guia` = '" + _guidelevel + "', `builder` = '" + _builder + "', `croupier` = '" + _croupier + "', `time_muted` = '" + TimeMuted + "',`friend_bar_state` = '" + FriendBarStateUtility.GetInt(FriendbarState) + "' WHERE id = '" + Id + "' LIMIT 1;UPDATE `user_stats` SET `roomvisits` = '" + _habboStats.RoomVisits + "', `onlineTime` = '" + (NeonEnvironment.GetUnixTimestamp() - SessionStart + _habboStats.OnlineTime) + "', `respect` = '" + _habboStats.Respect + "', `respectGiven` = '" + _habboStats.RespectGiven + "', `giftsGiven` = '" + _habboStats.GiftsGiven + "', `giftsReceived` = '" + _habboStats.GiftsReceived + "', `dailyRespectPoints` = '" + _habboStats.DailyRespectPoints + "', `dailyPetRespectPoints` = '" + _habboStats.DailyPetRespectPoints + "', `AchievementScore` = '" + _habboStats.AchievementPoints + "', `quest_id` = '" + _habboStats.QuestID + "', `quest_progress` = '" + _habboStats.QuestProgress + "', `groupid` = '" + _habboStats.FavouriteGroupId + "',`forum_posts` = '" + _habboStats.ForumPosts + "',`PurchaseUsersConcurrent` = '" + _habboStats.PurchaseUsersConcurrent + "', `vip_gifts` = '" + _habboStats.vipGifts + "' WHERE `id` = '" + Id + "' LIMIT 1;";
            }
        }

        public bool InitProcess()
        {
            _process = new ProcessComponent();
            if (_process.Init(this))
            {
                return true;
            }

            return false;
        }

        public bool InitSearches()
        {
            _navigatorSearches = new SearchesComponent();
            if (_navigatorSearches.Init(this))
            {
                return true;
            }

            return false;
        }

        public bool InitFX()
        {
            _fx = new EffectsComponent();
            if (_fx.Init(this))
            {
                return true;
            }

            return false;
        }

        public bool InitClothing()
        {
            _clothing = new ClothingComponent();
            return _clothing.Init(this);
        }

        private bool InitPermissions()
        {
            _permissions = new PermissionComponent();
            if (_permissions.Init(this))
            {
                return true;
            }

            return false;
        }


        public void InitInformation(UserData data)
        {
            BadgeComponent = new BadgeComponent(this, data);
            Relationships = data.Relations;
        }

        public void Init(GameClient client, UserData data)
        {
            Achievements = data.achievements;

            FavoriteRooms = new ArrayList();
            foreach (int id in data.favouritedRooms)
            {
                FavoriteRooms.Add(id);
            }

            Tags = new ArrayList();
            foreach (string name in data.tags)
            {
                Tags.Add(name);
            }

            MysticKeys = new ArrayList();
            foreach (string key in data.MysticKeys)
            {
                MysticKeys.Add(key);
            }

            MysticBoxes = new ArrayList();
            foreach (string box in data.MysticBoxes)
            {
                MysticBoxes.Add(box);
            }

            MutedUsers = data.ignores;

            _client = client;
            BadgeComponent = new BadgeComponent(this, data);
            InventoryComponent = new InventoryComponent(Id, client);

            quests = data.quests;

            Messenger = new HabboMessenger(Id);
            Messenger.Init(data.friends, data.requests);
            FriendCount = Convert.ToInt32(data.friends.Count);
            _disconnected = false;
            UsersRooms = data.rooms;
            Relationships = data.Relations;

            InitSearches();
            InitFX();
            InitClothing();
            ClubManager = new ClubManager(Id, data);
            InitCalendar();
            InitPolls();

        }


        public PermissionComponent GetPermissions()
        {
            return _permissions;
        }

        public void OnDisconnect()
        {
            if (_disconnected)
            {
                return;
            }

            try
            {
                if (_process != null)
                {
                    _process.Dispose();
                }
            }
            catch { }

            _disconnected = true;

            if (ClubManager != null)
            {
                ClubManager.Clear();
                ClubManager = null;
            }

            if (OnHelperDuty)
            {
                GameClient Session = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(Id);
                HelperToolsManager.RemoveHelper(Session);
            }

            NeonEnvironment.GetGame().GetClientManager().UnregisterClient(Id, Username);

            if (!_habboSaved) // GUARDADO DE USER
            {
                _habboSaved = true;
                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.runFastQuery("UPDATE `users` SET `online` = '0', `last_online` = '" + NeonEnvironment.GetUnixTimestamp() + "', `activity_points` = '" + Duckets + "', `credits` = '" + Credits + "',  `vip_points` = '" + Diamonds + "' ,  `bonus_points` = '" + BonusPoints + "', `home_room` = '" + HomeRoom + "', `gotw_points` = '" + GOTWPoints + "', `user_points` = '" + UserPoints + "', `time_muted` = '" + TimeMuted + "',`friend_bar_state` = '" + FriendBarStateUtility.GetInt(FriendbarState) + "' WHERE id = '" + Id + "' LIMIT 1;UPDATE `user_stats` SET `roomvisits` = '" + _habboStats.RoomVisits + "', `onlineTime` = '" + (NeonEnvironment.GetUnixTimestamp() - SessionStart + _habboStats.OnlineTime) + "', `respect` = '" + _habboStats.Respect + "', `respectGiven` = '" + _habboStats.RespectGiven + "', `giftsGiven` = '" + _habboStats.GiftsGiven + "', `giftsReceived` = '" + _habboStats.GiftsReceived + "', `dailyRespectPoints` = '" + _habboStats.DailyRespectPoints + "', `dailyPetRespectPoints` = '" + _habboStats.DailyPetRespectPoints + "', `AchievementScore` = '" + _habboStats.AchievementPoints + "', `quest_id` = '" + _habboStats.QuestID + "', `quest_progress` = '" + _habboStats.QuestProgress + "', `groupid` = '" + _habboStats.FavouriteGroupId + "',`forum_posts` = '" + _habboStats.ForumPosts + "',`PurchaseUsersConcurrent` = '" + _habboStats.PurchaseUsersConcurrent + "' WHERE `id` = '" + Id + "' LIMIT 1;");

                    if (GetPermissions().HasRight("mod_tickets"))
                    {
                        dbClient.RunQuery("UPDATE `moderation_tickets` SET `status` = 'open', `moderator_id` = '0' WHERE `status` ='picked' AND `moderator_id` = '" + Id + "'");
                    }
                }
            }

            Dispose();

            _client = null;

        }

        public void Dispose()
        {
            if (InventoryComponent != null)
            {
                InventoryComponent.SetIdleState();
            }

            if (UsersRooms != null)
            {
                UsersRooms.Clear();
            }

            if (MultiWhispers != null)
            {
                MultiWhispers.Clear();
            }

            if (InRoom && CurrentRoom != null)
            {
                CurrentRoom.GetRoomUserManager().RemoveUserFromRoom(_client, false, false);
            }

            if (Messenger != null)
            {
                Messenger.AppearOffline = true;
                Messenger.Destroy();
            }

            if (_fx != null)
            {
                _fx.Dispose();
            }

            if (_clothing != null)
            {
                _clothing.Dispose();
            }

            if (_permissions != null)
            {
                _permissions.Dispose();
            }
        }

        public void CheckBonusTimer()
        {
            try
            {
                BonusUpdateTick--;

                if (BonusUpdateTick <= 0)
                {
                    int BonusUpdate = 1;

                    BonusPoints += BonusUpdate;

                    _client.SendMessage(new HabboActivityPointNotificationComposer(BonusPoints, BonusUpdate, 101));
                    _client.SendMessage(new RoomCustomizedAlertComposer("¡Enhorabuena! Has recibido un punto bonus por estar conectado durante 2 horas."));
                    _client.SendMessage(new BonusRareMessageComposer(_client));
                    BonusUpdateTick = NeonStaticGameSettings.BonusRareUpdateTimer;
                }
            }
            catch { }
        }
        public void CheckCreditsTimer()
        {
            try
            {
                CreditsUpdateTick--;

                if (CreditsUpdateTick <= 0)
                {
                    int CreditUpdate = NeonStaticGameSettings.UserCreditsUpdateAmount;
                    int DiamondUpdate = NeonStaticGameSettings.UserDiamondUpdateAmount;
                    int VipDucketUpdate = NeonStaticGameSettings.UserVipPixelsUpdateAmount;

                    Credits += CreditUpdate;
                    if (_client.GetHabbo().Rank == 2 && _client.GetHabbo().VIPRank == 1)
                    {
                        Duckets += VipDucketUpdate;
                    }
                    else
                    {
                        Diamonds += DiamondUpdate;
                    }


                    if (_client.GetHabbo().Rank == 2 && _client.GetHabbo().VIPRank == 1)
                    {
                        _client.SendMessage(new CreditBalanceComposer(Credits));
                        _client.SendMessage(new HabboActivityPointNotificationComposer(Duckets, VipDucketUpdate));
                    }
                    else
                    {
                        _client.SendMessage(new CreditBalanceComposer(Credits));
                        _client.SendMessage(new HabboActivityPointNotificationComposer(Diamonds, DiamondUpdate, 5));
                    }

                    if (_client.GetHabbo().Rank == 2 && _client.GetHabbo().VIPRank == 1)
                    {
                        GetClient().SendMessage(RoomNotificationComposer.SendBubble("newuser", "Has recibido " + CreditUpdate + " créditos y " + VipDucketUpdate + " diamantes por estar conectado 30 minutos.", ""));

                    }
                    else
                    {
                        GetClient().SendMessage(RoomNotificationComposer.SendBubble("newuser", "Has recibido " + CreditUpdate + " créditos y " + DiamondUpdate + " diamantes por estar conectado 30 minutos.", ""));
                    }

                    CreditsUpdateTick = NeonStaticGameSettings.UserCreditsUpdateTimer;
                }
            }
            catch { }
        }

        public GameClient GetClient()
        {
            if (_client != null)
            {
                return _client;
            }

            return NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(Id);
        }

        public HabboMessenger GetMessenger()
        {
            return Messenger;
        }

        public BadgeComponent GetBadgeComponent()
        {
            return BadgeComponent;
        }

        public InventoryComponent GetInventoryComponent()
        {
            return InventoryComponent;
        }

        public SearchesComponent GetNavigatorSearches()
        {
            return _navigatorSearches;
        }

        public EffectsComponent Effects()
        {
            return _fx;
        }

        public ClothingComponent GetClothing()
        {
            return _clothing;
        }

        public int GetQuestProgress(int p)
        {
            quests.TryGetValue(p, out int progress);
            return progress;
        }

        public UserAchievement GetAchievementData(string p)
        {
            Achievements.TryGetValue(p, out UserAchievement achievement);
            return achievement;
        }

        public void ChangeName(string Username)
        {
            LastNameChange = NeonEnvironment.GetUnixTimestamp();
            this.Username = Username;

            SaveKey("username", Username);
            SaveKey("last_change", LastNameChange.ToString());
        }

        public void SaveKey(string Key, string Value)
        {
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET " + Key + " = @value WHERE `id` = '" + Id + "' LIMIT 1;");
                dbClient.AddParameter("value", Value);
                dbClient.RunQuery();
            }
        }

        public void PrepareRoom(int Id, string Password)
        {
            if (GetClient() == null || GetClient().GetHabbo() == null)
            {
                return;
            }

            if (GetClient().GetHabbo().InRoom)
            {
                if (!NeonEnvironment.GetGame().GetRoomManager().TryGetRoom(GetClient().GetHabbo().CurrentRoomId, out Room OldRoom))
                {
                    return;
                }

                if (OldRoom.GetRoomUserManager() != null)
                {
                    OldRoom.GetRoomUserManager().RemoveUserFromRoom(GetClient(), false, false);
                }
            }

            if (GetClient().GetHabbo().IsTeleporting && GetClient().GetHabbo().TeleportingRoomID != Id)
            {
                GetClient().SendMessage(new CloseConnectionComposer(GetClient()));
                return;
            }

            Room Room = NeonEnvironment.GetGame().GetRoomManager().LoadRoom(Id);
            if (Room == null)
            {
                GetClient().SendMessage(new CloseConnectionComposer(GetClient()));
                return;
            }

            if (Room.isCrashed)
            {
                GetClient().SendNotification("La sala no está disponible en estos momentos, ponte en contacto con un administrador.");
                GetClient().SendMessage(new CloseConnectionComposer(GetClient()));
                return;
            }

            GetClient().GetHabbo().CurrentRoomId = Room.RoomId;

            if (!GetClient().GetHabbo().GetPermissions().HasRight("room_ban_override") && Room.UserIsBanned(GetClient().GetHabbo().Id))
            {
                if (Room.HasBanExpired(GetClient().GetHabbo().Id))
                {
                    Room.RemoveBan(GetClient().GetHabbo().Id);
                }
                else
                {
                    GetClient().GetHabbo().RoomAuthOk = false;
                    GetClient().SendMessage(new CantConnectComposer(4));
                    GetClient().SendMessage(new CloseConnectionComposer(GetClient()));
                    return;
                }
            }

            GetClient().SendMessage(new OpenConnectionComposer());

            if (Room.GetRoomUserManager().userCount >= Room.UsersMax && !GetClient().GetHabbo().GetPermissions().HasRight("room_enter_full") && GetClient().GetHabbo().Id != Room.OwnerId)
            {
                GetClient().SendMessage(new CantConnectComposer(1));
                GetClient().SendMessage(new CloseConnectionComposer(GetClient()));
                return;

            }

            if (!Room.CheckRights(GetClient(), true, true) && !GetClient().GetHabbo().IsTeleporting && !GetClient().GetHabbo().IsHopping)
            {
                if (Room.Access == RoomAccess.DOORBELL && !GetClient().GetHabbo().GetPermissions().HasRight("room_enter_locked"))
                {
                    if (Room.UserCount > 0)
                    {
                        GetClient().SendMessage(new DoorbellComposer(""));
                        Room.SendMessage(new DoorbellComposer(GetClient().GetHabbo().Username), true);
                        return;
                    }
                    else
                    {
                        GetClient().SendMessage(new FlatAccessDeniedComposer(""));
                        GetClient().SendMessage(new CloseConnectionComposer(GetClient()));
                        return;
                    }
                }
                else if (Room.Access == RoomAccess.PASSWORD && !GetClient().GetHabbo().GetPermissions().HasRight("room_enter_locked"))
                {
                    if (Password.ToLower() != Room.Password.ToLower() || string.IsNullOrWhiteSpace(Password))
                    {
                        GetClient().SendMessage(new GenericErrorComposer(-100002));
                        GetClient().SendMessage(new CloseConnectionComposer(GetClient()));
                        return;
                    }
                }
            }

            if (!EnterRoom(Room))
            {
                GetClient().SendMessage(new CloseConnectionComposer(GetClient()));
            }
        }

        public void InitCalendar()
        {
            if (!NeonEnvironment.GetGame().GetCalendarManager().CampaignEnable())
            {
                return;
            }

            calendarGift = new bool[NeonEnvironment.GetGame().GetCalendarManager().GetTotalDays()];
            for (int i = 0; i < calendarGift.Length; i++)
            {
                calendarGift[i] = false;
            }

            DataTable dTable = null;
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT day FROM user_campaign_gifts WHERE user_id = '" + Id + "' AND campaign_name = @name");
                dbClient.AddParameter("name", NeonEnvironment.GetGame().GetCalendarManager().GetCampaignName());
                dTable = dbClient.getTable();
            }

            if (dTable != null)
            {
                foreach (DataRow dRow in dTable.Rows)
                {
                    int Day = (int)dRow["day"];
                    calendarGift[Day - 1] = true;
                }
            }
        }

        public bool EnterRoom(Room Room)
        {
            if (Room == null)
            {
                GetClient().SendMessage(new CloseConnectionComposer(GetClient()));
            }

            GetClient().SendMessage(new RoomReadyComposer(Room.RoomId, Room.ModelName));
            if (Room.Wallpaper != "0.0")
            {
                GetClient().SendMessage(new RoomPropertyComposer("wallpaper", Room.Wallpaper));
            }

            if (Room.Floor != "0.0")
            {
                GetClient().SendMessage(new RoomPropertyComposer("floor", Room.Floor));
            }

            GetClient().SendMessage(new RoomPropertyComposer("landscape", Room.Landscape));
            GetClient().SendMessage(new RoomRatingComposer(Room.Score, !(GetClient().GetHabbo().RatedRooms.Contains(Room.RoomId) || Room.OwnerId == GetClient().GetHabbo().Id)));

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("INSERT INTO user_roomvisits (user_id,room_id,entry_timestamp,exit_timestamp,hour,minute) VALUES ('" + GetClient().GetHabbo().Id + "','" + GetClient().GetHabbo().CurrentRoomId + "','" + NeonEnvironment.GetUnixTimestamp() + "','0','" + DateTime.Now.Hour + "','" + DateTime.Now.Minute + "');");// +
            }


            if (Room.OwnerId != Id)
            {
                GetClient().GetHabbo().GetStats().RoomVisits += 1;
                NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(GetClient(), "ACH_RoomEntry", 1);
            }
            return true;
        }
    }

    internal enum TypeOfHelper
    {
        None,
        Guide,
        Alpha,
        Guardian
    }
}