using Neon.HabboHotel.GameClients;
using Neon.Communication.Packets.Outgoing.Help.Helpers;
using Neon.HabboHotel.Helpers;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Neon.Communication.Packets.Incoming.Help.Helpers
{
    class HandleHelperToolEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session.GetHabbo().Rank > 5 || Session.GetHabbo()._guidelevel > 0)
            {

                var onDuty = Packet.PopBoolean();
                var isGuide = Packet.PopBoolean();
                var isHelper = Packet.PopBoolean();
                var isGuardian = Packet.PopBoolean();
                if (onDuty)
                    HelperToolsManager.AddHelper(Session, isHelper, isGuardian, isGuide);
                else
                    HelperToolsManager.RemoveHelper(Session);
                Session.SendMessage(new HandleHelperToolComposer(onDuty));
            }
            else
            {
                Session.SendMessage(new RoomNotificationComposer("Error al cargar las herramientas alfa:", "Por razones desconocidas has tenido un error al tratar de iniciar las herramientas de alfa, contacta con un administrador.", "", "Ok", "event:close"));
            }

        }
    }
}
