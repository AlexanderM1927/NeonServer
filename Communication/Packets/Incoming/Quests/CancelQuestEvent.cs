namespace Neon.Communication.Packets.Incoming.Quests
{
    internal class CancelQuestEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            NeonEnvironment.GetGame().GetQuestManager().CancelQuest(Session, Packet);
        }
    }
}
