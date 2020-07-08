using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Helpers;

namespace Neon.Communication.Packets.Incoming.Help.Helpers
{
    internal class InvinteHelperUserSessionEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            IHelperElement Element = HelperToolsManager.GetElement(Session);
            HabboHotel.Rooms.Room room = Session.GetHabbo().CurrentRoom;
            if (room == null)
            {
                return;
            }

            Element.OtherElement.Session.SendMessage(new Outgoing.Help.Helpers.HelperSessionInvinteRoomComposer(room.Id, room.Name));
            Session.SendMessage(new Outgoing.Help.Helpers.HelperSessionInvinteRoomComposer(room.Id, room.Name));
        }
    }
}
