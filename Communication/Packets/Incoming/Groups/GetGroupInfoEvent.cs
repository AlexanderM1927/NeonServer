using Neon.Communication.Packets.Outgoing.Groups;
using Neon.HabboHotel.Groups;

namespace Neon.Communication.Packets.Incoming.Groups
{
    internal class GetGroupInfoEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GroupId = Packet.PopInt();
            bool NewWindow = Packet.PopBoolean();

            if (!NeonEnvironment.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group Group))
            {
                return;
            }

            Session.SendMessage(new GroupInfoComposer(Group, Session, NewWindow));
        }
    }
}
