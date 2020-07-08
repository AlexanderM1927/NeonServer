using Neon.Communication.Packets.Outgoing.Rooms.Action;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Users;

namespace Neon.Communication.Packets.Incoming.Rooms.Action
{
    internal class UnIgnoreUserEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
            {
                return;
            }

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
            {
                return;
            }

            string Username = Packet.PopString();

            Habbo User = NeonEnvironment.GetHabboByUsername(Username);
            if (User == null || !Session.GetHabbo().MutedUsers.Contains(User.Id))
            {
                return;
            }

            Session.GetHabbo().MutedUsers.Remove(User.Id);
            Session.SendMessage(new IgnoreStatusComposer(3, Username));
        }
    }
}
