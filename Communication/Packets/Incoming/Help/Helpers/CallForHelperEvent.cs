using Neon.Communication.Packets.Outgoing.Help.Helpers;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Helpers;
using System.Linq;

namespace Neon.Communication.Packets.Incoming.Help.Helpers
{
    internal class CallForHelperEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int category = Packet.PopInt();
            string message = Packet.PopString();

            HabboHotel.Helpers.HabboHelper helper = HelperToolsManager.GetHelper(Session);
            if (helper != null)
            {
                Session.SendNotification("TEST");
                Session.SendMessage(new CloseHelperSessionComposer());
                return;
            }

            HelperCase call = HelperToolsManager.AddCall(Session, message, category);
            HabboHotel.Helpers.HabboHelper helpers = HelperToolsManager.GetHelpersToCase(call).FirstOrDefault();

            if (helpers != null)
            {
                HelperToolsManager.InvinteHelpCall(helpers, call);
                Session.SendMessage(new CallForHelperWindowComposer(false, call));
                return;
            }

            Session.SendMessage(new CallForHelperErrorComposer(1));

        }
    }
}
