
using log4net;

using Neon.Communication.Packets;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Moderation;
using Neon.HabboHotel.Support;
using Neon.HabboHotel.Catalog;
using Neon.HabboHotel.Items;
using Neon.HabboHotel.Items.Televisions;
using Neon.HabboHotel.Navigator;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Groups;
using Neon.HabboHotel.Quests;
using Neon.HabboHotel.Achievements;
using Neon.HabboHotel.LandingView;
using Neon.HabboHotel.Global;

using Neon.HabboHotel.Games;

using Neon.HabboHotel.Rooms.Chat;
using Neon.HabboHotel.Talents;
using Neon.HabboHotel.Bots;
using Neon.HabboHotel.Cache;
using Neon.HabboHotel.Rewards;
using Neon.HabboHotel.Badges;
using Neon.HabboHotel.Permissions;
using Neon.HabboHotel.Subscriptions;
using System.Threading;
using System.Threading.Tasks;
using Neon.HabboHotel.Camera;
using Neon.HabboHotel.Catalog.FurniMatic;
using Neon.Communication.Packets.Incoming.LandingView;
using Neon.HabboHotel.Helpers;
using Neon.HabboHotel.Groups.Forums;
using System;
using Neon.HabboHotel.Rooms.Music;
using Neon.HabboHotel.Items.Crafting;
using Neon.HabboHotel.Rooms.Polls;
using Neon.HabboHotel.Calendar;
using Neon.HabboHotel.Items.RentableSpaces;
using Neon.Communication.Packets.Outgoing.Nux;
using Neon.HabboHotel.LandingView.CommunityGoal;
using Neon.WebSockets;
//using Neon.HabboHotel.Alphas;

namespace Neon.HabboHotel
{
    public class Game
    {
        private static readonly ILog log = LogManager.GetLogger("Neon.HabboHotel.Game");

        public GroupForumManager GetGroupForumManager()
        {
            return forummanager;
        }

        private GroupForumManager forummanager;
        private readonly PacketManager _packetManager;
        private readonly GameClientManager _clientManager;
        private readonly ModerationManager _modManager;
        private readonly FrontpageManager _frontpageManager;
        private readonly ItemDataManager _itemDataManager;
        private readonly CatalogManager _catalogManager;
        private readonly TelevisionManager _televisionManager;//TODO: Initialize from the item manager.
        private readonly NavigatorManager _navigatorManager;
        private readonly RoomManager _roomManager;
        private readonly ChatManager _chatManager;
        private readonly GroupManager _groupManager;
        private readonly QuestManager _questManager;
        private readonly AchievementManager _achievementManager;
        private readonly TalentTrackManager _talentTrackManager;
        private readonly LandingViewManager _landingViewManager;//TODO: Rename class
        private readonly GameDataManager _gameDataManager;
        private readonly CraftingManager _craftingManager;
        private readonly ServerStatusUpdater _globalUpdater;
        private readonly LanguageLocale _languageLocale;
        private readonly AntiMutant _antiMutant;
        private readonly BotManager _botManager;
        private readonly CacheManager _cacheManager;
        private readonly RewardManager _rewardManager;
        private readonly BadgeManager _badgeManager;
        private readonly SongManager _musicManager;
        private readonly PermissionManager _permissionManager;
        private readonly SubscriptionManager _subscriptionManager;
        private readonly TargetedOffersManager _targetedoffersManager;
        //private readonly CameraPhotoManager _cameraManager;
        private readonly CrackableManager _crackableManager;
        private readonly FurniMaticRewardsManager _furniMaticRewardsManager;
        private readonly PollManager _pollManager;
        private readonly CommunityGoalVS _communityGoalVS;
        private readonly CalendarManager _calendarManager;
        private RentableSpaceManager _rentableSpaceManager;
        private readonly NuxUserGiftsManager _nuxusergiftManager;
        private readonly NuxUserGiftsListManager _nuxusergiftlistManager;
        private readonly LeaderBoardDataManager _leaderBoardDataManager;

        private bool _cycleEnded;
        private bool _cycleActive;
        private Task _gameCycle;
        private int _cycleSleepTime = 25;

        public Game()
        {
            GetHallOfFame.GetInstance().Load();
            this._packetManager = new PacketManager();
            this._rentableSpaceManager = new RentableSpaceManager();
            this._clientManager = new GameClientManager();
            this._modManager = new ModerationManager();

            this._itemDataManager = new ItemDataManager();
            this._itemDataManager.Init();
            //this._cameraManager = new CameraPhotoManager();
            //this._cameraManager.Init(this._itemDataManager);
            this._catalogManager = new CatalogManager();
            this._catalogManager.Init(this._itemDataManager);
            this._frontpageManager = new FrontpageManager();

            this._televisionManager = new TelevisionManager();
            this._crackableManager = new CrackableManager();
            this._crackableManager.Initialize(NeonEnvironment.GetDatabaseManager().GetQueryReactor());
            this._furniMaticRewardsManager = new FurniMaticRewardsManager();
            this._furniMaticRewardsManager.Initialize(NeonEnvironment.GetDatabaseManager().GetQueryReactor());

            this._craftingManager = new CraftingManager();
            this._craftingManager.Init();

            this._navigatorManager = new NavigatorManager();
            this._roomManager = new RoomManager();
            this._chatManager = new ChatManager();
            this._groupManager = new GroupManager();
            this._questManager = new QuestManager();
            this._achievementManager = new AchievementManager();
            this._talentTrackManager = new TalentTrackManager();

            this._landingViewManager = new LandingViewManager();
            this._gameDataManager = new GameDataManager();

            this._globalUpdater = new ServerStatusUpdater();
            this._globalUpdater.Init();

            this._languageLocale = new LanguageLocale();
            this._antiMutant = new AntiMutant();
            this._botManager = new BotManager();

            this._cacheManager = new CacheManager();
            this._rewardManager = new RewardManager();
            this._musicManager = new SongManager();

            this._badgeManager = new BadgeManager();
            this._badgeManager.Init();

            this.forummanager = new GroupForumManager();

            this._communityGoalVS = new CommunityGoalVS();
            this._communityGoalVS.LoadCommunityGoalVS();

            this._permissionManager = new PermissionManager();
            this._permissionManager.Init();

            this._subscriptionManager = new SubscriptionManager();
            this._subscriptionManager.Init();

            HelperToolsManager.Init();

            this._calendarManager = new CalendarManager();
            this._calendarManager.Init();

            this._leaderBoardDataManager = new LeaderBoardDataManager();

            this._targetedoffersManager = new TargetedOffersManager();
            this._targetedoffersManager.Initialize(NeonEnvironment.GetDatabaseManager().GetQueryReactor());

            this._nuxusergiftManager = new NuxUserGiftsManager();
            this._nuxusergiftManager.Initialize(NeonEnvironment.GetDatabaseManager().GetQueryReactor());

            this._nuxusergiftlistManager = new NuxUserGiftsListManager();
            this._nuxusergiftlistManager.Initialize(NeonEnvironment.GetDatabaseManager().GetQueryReactor());

            this._pollManager = new PollManager();
            this._pollManager.Init();
            WebSocketManager.StartListener();


        }

        public void StartGameLoop()
        {
            this._gameCycle = new Task(GameCycle);
            this._gameCycle.Start();

            this._cycleActive = true;
        }

        private void GameCycle()
        {
            while (this._cycleActive)
            {
                this._cycleEnded = false;

                NeonEnvironment.GetGame().GetRoomManager().OnCycle();
                NeonEnvironment.GetGame().GetClientManager().OnCycle();
                //AlphaManager.getInstance().onCycle();
                this._cycleEnded = true;
                Thread.Sleep(this._cycleSleepTime);
            }
        }

        public void StopGameLoop()
        {
            this._cycleActive = false;

            while (!this._cycleEnded)
            {
                Thread.Sleep(this._cycleSleepTime);
            }
        }

        public PacketManager GetPacketManager()
        {
            return _packetManager;
        }

        public GameClientManager GetClientManager()
        {
            return _clientManager;
        }

        public SongManager GetMusicManager()
        {
            return _musicManager;
        }

        public CatalogManager GetCatalog()
        {
            return _catalogManager;
        }

        public NavigatorManager GetNavigator()
        {
            return _navigatorManager;
        }

        public CalendarManager GetCalendarManager()
        {
            return _calendarManager;
        }

        public FrontpageManager GetCatalogFrontPageManager()
        {
            return this._frontpageManager;
        }

        public ItemDataManager GetItemManager()
        {
            return _itemDataManager;
        }

        public RoomManager GetRoomManager()
        {
            return _roomManager;
        }

        internal TargetedOffersManager GetTargetedOffersManager()
        {
            return _targetedoffersManager;
        }

        public AchievementManager GetAchievementManager()
        {
            return _achievementManager;
        }

        public TalentTrackManager GetTalentTrackManager()
        {
            return _talentTrackManager;
        }


        public ModerationManager GetModerationManager()
        {
            return this._modManager;
        }

        public PermissionManager GetPermissionManager()
        {
            return this._permissionManager;
        }

        public SubscriptionManager GetSubscriptionManager()
        {
            return this._subscriptionManager;
        }

        public QuestManager GetQuestManager()
        {
            return this._questManager;
        }

        public RentableSpaceManager GetRentableSpaceManager()
        {
            return _rentableSpaceManager;
        }

        public GroupManager GetGroupManager()
        {
            return _groupManager;
        }

        public LandingViewManager GetLandingManager()
        {
            return _landingViewManager;
        }
        public TelevisionManager GetTelevisionManager()
        {
            return _televisionManager;
        }

        public ChatManager GetChatManager()
        {
            return this._chatManager;
        }

        //public CameraPhotoManager GetCameraManager()
        //{
        //    return this._cameraManager;
        //}

        internal CrackableManager GetPinataManager()
        {
            return this._crackableManager;
        }

        public CraftingManager GetCraftingManager()
        {
            return this._craftingManager;
        }

        public FurniMaticRewardsManager GetFurniMaticRewardsMnager()
        {
            return this._furniMaticRewardsManager;
        }

        public GameDataManager GetGameDataManager()
        {
            return this._gameDataManager;
        }

        public LanguageLocale GetLanguageLocale()
        {
            return this._languageLocale;
        }
        public AntiMutant GetAntiMutant() => _antiMutant;

        public BotManager GetBotManager()
        {
            return this._botManager;
        }

        public CacheManager GetCacheManager()
        {
            return this._cacheManager;
        }

        public RewardManager GetRewardManager()
        {
            return this._rewardManager;
        }

        internal NuxUserGiftsManager GetNuxUserGiftsManager()
        {
            return this._nuxusergiftManager;
        }

        internal LeaderBoardDataManager GetLeaderBoardDataManager()
        {
            return this._leaderBoardDataManager;
        }

        internal NuxUserGiftsListManager GetNuxUserGiftsListManager()
        {
            return _nuxusergiftlistManager;
        }

        public BadgeManager GetBadgeManager()
        {
            return this._badgeManager;
        }


        internal object GetFilterComponent()
        {
            throw new NotImplementedException();
        }

        public PollManager GetPollManager()
        {
            return this._pollManager;
        }

        public CommunityGoalVS GetCommunityGoalVS()
        {
            return this._communityGoalVS;
        }
    }
}