using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Helpers;

namespace Neon.Communication.Packets.Incoming.Help.Helpers
{
    class InvinteHelperUserSessionEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var Element = HelperToolsManager.GetElement(Session);
            var room = Session.GetHabbo().CurrentRoom;
            if (room == null)
                return;

            Element.OtherElement.Session.SendMessage(new Outgoing.Help.Helpers.HelperSessionInvinteRoomComposer(room.Id, room.Name));
            Session.SendMessage(new Outgoing.Help.Helpers.HelperSessionInvinteRoomComposer(room.Id, room.Name));
        }
    }
}
