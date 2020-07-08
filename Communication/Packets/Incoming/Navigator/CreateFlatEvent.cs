
using Neon.Communication.Packets.Outgoing.Navigator;
using Neon.HabboHotel.Navigator;
using Neon.HabboHotel.Rooms;

namespace Neon.Communication.Packets.Incoming.Navigator
{
    internal class CreateFlatEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
            {
                return;
            }

            if (Session.GetHabbo().UsersRooms.Count >= 500)
            {
                Session.SendMessage(new CanCreateRoomComposer(true, 500));
                return;
            }

            string Name = Packet.PopString();
            Name = NeonEnvironment.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(Name, out string word) ? "Spam" : Name;
            string Description = Packet.PopString();
            Description = NeonEnvironment.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(Description, out word) ? "Spam" : Description;
            string ModelName = Packet.PopString();

            int Category = Packet.PopInt();
            int MaxVisitors = Packet.PopInt();//10 = min, 25 = max.
            int TradeSettings = Packet.PopInt();//2 = All can trade, 1 = owner only, 0 = no trading.

            if (Name.Length < 3)
            {
                return;
            }

            if (Name.Length > 25)
            {
                return;
            }

            if (!NeonEnvironment.GetGame().GetRoomManager().TryGetModel(ModelName, out RoomModel RoomModel))
            {
                return;
            }

            if (!NeonEnvironment.GetGame().GetNavigator().TryGetSearchResultList(Category, out SearchResultList SearchResultList))
            {
                Category = 36;
            }

            if (SearchResultList.CategoryType != NavigatorCategoryType.CATEGORY || SearchResultList.RequiredRank > Session.GetHabbo().Rank)
            {
                Category = 36;
            }

            if (MaxVisitors < 10 || MaxVisitors > 25)
            {
                MaxVisitors = 10;
            }

            if (TradeSettings < 0 || TradeSettings > 2)
            {
                TradeSettings = 0;
            }

            RoomData NewRoom = NeonEnvironment.GetGame().GetRoomManager().CreateRoom(Session, Name, Description, ModelName, Category, MaxVisitors, TradeSettings);
            if (NewRoom != null)
            {
                Session.SendMessage(new FlatCreatedComposer(NewRoom.Id, Name));
            }
        }
    }
}
