using Neon.Communication.Packets.Incoming.LandingView;

namespace Neon.Communication.Packets.Outgoing.LandingView
{
    internal class HallOfFameComposer : ServerPacket
    {
        public HallOfFameComposer() : base(ServerPacketHeader.UpdateHallOfFameListMessageComposer)
        {
            WriteString("halloffame.staff");
            GetHallOfFame.GetInstance().Serialize(this);
            return;
        }
    }
}