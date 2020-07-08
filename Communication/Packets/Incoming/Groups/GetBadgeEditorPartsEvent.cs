
using Neon.Communication.Packets.Outgoing.Groups;

namespace Neon.Communication.Packets.Incoming.Groups
{
    internal class GetBadgeEditorPartsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new BadgeEditorPartsComposer(
                NeonEnvironment.GetGame().GetGroupManager().Bases,
                NeonEnvironment.GetGame().GetGroupManager().Symbols,
                NeonEnvironment.GetGame().GetGroupManager().BaseColours,
                NeonEnvironment.GetGame().GetGroupManager().SymbolColours,
                NeonEnvironment.GetGame().GetGroupManager().BackGroundColours));

        }
    }
}
