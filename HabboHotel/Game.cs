
using log4net;
using Neon.Communication.Packets;
using Neon.Communication.Packets.Incoming.LandingView;
using Neon.Communication.Packets.Outgoing.Nux;
using Neon.HabboHotel.Achievements;
using Neon.HabboHotel.Badges;
using Neon.HabboHotel.Bots;
using Neon.HabboHotel.Cache;
using Neon.HabboHotel.Calendar;
using Neon.HabboHotel.Catalog;
using Neon.HabboHotel.Catalog.FurniMatic;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Games;
using Neon.HabboHotel.Global;
using Neon.HabboHotel.Groups;
using Neon.HabboHotel.Groups.Forums;
using Neon.HabboHotel.Helpers;
using Neon.HabboHotel.Items;
using Neon.HabboHotel.Items.Crafting;
using Neon.HabboHotel.Items.RentableSpaces;
using Neon.HabboHotel.Items.Televisions;
using Neon.HabboHotel.LandingView;
using Neon.HabboHotel.LandingView.CommunityGoal;
using Neon.HabboHotel.Moderation;
using Neon.HabboHotel.Navigator;
using Neon.HabboHotel.Permissions;
using Neon.HabboHotel.Quests;
using Neon.HabboHotel.Rewards;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Rooms.Chat;
using Neon.HabboHotel.Rooms.Music;
using Neon.HabboHotel.Rooms.Polls;
using Neon.HabboHotel.Subscriptions;
using Neon.HabboHotel.Talents;
using Neon.WebSockets;
using System;
using System.Threading;
using System.Threading.Tasks;
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

        private readonly GroupForumManager forummanager;
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
        private readonly RentableSpaceManager _rentableSpaceManager;
        private readonly NuxUserGiftsManager _nuxusergiftManager;
        private readonly NuxUserGiftsListManager _nuxusergiftlistManager;
        private readonly LeaderBoardDataManager _leaderBoardDataManager;

        private bool _cycleEnded;
        private bool _cycleActive;
        private Task _gameCycle;
        private readonly int _cycleSleepTime = 25;

        public Game()
        {
            GetHallOfFame.GetInstance().Load();
            _packetManager = new PacketManager();
            _rentableSpaceManager = new RentableSpaceManager();
            _clientManager = new GameClientManager();
            _modManager = new ModerationManager();

            _itemDataManager = new ItemDataManager();
            _itemDataManager.Init();
            //this._cameraManager = new CameraPhotoManager();
            //this._cameraManager.Init(this._itemDataManager);
            _catalogManager = new CatalogManager();
            _catalogManager.Init(_itemDataManager);
            _frontpageManager = new FrontpageManager();

            _televisionManager = new TelevisionManager();
            _crackableManager = new CrackableManager();
            _crackableManager.Initialize(NeonEnvironment.GetDatabaseManager().GetQueryReactor());
            _furniMaticRewardsManager = new FurniMaticRewardsManager();
            _furniMaticRewardsManager.Initialize(NeonEnvironment.GetDatabaseManager().GetQueryReactor());

            _craftingManager = new CraftingManager();
            _craftingManager.Init();

            _navigatorManager = new NavigatorManager();
            _roomManager = new RoomManager();
            _chatManager = new ChatManager();
            _groupManager = new GroupManager();
            _questManager = new QuestManager();
            _achievementManager = new AchievementManager();
            _talentTrackManager = new TalentTrackManager();

            _landingViewManager = new LandingViewManager();
            _gameDataManager = new GameDataManager();

            _globalUpdater = new ServerStatusUpdater();
            _globalUpdater.Init();

            _languageLocale = new LanguageLocale();
            _antiMutant = new AntiMutant();
            _botManager = new BotManager();

            _cacheManager = new CacheManager();
            _rewardManager = new RewardManager();
            _musicManager = new SongManager();

            _badgeManager = new BadgeManager();
            _badgeManager.Init();

            forummanager = new GroupForumManager();

            _communityGoalVS = new CommunityGoalVS();
            _communityGoalVS.LoadCommunityGoalVS();

            _permissionManager = new PermissionManager();
            _permissionManager.Init();

            _subscriptionManager = new SubscriptionManager();
            _subscriptionManager.Init();

            HelperToolsManager.Init();

            _calendarManager = new CalendarManager();
            _calendarManager.Init();

            _leaderBoardDataManager = new LeaderBoardDataManager();

            _targetedoffersManager = new TargetedOffersManager();
            _targetedoffersManager.Initialize(NeonEnvironment.GetDatabaseManager().GetQueryReactor());

            _nuxusergiftManager = new NuxUserGiftsManager();
            _nuxusergiftManager.Initialize(NeonEnvironment.GetDatabaseManager().GetQueryReactor());

            _nuxusergiftlistManager = new NuxUserGiftsListManager();
            _nuxusergiftlistManager.Initialize(NeonEnvironment.GetDatabaseManager().GetQueryReactor());

            _pollManager = new PollManager();
            _pollManager.Init();
            WebSocketManager.StartListener();


        }

        public void StartGameLoop()
        {
            _gameCycle = new Task(GameCycle);
            _gameCycle.Start();

            _cycleActive = true;
        }

        private void GameCycle()
        {
            while (_cycleActive)
            {
                _cycleEnded = false;

                NeonEnvironment.GetGame().GetRoomManager().OnCycle();
                NeonEnvironment.GetGame().GetClientManager().OnCycle();
                //AlphaManager.getInstance().onCycle();
                _cycleEnded = true;
                Thread.Sleep(_cycleSleepTime);
            }
        }

        public void StopGameLoop()
        {
            _cycleActive = false;

            while (!_cycleEnded)
            {
                Thread.Sleep(_cycleSleepTime);
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
            return _frontpageManager;
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
            return _modManager;
        }

        public PermissionManager GetPermissionManager()
        {
            return _permissionManager;
        }

        public SubscriptionManager GetSubscriptionManager()
        {
            return _subscriptionManager;
        }

        public QuestManager GetQuestManager()
        {
            return _questManager;
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
            return _chatManager;
        }

        //public CameraPhotoManager GetCameraManager()
        //{
        //    return this._cameraManager;
        //}

        internal CrackableManager GetPinataManager()
        {
            return _crackableManager;
        }

        public CraftingManager GetCraftingManager()
        {
            return _craftingManager;
        }

        public FurniMaticRewardsManager GetFurniMaticRewardsMnager()
        {
            return _furniMaticRewardsManager;
        }

        public GameDataManager GetGameDataManager()
        {
            return _gameDataManager;
        }

        public LanguageLocale GetLanguageLocale()
        {
            return _languageLocale;
        }
        public AntiMutant GetAntiMutant()
        {
            return _antiMutant;
        }

        public BotManager GetBotManager()
        {
            return _botManager;
        }

        public CacheManager GetCacheManager()
        {
            return _cacheManager;
        }

        public RewardManager GetRewardManager()
        {
            return _rewardManager;
        }

        internal NuxUserGiftsManager GetNuxUserGiftsManager()
        {
            return _nuxusergiftManager;
        }

        internal LeaderBoardDataManager GetLeaderBoardDataManager()
        {
            return _leaderBoardDataManager;
        }

        internal NuxUserGiftsListManager GetNuxUserGiftsListManager()
        {
            return _nuxusergiftlistManager;
        }

        public BadgeManager GetBadgeManager()
        {
            return _badgeManager;
        }


        internal object GetFilterComponent()
        {
            throw new NotImplementedException();
        }

        public PollManager GetPollManager()
        {
            return _pollManager;
        }

        public CommunityGoalVS GetCommunityGoalVS()
        {
            return _communityGoalVS;
        }
    }
}