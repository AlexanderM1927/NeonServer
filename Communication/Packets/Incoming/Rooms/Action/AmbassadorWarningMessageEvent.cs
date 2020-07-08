using Neon.Communication.Packets.Outgoing.Moderation;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.Rooms;

namespace Neon.Communication.Packets.Incoming.Rooms.Action
{
    internal class AmbassadorWarningMessageEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {

            int UserId = Packet.PopInt();
            int RoomId = Packet.PopInt();
            int Time = Packet.PopInt();

            Room Room = Session.GetHabbo().CurrentRoom;
            RoomUser Target = Room.GetRoomUserManager().GetRoomUserByHabbo(NeonEnvironment.GetUsernameById(UserId));
            if (Target == null)
            {
                return;
            }

            long nowTime = NeonEnvironment.CurrentTimeMillis();
            long timeBetween = nowTime - Session.GetHabbo()._lastTimeUsedHelpCommand;
            if (timeBetween < 60000)
            {
                Session.SendMessage(RoomNotificationComposer.SendBubble("abuse", "Espera al menos 1 minuto para volver a enviar una alerta.", ""));
                return;
            }

            else
            {
                NeonEnvironment.GetGame().GetClientManager().StaffAlert(RoomNotificationComposer.SendBubble("advice", "" + Session.GetHabbo().Username + " acaba de mandarle una alerta embajador a " + Target.GetClient().GetHabbo().Username + ", pulsa aquí para ir a mirar.", "event:navigator/goto/" + Session.GetHabbo().CurrentRoomId));
            }

            Target.GetClient().SendMessage(new BroadcastMessageAlertComposer("<b><font size='15px' color='#c40101'>Mensaje de los Embajadores<br></font></b>Los Embajadores consideran que tu comportamiento no es el más adecuado. Por favor, reconsidera tu actitud, antes de que un Moderador tome medidas."));

            Session.GetHabbo()._lastTimeUsedHelpCommand = nowTime;
        }
    }
}
