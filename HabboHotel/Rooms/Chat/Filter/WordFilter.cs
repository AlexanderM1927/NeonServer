namespace Neon.HabboHotel.Rooms.Chat.Filter
{
    internal sealed class WordFilter
    {
        private readonly string _word;
        private readonly string _replacement;
        private readonly bool _strict;
        private readonly bool _bannable;

        public WordFilter(string Word, string Replacement, bool Strict, bool Bannable)
        {
            _word = Word;
            _replacement = Replacement;
            _strict = Strict;
            _bannable = Bannable;
        }

        public string Word => _word;

        public string Replacement => _replacement;
        public bool IsStrict => _strict;
        public bool IsBannable => _bannable;
    }
}
