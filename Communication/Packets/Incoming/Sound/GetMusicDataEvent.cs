using Neon.Communication.Packets.Outgoing.Rooms.Music;
using Neon.HabboHotel.Rooms.Music;
using System.Collections.Generic;

namespace Neon.Communication.Packets.Incoming.Rooms.Music
{
    internal class GetMusicDataEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int Songs = Packet.PopInt();

            List<SongData> SongData = new List<SongData>();

            for (int i = 0; i < Songs; i++)
            {
                int Pint = Packet.PopInt();
                SongData item = NeonEnvironment.GetGame().GetMusicManager().GetSong(Pint);

                if (item != null)
                {
                    SongData.Add(item);
                }
            }

            Session.SendMessage(new GetMusicDataComposer(SongData));
        }
    }
}
