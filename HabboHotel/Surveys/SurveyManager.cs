﻿using Neon.Database.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Data;

namespace Neon.HabboHotel.Surveys
{
    internal class SurveyManager
    {
        private readonly ConcurrentDictionary<int, Question> _questions;

        public SurveyManager()
        {
            _questions = new ConcurrentDictionary<int, Question>();

            Init();
        }

        public void Init()
        {
            DataTable Table = null;
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `questions`");
                Table = dbClient.getTable();
            }

            if (Table != null)
            {
                foreach (DataRow Row in Table.Rows)
                {
                    if (!_questions.ContainsKey(Convert.ToInt32(Row["id"])))
                    {
                        _questions.TryAdd(Convert.ToInt32(Row["id"]), new Question());
                    }
                }
            }
        }

        public bool TryGetQuestion(int QuestionId, out Question Question)
        {
            return _questions.TryGetValue(QuestionId, out Question);
        }
    }
}
