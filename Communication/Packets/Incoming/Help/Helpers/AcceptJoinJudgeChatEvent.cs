using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Helpers;
using Neon.Communication.Packets.Outgoing.Help;
using Neon.Communication.Packets.Outgoing;

namespace Neon.Communication.Packets.Incoming.Help.Helpers
{
    class AcceptJoinJudgeChatEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            //bool request = Packet.PopBoolean();

            //switch(request)
            //{
            //    case true:
            //        var response = new ServerPacket(ServerPacketHeader.GuardianSendChatCaseMessageComposer);
            //        response.WriteInteger(60);
            //        response.WriteString("");
            //        Session.SendMessage(response);
            //        break;
            //    case false:

            //        break;
            //}
        }
    }
}
