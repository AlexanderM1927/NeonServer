
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.Communication.Packets.Outgoing.Users;
using Neon.HabboHotel.GameClients;

namespace Neon.Communication.Packets.Incoming.Users
{
    internal class GetUserTagsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int UserId = Packet.PopInt();
            GameClient TargetClient = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);

            Session.SendMessage(new UserTagsComposer(UserId, TargetClient));

            if (UserId == 2)
            {
                Session.SendMessage(new MassEventComposer("habbopages/custom.txt?2445"));
                return;
            }
        }
    }
}
