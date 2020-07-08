
using Neon.Communication.Packets.Outgoing.Users;
using Neon.HabboHotel.Users;

namespace Neon.Communication.Packets.Incoming.Users
{
    internal class GetSelectedBadgesEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int UserId = Packet.PopInt();
            Habbo Habbo = NeonEnvironment.GetHabboById(UserId);
            if (Habbo == null)
            {
                return;
            }

            Session.GetHabbo().LastUserId = UserId;
            Session.SendMessage(new HabboUserBadgesComposer(Habbo));
        }
    }
}