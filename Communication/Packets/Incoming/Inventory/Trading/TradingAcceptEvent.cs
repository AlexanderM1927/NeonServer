﻿
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Rooms.Trading;

namespace Neon.Communication.Packets.Incoming.Inventory.Trading
{
    internal class TradingAcceptEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().InRoom)
            {
                return;
            }


            if (!NeonEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room Room))
            {
                return;
            }

            if (!Room.CanTradeInRoom)
            {
                return;
            }

            Trade Trade = Room.GetUserTrade(Session.GetHabbo().Id);
            if (Trade == null)
            {
                return;
            }

            Trade.Accept(Session.GetHabbo().Id);
        }
    }
}