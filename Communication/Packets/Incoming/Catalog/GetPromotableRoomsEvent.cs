using Neon.Communication.Packets.Outgoing.Catalog;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Rooms;
using System.Collections.Generic;
using System.Linq;

namespace Neon.Communication.Packets.Incoming.Catalog
{
    internal class GetPromotableRoomsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            List<RoomData> Rooms = Session.GetHabbo().UsersRooms;
            Rooms = Rooms.Where(x => (x.Promotion == null || x.Promotion.TimestampExpires < NeonEnvironment.GetUnixTimestamp())).ToList();
            Session.SendMessage(new PromotableRoomsComposer(Rooms));
        }
    }
}
