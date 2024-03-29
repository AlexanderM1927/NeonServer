﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace Neon.HabboHotel.Rooms.Chat.Filter
{
    public sealed class WordFilterManager
    {
        // New filter system by Komok
        // All rights

        private readonly List<string> _filteredWords;
        private readonly List<WordFilterReplacements> _filterReplacements;

        public WordFilterManager()
        {
            _filteredWords = new List<string>();
            _filterReplacements = new List<WordFilterReplacements>();
        }

        public void InitWords()
        {
            if (_filteredWords.Count > 0)
            {
                _filteredWords.Clear();
            }

            DataTable Data = null;
            using (Database.Interfaces.IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT word FROM `wordfilter`");
                Data = dbClient.getTable();
                if (Data != null)
                {
                    foreach (DataRow Row in Data.Rows)
                    {
                        _filteredWords.Add(Convert.ToString(Row["word"]));
                    }
                }
            }
        }

        public void InitCharacters()
        {
            if (_filterReplacements.Count > 0)
            {
                _filterReplacements.Clear();
            }

            DataTable Data = null;
            using (Database.Interfaces.IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `wordfilter_characters`");
                Data = dbClient.getTable();
                if (Data != null)
                {
                    foreach (DataRow Row in Data.Rows)
                    {
                        _filterReplacements.Add(new WordFilterReplacements(Convert.ToString(Row["character"]),
                        Convert.ToString(Row["replacement"])));
                    }
                }
            }
        }

        public bool IsUnnaceptableWord(string str, out string output)
        {
            str = str.ToLower();
            foreach (WordFilterReplacements replacement in _filterReplacements.Select(word => word).Where(word => str.Contains(word.Character)))
            {
                str = str.Replace(replacement.Character, replacement.Replacement);
            }

            output = _filteredWords.FirstOrDefault(hotelWords => str.Contains(hotelWords.ToLower()));
            return !string.IsNullOrEmpty(output);
        }
    }

    public class WordFilterReplacements
    {
        public string Character;
        public string Replacement;

        public WordFilterReplacements(string character, string replacement)
        {
            Character = character;
            Replacement = replacement;
        }
    }
}

