using System.Collections.Generic;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Quests;
using Neon.Communication.Packets.Incoming;

namespace Neon.Communication.Packets.Incoming.Quests
{
    public class GetQuestListEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            NeonEnvironment.GetGame().GetQuestManager().GetList(Session, null);
        }
    }
}