using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.Talents;
using Neon.Communication.Packets.Outgoing.Talents;

namespace Neon.Communication.Packets.Incoming.Talents
{
    class GetTalentTrackEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            string Type = Packet.PopString();

            ICollection<TalentTrackLevel> Levels = NeonEnvironment.GetGame().GetTalentTrackManager().GetLevels();

            Session.SendMessage(new TalentTrackComposer(Levels, Type, Session));
        }
    }
}
