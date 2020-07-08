using Neon.Communication.Packets.Outgoing.Talents;
using Neon.HabboHotel.Talents;
using System.Collections.Generic;

namespace Neon.Communication.Packets.Incoming.Talents
{
    internal class GetTalentTrackEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            string Type = Packet.PopString();

            ICollection<TalentTrackLevel> Levels = NeonEnvironment.GetGame().GetTalentTrackManager().GetLevels();

            Session.SendMessage(new TalentTrackComposer(Levels, Type, Session));
        }
    }
}
