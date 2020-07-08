namespace Neon.Communication.Packets.Outgoing.Rooms.Furni
{
    internal class MysticBoxCloseComposer : ServerPacket
    {
        public MysticBoxCloseComposer()
            : base(ServerPacketHeader.MysticBoxCloseComposer)
        {
        }
    }
}