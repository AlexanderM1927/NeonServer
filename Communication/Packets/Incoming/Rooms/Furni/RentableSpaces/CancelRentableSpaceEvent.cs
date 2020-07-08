
using Neon.Communication.Packets.Outgoing.Rooms.Furni.RentableSpaces;
using Neon.HabboHotel.Items;
using Neon.HabboHotel.Items.RentableSpaces;
using Neon.HabboHotel.Rooms;

namespace Neon.Communication.Packets.Incoming.Rooms.Furni.RentableSpaces
{
    internal class CancelRentableSpaceEvent : IPacketEvent
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

            Item item = room.GetRoomItemHandler().GetItem(itemId);
            if (item == null)
            {
                return;
            }

            if (!NeonEnvironment.GetGame().GetRentableSpaceManager().GetRentableSpaceItem(itemId, out RentableSpaceItem _rentableSpace))
            {
                return;
            }

            int errorCode = NeonEnvironment.GetGame().GetRentableSpaceManager().GetCancelErrorCode(Session, _rentableSpace);

            if (errorCode > 0)
            {
                Session.SendMessage(new RentableSpaceComposer(_rentableSpace.IsRented(), errorCode, _rentableSpace.OwnerId, _rentableSpace.OwnerUsername, _rentableSpace.GetExpireSeconds(), _rentableSpace.Price));
                return;
            }


            if (!NeonEnvironment.GetGame().GetRentableSpaceManager().ConfirmCancel(Session, _rentableSpace))
            {
                Session.SendNotification("global.error");
                return;
            }

            Session.SendMessage(new RentableSpaceComposer(false, 0, 0, "", 0, _rentableSpace.Price));
        }
    }
}