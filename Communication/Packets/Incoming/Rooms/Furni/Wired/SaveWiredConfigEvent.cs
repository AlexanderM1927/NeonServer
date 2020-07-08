
using Neon.Communication.Packets.Outgoing.Rooms.Furni.Wired;
using Neon.HabboHotel.Items;
using Neon.HabboHotel.Items.Wired;
using Neon.HabboHotel.Rooms;

namespace Neon.Communication.Packets.Incoming.Rooms.Furni.Wired
{
    internal class SaveWiredConfigEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
            {
                return;
            }

            if (!Session.GetHabbo().InRoom)
            {
                return;
            }

            int ItemId = Packet.PopInt();

            Session.SendMessage(new HideWiredConfigComposer());

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
            {
                return;
            }

            if (!Room.CheckRights(Session, false) && !Room.CheckRights(Session, true))
            {
                return;
            }

            Item SelectedItem = Room.GetRoomItemHandler().GetItem(ItemId);
            if (SelectedItem == null)
            {
                return;
            }

            if (!Session.GetHabbo().CurrentRoom.GetWired().TryGet(ItemId, out IWiredItem Box))
            {
                return;
            }

            if (Box.Type == WiredBoxType.EffectGiveUserBadge && !Session.GetHabbo().GetPermissions().HasRight("room_item_wired_rewards"))
            {
                Session.SendNotification("Usted no tiene los suficientes permisos para utilizar este Wired");
                return;
            }

            Box.HandleSave(Packet);
            Session.GetHabbo().CurrentRoom.GetWired().SaveBox(Box);
        }
    }
}
