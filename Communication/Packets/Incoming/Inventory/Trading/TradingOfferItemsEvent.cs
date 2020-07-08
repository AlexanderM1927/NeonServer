using Neon.HabboHotel.Items;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Rooms.Trading;
using System.Collections.Generic;
using System.Linq;

namespace Neon.Communication.Packets.Incoming.Inventory.Trading
{
    internal class TradingOfferItemsEvent : IPacketEvent
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

            int Amount = Packet.PopInt();

            Item Item = Session.GetHabbo().GetInventoryComponent().GetItem(Packet.PopInt());
            if (Item == null)
            {
                return;
            }

            List<Item> AllItems = Session.GetHabbo().GetInventoryComponent().GetItems.Where(x => x.Data.Id == Item.Data.Id).Take(Amount).ToList();
            foreach (Item I in AllItems)
            {
                Trade.OfferItem(Session.GetHabbo().Id, I);
            }
        }
    }
}