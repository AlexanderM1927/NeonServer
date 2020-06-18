using Neon.HabboHotel.Rooms.Polls;
using Neon.Communication.Packets.Outgoing.Rooms.Polls;

namespace Neon.Communication.Packets.Incoming.Rooms.Polls
{
    class PollStartEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient session, ClientPacket packet)
        {
            int pollId = packet.PopInt();

            RoomPoll poll = null;
            if (!NeonEnvironment.GetGame().GetPollManager().TryGetPoll(pollId, out poll))
                return;

            session.SendMessage(new PollContentsComposer(poll));
        }
    }
}
