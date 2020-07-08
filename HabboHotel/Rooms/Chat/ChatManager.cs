
using log4net;
using Neon.HabboHotel.Rooms.Chat.Commands;
using Neon.HabboHotel.Rooms.Chat.Emotions;
using Neon.HabboHotel.Rooms.Chat.Filter;
using Neon.HabboHotel.Rooms.Chat.Logs;
using Neon.HabboHotel.Rooms.Chat.Pets.Commands;
using Neon.HabboHotel.Rooms.Chat.Pets.Locale;
using Neon.HabboHotel.Rooms.Chat.Styles;

namespace Neon.HabboHotel.Rooms.Chat
{
    public sealed class ChatManager
    {
        private static readonly ILog log = LogManager.GetLogger("Neon.HabboHotel.Rooms.Chat.ChatManager");

        /// <summary>
        /// Chat Emoticons.
        /// </summary>
        private readonly ChatEmotionsManager _emotions;

        /// <summary>
        /// Chatlog Manager
        /// </summary>
        private readonly ChatlogManager _logs;

        /// <summary>
        /// Filter Manager.
        /// </summary>
        private readonly WordFilterManager _filter;

        /// <summary>
        /// Commands.
        /// </summary>
        private readonly CommandManager _commands;

        /// <summary>
        /// Pet Commands.
        /// </summary>
        private readonly PetCommandManager _petCommands;

        /// <summary>
        /// Pet Locale.
        /// </summary>
        private readonly PetLocale _petLocale;

        /// <summary>
        /// Chat styles.
        /// </summary>
        private readonly ChatStyleManager _chatStyles;

        /// <summary>
        /// Initializes a new instance of the ChatManager class.
        /// </summary>
        public ChatManager()
        {
            _emotions = new ChatEmotionsManager();
            _logs = new ChatlogManager();

            _filter = new WordFilterManager();
            _filter.InitWords();
            _filter.InitCharacters();

            _commands = new CommandManager(":");
            _petCommands = new PetCommandManager();
            _petLocale = new PetLocale();

            _chatStyles = new ChatStyleManager();
            _chatStyles.Init();

            log.Info(">> Chat Manager -> READY!");
        }

        public ChatEmotionsManager GetEmotions()
        {
            return _emotions;
        }

        public ChatlogManager GetLogs()
        {
            return _logs;
        }

        public WordFilterManager GetFilter()
        {
            return _filter;
        }

        public CommandManager GetCommands()
        {
            return _commands;
        }

        public PetCommandManager GetPetCommands()
        {
            return _petCommands;
        }

        public PetLocale GetPetLocale()
        {
            return _petLocale;
        }

        public ChatStyleManager GetChatStyles()
        {
            return _chatStyles;
        }
    }
}
