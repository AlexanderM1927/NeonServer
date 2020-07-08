namespace Neon.Communication.Packets.Outgoing.Help
{
    internal class SubmitBullyReportComposer : ServerPacket
    {
        public SubmitBullyReportComposer(int Result)
            : base(ServerPacketHeader.SubmitBullyReportMessageComposer)
        {
            base.WriteInteger(Result);
        }
    }
}
