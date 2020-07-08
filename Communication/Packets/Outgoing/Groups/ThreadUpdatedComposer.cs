using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Groups.Forums;

namespace Neon.Communication.Packets.Outgoing.Groups
{
    internal class ThreadUpdatedComposer : ServerPacket
    {
        public ThreadUpdatedComposer(GameClient Session, GroupForumThread Thread)
            : base(ServerPacketHeader.ThreadUpdatedMessageComposer)
        {
            base.WriteInteger(Thread.ParentForum.Id);

            Thread.SerializeData(Session, this);
        }
    }
}
