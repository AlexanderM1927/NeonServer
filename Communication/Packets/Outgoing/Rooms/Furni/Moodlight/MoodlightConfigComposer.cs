
using Neon.HabboHotel.Items.Data.Moodlight;

namespace Neon.Communication.Packets.Outgoing.Rooms.Furni.Moodlight
{
    internal class MoodlightConfigComposer : ServerPacket
    {
        public MoodlightConfigComposer(MoodlightData MoodlightData)
            : base(ServerPacketHeader.MoodlightConfigMessageComposer)
        {
            base.WriteInteger(MoodlightData.Presets.Count);
            base.WriteInteger(MoodlightData.CurrentPreset);

            int i = 1;
            foreach (MoodlightPreset Preset in MoodlightData.Presets)
            {
                base.WriteInteger(i);
                base.WriteInteger(Preset.BackgroundOnly ? 2 : 1);
                base.WriteString(Preset.ColorCode);
                base.WriteInteger(Preset.ColorIntensity);
                i++;
            }
        }
    }
}