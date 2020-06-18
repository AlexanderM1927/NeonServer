//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Neon.HabboHotel.GameClients;
//using Neon.Communication.Packets.Outgoing.LandingView;

//namespace Neon.Communication.Packets.Incoming.Quests
//{
//    class GetDailyQuestEvent : IPacketEvent
//    {
//        public void Parse(GameClient Session, ClientPacket Packet)
//        {
//            int UsersOnline = NeonEnvironment.GetGame().GetClientManager().Count;

//            Session.SendMessage(new ConcurrentUsersGoalProgressComposer(UsersOnline));
//        }
//    }
//}
