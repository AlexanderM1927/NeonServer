using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neon.Communication.Packets.Incoming.Help.Helpers
{
    class CloseHelperChatSessionEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var Element = HelperToolsManager.GetElement(Session);

            if (Element != null)
            {
                Element.End();
                if (Element.OtherElement != null)
                    Element.OtherElement.End();
            }

            if(Session.GetHabbo().OnHelperDuty)
            {
                NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_GuideTourGiver", 1);
            }
        }
    }
}
