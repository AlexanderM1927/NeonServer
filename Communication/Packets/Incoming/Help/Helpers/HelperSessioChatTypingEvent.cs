using Neon.HabboHotel.GameClients;

namespace Neon.Communication.Packets.Incoming.Help.Helpers
{
    internal class HelperSessioChatTypingEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            HabboHotel.Helpers.IHelperElement element = HabboHotel.Helpers.HelperToolsManager.GetElement(Session);
            if (element != null && element.OtherElement != null)
            {
                element.OtherElement.Session.SendMessage(new Outgoing.Help.Helpers.HelperSessionChatIsTypingComposer(Packet.PopBoolean()));
            }
        }
    }
}
