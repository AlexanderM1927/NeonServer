﻿using Neon.HabboHotel.Rooms;
using System.Collections.Generic;

namespace Neon.Communication.Packets.Outgoing.Catalog
{
    internal class GetCatalogRoomPromotionComposer : ServerPacket
    {
        public GetCatalogRoomPromotionComposer(List<RoomData> UsersRooms)
            : base(ServerPacketHeader.PromotableRoomsMessageComposer)
        {
            base.WriteBoolean(true);//wat
            base.WriteInteger(UsersRooms.Count);//Count of rooms?
            foreach (RoomData Room in UsersRooms)
            {
                base.WriteInteger(Room.Id);
                base.WriteString(Room.Name);
                base.WriteBoolean(true);
            }
        }
    }
}
