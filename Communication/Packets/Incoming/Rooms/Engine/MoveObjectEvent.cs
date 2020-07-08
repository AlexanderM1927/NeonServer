
using Neon.Communication.Packets.Outgoing.Rooms.Engine;
using Neon.HabboHotel.Items;
using Neon.HabboHotel.Quests;
using Neon.HabboHotel.Rooms;



namespace Neon.Communication.Packets.Incoming.Rooms.Engine
{
    internal class MoveObjectEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
            {
                return;
            }

            if (Session.GetHabbo().Rank > 8 && !Session.GetHabbo().StaffOk || NeonStaticGameSettings.IsGoingToBeClose)
            {
                return;
            }

            int ItemId = Packet.PopInt();
            if (ItemId == 0)
            {
                return;
            }


            if (!NeonEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room Room))
            {
                return;
            }

            Item Item;
            if (Room.Group != null)
            {
                if (!Room.CheckRights(Session, false, true))
                {
                    Item = Room.GetRoomItemHandler().GetItem(ItemId);
                    if (Item == null)
                    {
                        return;
                    }

                    Session.SendMessage(new ObjectUpdateComposer(Item, Room.OwnerId));
                    return;
                }
            }
            else
            {
                if (!Room.CheckRights(Session))
                {
                    return;
                }
            }

            Item = Room.GetRoomItemHandler().GetItem(ItemId);

            if (Item == null)
            {
                return;
            }

            int x = Packet.PopInt();
            int y = Packet.PopInt();
            int Rotation = Packet.PopInt();

            if (x != Item.GetX || y != Item.GetY)
            {
                NeonEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.FURNI_MOVE);
            }

            if (Rotation != Item.Rotation)
            {
                NeonEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.FURNI_ROTATE);
            }

            if (!Room.GetRoomItemHandler().SetFloorItem(Session, Item, x, y, Rotation, false, false, true))
            {
                Room.SendMessage(new ObjectUpdateComposer(Item, Room.OwnerId));
                return;
            }

            if (Item.GetZ >= 0.1)
            {
                NeonEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.FURNI_STACK);
            }
        }
    }
}