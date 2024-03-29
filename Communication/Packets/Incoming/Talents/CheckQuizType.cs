﻿using Neon.Communication.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace Neon.Communication.Packets.Incoming.Talents
{
    internal class CheckQuizType : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
            {
                return;
            }

            string HabboType = Packet.PopString();
            if (HabboType == "HabboWay1")
            {
                Session.GetHabbo()._HabboQuizQuestions = new List<int>(5);

                ServerPacket quiz = new ServerPacket(ServerPacketHeader.QuizDataMessageComposer);
                quiz.WriteString(HabboType);
                quiz.WriteInteger(5); // longitud.                
                for (int i = 0; i < 5; i++)
                {
                    int rndNumber = new Random().Next(10);
                    if (Session.GetHabbo()._HabboQuizQuestions.Contains(rndNumber))
                    {
                        for (int ii = 0; ii < 10; ii++)
                        {
                            if (!Session.GetHabbo()._HabboQuizQuestions.Contains(ii))
                            {
                                rndNumber = ii;
                                break;
                            }
                        }
                    }
                    Session.GetHabbo()._HabboQuizQuestions.Add(rndNumber);
                    quiz.WriteInteger(rndNumber);
                }
                Session.SendMessage(quiz);
            }
            else if (HabboType == "SafetyQuiz1")
            {
                NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_SafetyQuizGraduate", 1);
            }
        }
    }
}
