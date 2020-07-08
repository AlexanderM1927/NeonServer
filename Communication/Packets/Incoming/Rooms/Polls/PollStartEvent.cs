using Neon.Communication.Packets.Outgoing.Rooms.Polls;
using Neon.HabboHotel.Rooms.Polls;

namespace Neon.Communication.Packets.Incoming.Rooms.Polls
{
    internal class PollStartEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient session, ClientPacket packet)
        {
            int pollId = packet.PopInt();

            if (!NeonEnvironment.GetGame().GetPollManager().TryGetPoll(pollId, out RoomPoll poll))
            {
                return;
            }

            session.SendMessage(new PollContentsComposer(poll));
        }
    }
}
