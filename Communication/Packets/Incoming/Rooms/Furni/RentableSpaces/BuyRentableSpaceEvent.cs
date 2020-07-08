using Neon.HabboHotel.Items.RentableSpaces;
using Neon.HabboHotel.Rooms;

namespace Neon.Communication.Packets.Incoming.Rooms.Furni.RentableSpaces
{
    internal class BuyRentableSpaceEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {

            int itemId = Packet.PopInt();

            if (!NeonEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room room))
            {
                return;
            }

            if (room == null || room.GetRoomItemHandler() == null)
            {
                return;
            }

            if (NeonEnvironment.GetGame().GetRentableSpaceManager().GetRentableSpaceItem(itemId, out RentableSpaceItem rsi))
            {
                NeonEnvironment.GetGame().GetRentableSpaceManager().ConfirmBuy(Session, rsi, 3600);
            }
        }
    }
}