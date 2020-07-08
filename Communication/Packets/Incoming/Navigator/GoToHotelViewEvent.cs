
using Neon.HabboHotel.Rooms;

namespace Neon.Communication.Packets.Incoming.Navigator
{
    internal class GoToHotelViewEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
            {
                return;
            }

            if (Session.GetHabbo().InRoom)
            {

                if (!NeonEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room OldRoom))
                {
                    return;
                }

                //if(OldRoom.QueueingUsers.Contains(Session.GetHabbo()))
                //{
                //    OldRoom.QueueingUsers.Remove(OldRoom.QueueingUsers.First());
                //    foreach (Habbo user in OldRoom.QueueingUsers)
                //    {
                //        user.GetClient().SendMessage(new RoomQueueComposer(OldRoom.QueueingUsers.IndexOf(user)));
                //    }
                //}

                if (OldRoom.GetRoomUserManager() != null)
                {
                    OldRoom.GetRoomUserManager().RemoveUserFromRoom(Session, true, false);
                }
            }
        }
    }
}
