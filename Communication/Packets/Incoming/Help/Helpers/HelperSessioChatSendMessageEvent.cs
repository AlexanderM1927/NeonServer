using Neon.Communication.Packets.Outgoing.Help.Helpers;
using Neon.Database.Interfaces;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Helpers;
using System;

namespace Neon.Communication.Packets.Incoming.Help.Helpers
{
    internal class HelperSessioChatSendMessageEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            IHelperElement Element = HelperToolsManager.GetElement(Session);
            string message = Packet.PopString();
            if (Element.OtherElement != null)
            {
                Session.SendMessage(new HelperSessionSendChatComposer(Session.GetHabbo().Id, message));
                Element.OtherElement.Session.SendMessage(new HelperSessionSendChatComposer(Session.GetHabbo().Id, message));
                LogHelper(Session.GetHabbo().Id, Element.OtherElement.Session.GetHabbo().Id, message);
            }
            else
            {
                Session.SendMessage(new CallForHelperErrorComposer(0));
            }
        }

        public void LogHelper(int From_Id, int ToId, string Message)
        {
            DateTime Now = DateTime.Now;
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO chatlogs_helper VALUES (NULL, " + From_Id + ", " + ToId + ", @message, UNIX_TIMESTAMP())");
                dbClient.AddParameter("message", Message);
                dbClient.RunQuery();
            }
        }
    }
}
