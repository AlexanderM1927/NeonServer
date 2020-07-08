using Neon.Communication.Packets.Outgoing.Talents;

namespace Neon.Communication.Packets.Incoming.Talents
{
    internal class GetCitizenshipTypeEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
            {
                return;
            }

            string data = Packet.PopString();

            Session.SendMessage(new TalentTrackLevelComposer(data));

        }
    }
}
