namespace Neon.Communication.Packets.Outgoing.Handshake
{
    internal class VideoOffersRewardsComposer : ServerPacket
    {
        public VideoOffersRewardsComposer(/*int Id, string Type, string Message*/)
            : base(ServerPacketHeader.VideoOffersRewardsMessageComposer)
        {
            WriteString("start_video");
            WriteInteger(0);
            WriteString("");
            WriteString("");
        }
    }
}

