namespace Neon.Communication.Packets.Outgoing.Help.Helpers
{
    internal class EndHelperSessionComposer : ServerPacket
    {
        public EndHelperSessionComposer(int closeCode = 0)
            : base(ServerPacketHeader.EndHelperSessionMessageComposer)
        {
            base.WriteInteger(closeCode);
        }
    }
}
