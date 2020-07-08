using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Helpers;

namespace Neon.Communication.Packets.Incoming.Help.Helpers
{
    internal class FinishHelperSessionEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            bool Voted = Packet.PopBoolean();
            IHelperElement Element = HelperToolsManager.GetElement(Session);
            if (Element is HelperCase)
            {
                if (Voted)
                {
                    Element.OtherElement.Session.SendMessage(RoomNotificationComposer.SendBubble("ambassador", "" + Element.OtherElement.Session.GetHabbo().Username + ", gracias por colaborar en el programa de Alfas, has atendido correctamente la duda del usuario.", ""));
                    //if (Element.OtherElement.Session.GetHabbo()._guidelevel >= 1)
                    //{
                    //    NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Element.OtherElement.Session, "ACH_GuideTourGiver", 1);
                    //}
                }
                else
                {
                    Element.OtherElement.Session.SendMessage(RoomNotificationComposer.SendBubble("ambassador", "" + Element.OtherElement.Session.GetHabbo().Username + ", gracias por colaborar en el programa de Alfas, has atendido satisfactoriamente la duda del usuario.", ""));
                }
            }

            Element.Close();
        }
    }
}
