using Neon.HabboHotel.GameClients;

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