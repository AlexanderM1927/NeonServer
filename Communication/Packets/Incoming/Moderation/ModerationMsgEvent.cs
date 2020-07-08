using Neon.HabboHotel.GameClients;

namespace Neon.Communication.Packets.Incoming.Moderation
{
    internal class ModerationMsgEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().GetPermissions().HasRight("mod_alert"))
            {
                return;
            }

            int UserId = Packet.PopInt();
            string Message = Packet.PopString();

            GameClient Client = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
            if (Client == null)
            {
                return;
            }

            Client.SendNotification(Message);
        }
    }
}
