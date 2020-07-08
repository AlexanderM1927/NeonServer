namespace Neon.Communication.Packets.Outgoing.Talents
{
    internal class TalentTrackLevelComposer : ServerPacket
    {
        public TalentTrackLevelComposer(string type)
            : base(ServerPacketHeader.TalentTrackLevelMessageComposer)
        {
            base.WriteString(type);
            base.WriteInteger(4);
            base.WriteInteger(4);
        }
    }
}