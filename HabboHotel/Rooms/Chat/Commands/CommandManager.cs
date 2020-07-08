using Neon.Communication.Packets.Outgoing.Notifications;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.Database.Interfaces;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Items.Wired;
using Neon.HabboHotel.Rooms.Chat.Commands.Administrator;
using Neon.HabboHotel.Rooms.Chat.Commands.Events;
using Neon.HabboHotel.Rooms.Chat.Commands.Moderator;
using Neon.HabboHotel.Rooms.Chat.Commands.Moderator.Fun;
using Neon.HabboHotel.Rooms.Chat.Commands.Staff;
using Neon.HabboHotel.Rooms.Chat.Commands.User;
using Neon.HabboHotel.Rooms.Chat.Commands.User.Fan;
using Neon.HabboHotel.Rooms.Chat.Commands.User.Fun;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neon.HabboHotel.Rooms.Chat.Commands
{
    public class CommandManager
    {
        /// <summary>
        /// Command Prefix only applies to custom commands.
        /// </summary>
        private readonly string _prefix = ":";

        /// <summary>
        /// Commands registered for use.
        /// </summary>
        private readonly Dictionary<string, IChatCommand> _commands;

        /// <summary>
        /// The default initializer for the CommandManager
        /// </summary>
        public CommandManager(string Prefix)
        {
            _prefix = Prefix;
            _commands = new Dictionary<string, IChatCommand>();

            RegisterVIP();
            RegisterUser();
            RegisterEvents();
            RegisterModerator();
            RegisterAdministrator();
        }

        /// <summary>
        /// Request the text to parse and check for commands that need to be executed.
        /// </summary>
        /// <param name="Session">Session calling this method.</param>
        /// <param name="Message">The message to parse.</param>
        /// <returns>True if parsed or false if not.</returns>
        public bool Parse(GameClient Session, string Message)
        {

            if (Session == null || Session.GetHabbo() == null || Session.GetHabbo().CurrentRoom == null || NeonStaticGameSettings.IsGoingToBeClose)
            {
                return false;
            }

            if (!Message.StartsWith(_prefix))
            {
                return false;
            }

            Room room = Session.GetHabbo().CurrentRoom;

            if (room.GetFilter().CheckCommandFilter(Message))
            {
                return false;
            }

            if (Message == _prefix + "commands" || Message == _prefix + "comandos")
            {
                StringBuilder List = new StringBuilder();
                List.Append("LISTA DE COMANDOS DISPONIBLES\n\n");
                foreach (KeyValuePair<string, IChatCommand> CmdList in _commands.ToList())
                {
                    if (!string.IsNullOrEmpty(CmdList.Value.PermissionRequired))
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand(CmdList.Value.PermissionRequired))
                        {
                            continue;
                        }
                    }

                    List.Append("- :" + CmdList.Key + " " + CmdList.Value.Parameters + " > " + CmdList.Value.Description + "\n");
                }
                Session.SendMessage(new MOTDNotificationComposer(List.ToString()));
                //int Rank = Session.GetHabbo().Rank;

                //switch(Rank)
                //{
                //    case 1:
                //        Session.SendMessage(new MassEventComposer("habbopages/chat/userscommands.txt"));
                //        break;
                //    case 2:
                //        Session.SendMessage(new MassEventComposer("habbopages/chat/vipcommands.txt"));
                //        break;
                //}
                return true;
            }

            Message = Message.Substring(1);
            string[] Split = Message.Split(' ');

            if (Split.Length == 0)
            {
                return false;
            }

            if (_commands.TryGetValue(Split[0].ToLower(), out IChatCommand Cmd))
            {
                if (Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
                {
                    LogCommand(Session.GetHabbo().Id, Message, Session.GetHabbo().MachineId, Session.GetHabbo().Username);
                }

                if (!string.IsNullOrEmpty(Cmd.PermissionRequired))
                {
                    if (!Session.GetHabbo().GetPermissions().HasCommand(Cmd.PermissionRequired))
                    {
                        return false;
                    }
                }


                Session.GetHabbo().IChatCommand = Cmd;
                Session.GetHabbo().CurrentRoom.GetWired().TriggerEvent(WiredBoxType.TriggerUserSays, Session.GetHabbo(), this);
                Session.GetHabbo().CurrentRoom.GetWired().TriggerEvent(WiredBoxType.TriggerUserSaysCommand, Session.GetHabbo(), this);

                Cmd.Execute(Session, Session.GetHabbo().CurrentRoom, Split);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Registers the VIP set of commands.
        /// </summary>
        private void RegisterVIP()
        {
            Register("spull", new SuperPullCommand());
        }

        /// <summary>
        /// Registers the Events set of commands.
        /// </summary>
        private void RegisterEvents()
        {
            Register("eha", new EventAlertCommand());
            Register("eventalert", new EventAlertCommand());
            Register("special", new SpecialEvent());
            Register("cata", new CatalogUpdateAlert());
            Register("dados", new DiceAlertCommand());
        }

        /// <summary>
        /// Registers the default set of commands.
        /// </summary>
        private void RegisterUser()
        {
            Register("hidewired", new HideWiredCommand());
            Register("random", new RandomizeCommand());
            Register("eventtype", new EventSwapCommand());
            Register("changelog", new ChangelogCommand());
            Register("handitem", new CarryCommand());
            Register("setbet", new SetBetCommand());
            Register("bubblebot", new BubbleBotCommand());
            Register("chatdegrupo", new GroepChatCommand());
            Register("disablespam", new DisableSpamCommand());
            Register("build", new BuildCommand());
            Register("about", new InfoCommand());
            Register("vipstatus", new ViewVIPStatusCommand());
            Register("builder", new Builder());
            Register("color", new ColourCommand());
            Register("info", new InfoCommand());
            Register("precios", new PriceList());
            Register("custom", new CustomLegit());
            Register("pickall", new PickAllCommand());
            Register("ejectall", new EjectAllCommand());
            Register("lay", new LayCommand());
            Register("sit", new SitCommand());
            Register("ayuda", new HelpCommand());
            Register("prefix", new PrefixCommand());
            Register("help", new HelpCommand());
            Register("tour", new HelpCommand());
            Register("stand", new StandCommand());
            Register("mutepets", new MutePetsCommand());
            Register("mutebots", new MuteBotsCommand());
            Register("buyroom", new BuyRoomCommand());
            Register("sellroom", new SellRoomCommand());
            Register("mimic", new MimicCommand());
            Register("dance", new DanceCommand());
            Register("push", new PushCommand());
            Register("pull", new PullCommand());
            Register("golpe", new GolpeCommand());
            Register("curar", new CurarCommand());
            Register("enable", new EnableCommand());
            Register("follow", new FollowCommand());
            Register("faceless", new FacelessCommand());
            Register("moonwalk", new MoonwalkCommand());
            Register("variables", new WiredVariable());
            Register("paja", new PajaCommand());
            Register("fumar", new FumarCommand());

            Register("unload", new UnloadCommand());
            Register("regenmaps", new RegenMaps());
            Register("empty", new EmptyItems());
            Register("setmax", new SetMaxCommand());
            Register("setspeed", new SetSpeedCommand());
            Register("disablediagonal", new DisableDiagonalCommand());
            Register("flagme", new FlagMeCommand());

            Register("stats", new StatsCommand());
            Register("estadisticas", new StatsCommand());
            Register("kickpets", new KickPetsCommand());
            Register("kickbots", new KickBotsCommand());

            Register("room", new RoomCommand());
            Register("dnd", new DNDCommand());
            Register("disablegifts", new DisableGiftsCommand());
            Register("convertcredits", new ConvertCreditsCommand());
            Register("disablewhispers", new DisableWhispersCommand());
            Register("disablemimic", new DisableMimicCommand()); ;

            Register("pet", new PetCommand());
            Register("spush", new SuperPushCommand());
            Register("superpush", new SuperPushCommand());
            Register("disablefriends", new DisableFriends());
            Register("enablefriends", new EnableFriends());
            Register("beso", new KissCommand());
            Register("reload", new Reloadcommand());
            Register("quemar", new BurnCommand());
            Register("chess", new SetChessGameCommand());
            Register("cd", new CloseDiceCommand());
            Register("sexo", new SexCommand());
            Register("pagar", new PayCommand());
            //Register("setz", new SetzCommand());
            Register("resetsc", new ResetScoreBoard());
            Register("emoji", new EmojiCommand());
        }

        /// <summary>
        /// Registers the moderator set of commands.
        /// </summary>
        private void RegisterModerator()
        {
            Register("ban", new BanCommand());
            Register("mip", new MIPCommand());
            Register("ipban", new IPBanCommand());

            Register("userinfo", new UserInfoCommand());
            Register("sa", new StaffAlertCommand());
            Register("ga", new GuideAlertCommand());
            Register("roomunmute", new RoomUnmuteCommand());
            Register("roommute", new RoomMuteCommand());
            Register("roombadge", new RoomBadgeCommand());
            Register("roomalert", new RoomAlertCommand());
            Register("roomkick", new RoomKickCommand());
            Register("usermute", new MuteCommand());
            Register("unmute", new UnmuteCommand());
            Register("massbadge", new MassBadgeCommand());
            Register("kick", new KickCommand());
            Register("ha", new HotelAlertCommand());
            Register("hal", new HALCommand());
            Register("give", new GiveCommand());
            Register("massgive", new MassGiveCommand());
            Register("roomgive", new RoomGiveCommand());
            Register("givebadge", new GiveBadgeCommand());
            Register("kill", new DisconnectCommand());
            Register("alert", new AlertCommand());
            Register("tradeban", new TradeBanCommand());

            Register("masspoll", new MassPollCommand());
            Register("poll", new PollCommand());
            Register("quizz", new IdolQuizCommand());
            Register("lastmsg", new LastMessagesCommand());
            Register("lastconsolemsg", new LastConsoleMessagesCommand());

            Register("teleport", new TeleportCommand());
            Register("summon", new SummonCommand());
            Register("override", new OverrideCommand());
            Register("massenable", new MassEnableCommand());
            Register("massdance", new MassDanceCommand());
            Register("freeze", new FreezeCommand());
            Register("unfreeze", new UnFreezeCommand());
            Register("fastwalk", new FastwalkCommand());
            Register("superfastwalk", new SuperFastwalkCommand());
            Register("coords", new CoordsCommand());
            Register("alleyesonme", new AllEyesOnMeCommand());
            Register("allaroundme", new AllAroundMeCommand());
            Register("forcesit", new ForceSitCommand());

            Register("ignorewhispers", new IgnoreWhispersCommand());
            Register("forced_effects", new DisableForcedFXCommand());

            Register("makesay", new MakeSayCommand());
            Register("flaguser", new FlagUserCommand());
            Register("filter", new FilterCommand());
            Register("publialert", new PubliAlert());
            Register("rank", new GiveRanksCommand());
            Register("premiar", new PremiarCommand());
        }

        /// <summary>
        /// Registers the administrator set of commands.
        /// </summary>
        private void RegisterAdministrator()
        {
            Register("forcebox", new ForceFurniMaticBoxCommand());
            Register("mw", new MultiwhisperModeCommand());
            Register("progress", new ProgressAchievementCommand());
            Register("control", new ControlCommand());
            Register("dice", new ForceDiceCommand());
            Register("developer", new DevelopperCommand());
            Register("ia", new SendGraphicAlertCommand());
            Register("iau", new SendImageToUserCommand());
            Register("viewevents", new ViewStaffEventListCommand());
            Register("addtag", new AddTagsToUserCommand());
            Register("givespecial", new GiveSpecialReward());
            Register("bubble", new BubbleCommand());
            Register("tpalerta", new AlertSwapCommand());
            Register("update", new UpdateCommand());
            Register("deletegroup", new DeleteGroupCommand());
            Register("carry", new CarryCommand());
            Register("goto", new GOTOCommand());
            Register("addpredesigned", new AddPredesignedCommand());
            Register("removepredesigned", new RemovePredesignedCommand());
            Register("radioalert", new DJAlert());
            Register("radio", new DJAlert());
            Register("dj", new DJAlert());
            Register("c", new TrollAlert());
            Register("summonall", new SummonAll());
            Register("ca", new CustomizedHotelAlert());
            Register("massevent", new MassiveEventCommand());
            Register("viewinventary", new ViewInventaryCommand());
            Register("makevip", new MakeVipCommand());
            Register("removebadge", new RemoveBadgeCommand());
            Register("staffinfo", new StaffInfo());
            Register("voucher", new VoucherCommand());
            Register("link", new LinkStaffCommand());
            Register("pbr", new PremiarBonusRare());
            Register("tpu", new TeleportUserCommand());
            Register("ivs", new InvisibleCommand());
        }

        /// <summary>
        /// Registers a Chat Command.
        /// </summary>
        /// <param name="CommandText">Text to type for this command.</param>
        /// <param name="Command">The command to execute.</param>
        public void Register(string CommandText, IChatCommand Command)
        {
            _commands.Add(CommandText, Command);
        }

        public static string MergeParams(string[] Params, int Start)
        {
            StringBuilder Merged = new StringBuilder();
            for (int i = Start; i < Params.Length; i++)
            {
                if (i > Start)
                {
                    Merged.Append(" ");
                }

                Merged.Append(Params[i]);
            }

            return Merged.ToString();
        }

        public void LogCommand(int UserId, string Data, string MachineId, string Username)
        {
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO `logs_client_staff` (`user_id`,`data_string`,`machine_id`, `timestamp`) VALUES (@UserId,@Data,@MachineId,@Timestamp)");
                dbClient.AddParameter("UserId", UserId);
                dbClient.AddParameter("Data", Data);
                dbClient.AddParameter("MachineId", MachineId);
                dbClient.AddParameter("Timestamp", NeonEnvironment.GetUnixTimestamp());
                dbClient.RunQuery();
            }

            if (Data == "regenmaps" || Data == "emoji" || Data.StartsWith("c") || Data == "sa" || Data == "ga" || UserId == 2)
            { return; }

            else
            {
                NeonEnvironment.GetGame().GetClientManager().ManagerAlert(RoomNotificationComposer.SendBubble("advice", "" + Username + "\n\nUsó el comando:\n:" + Data + ".", ""));
            }
        }

        public bool TryGetCommand(string Command, out IChatCommand IChatCommand)
        {
            return _commands.TryGetValue(Command, out IChatCommand);
        }
    }
}
