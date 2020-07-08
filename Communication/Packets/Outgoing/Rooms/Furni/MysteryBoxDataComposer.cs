using Neon.HabboHotel.GameClients;

namespace Neon.Communication.Packets.Outgoing.Rooms.Furni
{
    internal class MysteryBoxDataComposer : ServerPacket
    {
        public MysteryBoxDataComposer(GameClient Session)
            : base(ServerPacketHeader.MysteryBoxDataComposer)
        {
            foreach (string box in Session.GetHabbo().MysticBoxes.ToArray())
            {
                base.WriteString(box);
            }
            foreach (string key in Session.GetHabbo().MysticKeys.ToArray())
            {
                base.WriteString(key);
            }
        }
    }
}
