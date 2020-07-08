namespace Neon.Communication.Packets.Outgoing.HabboCamera
{
    internal class ThumbnailSuccessMessageComposer : ServerPacket
    {
        public ThumbnailSuccessMessageComposer()
            : base(ServerPacketHeader.ThumbnailSuccessMessageComposer)
        {

        }
    }
}
