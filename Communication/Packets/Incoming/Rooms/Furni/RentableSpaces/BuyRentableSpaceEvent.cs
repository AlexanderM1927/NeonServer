using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.Communication.Packets.Outgoing.Rooms.Furni.RentableSpaces;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Items;
using Neon.HabboHotel.Items.RentableSpaces;

namespace Neon.Communication.Packets.Incoming.Rooms.Furni.RentableSpaces
{
    class BuyRentableSpaceEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {

            int itemId = Packet.PopInt();

            Room room;
            if (!NeonEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out room))
                return;

            if (room == null || room.GetRoomItemHandler() == null)
                return;

            RentableSpaceItem rsi;
            if (NeonEnvironment.GetGame().GetRentableSpaceManager().GetRentableSpaceItem(itemId, out rsi))
            {
                NeonEnvironment.GetGame().GetRentableSpaceManager().ConfirmBuy(Session, rsi, 3600);
            }


        }
    }
}