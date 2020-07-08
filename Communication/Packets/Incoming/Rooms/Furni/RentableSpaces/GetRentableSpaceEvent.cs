
using Neon.Communication.Packets.Outgoing.Rooms.Furni.RentableSpaces;
using Neon.HabboHotel.Items;
using Neon.HabboHotel.Items.RentableSpaces;
using Neon.HabboHotel.Rooms;

namespace Neon.Communication.Packets.Incoming.Rooms.Furni.RentableSpaces
{
    internal class GetRentableSpaceEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int ItemId = Packet.PopInt();

            if (!NeonEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room room))
            {
                return;
            }

            Item item = room.GetRoomItemHandler().GetItem(ItemId);

            if (item == null)
            {
                return;
            }

            if (item.GetBaseItem() == null)
            {
                return;
            }

            if (item.GetBaseItem().InteractionType != InteractionType.RENTABLE_SPACE)
            {
                return;
            }

            if (!NeonEnvironment.GetGame().GetRentableSpaceManager().GetRentableSpaceItem(ItemId, out RentableSpaceItem _rentableSpace))
            {
                _rentableSpace = NeonEnvironment.GetGame().GetRentableSpaceManager().CreateAndAddItem(ItemId, Session);
            }

            if (_rentableSpace.Rented)
            {
                Session.SendMessage(new RentableSpaceComposer(_rentableSpace.OwnerId, _rentableSpace.OwnerUsername, _rentableSpace.GetExpireSeconds()));
            }
            else
            {
                int errorCode = NeonEnvironment.GetGame().GetRentableSpaceManager().GetButtonErrorCode(Session, _rentableSpace);
                Session.SendMessage(new RentableSpaceComposer(false, errorCode, -1, "", 0, _rentableSpace.Price));
            }
        }
    }
}