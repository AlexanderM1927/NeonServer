namespace Neon.Communication.Packets.Incoming.Quests
{
    internal class StartQuestEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int QuestId = Packet.PopInt();

            NeonEnvironment.GetGame().GetQuestManager().ActivateQuest(Session, QuestId);
        }
    }
}
