namespace Neon.Communication.Packets.Outgoing.LandingView
{
    internal class ConcurrentUsersGoalProgressComposer : ServerPacket
    {
        public ConcurrentUsersGoalProgressComposer(int UsersNow, int type, int goal)
            : base(ServerPacketHeader.ConcurrentUsersGoalProgressMessageComposer)
        {
            base.WriteInteger(type);
            base.WriteInteger(UsersNow);
            base.WriteInteger(goal);
        }
    }
}
