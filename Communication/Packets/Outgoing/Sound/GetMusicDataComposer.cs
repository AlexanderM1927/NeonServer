using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Neon.HabboHotel.Rooms.Music;

namespace Neon.Communication.Packets.Outgoing.Rooms.Music
{
    class GetMusicDataComposer : ServerPacket
    {
        public GetMusicDataComposer(List<SongData> Songs)
            : base(ServerPacketHeader.GetMusicDataMessageComposer)
        {
            base.WriteInteger(Songs.Count);

            foreach (SongData Song in Songs)
            {
                base.WriteInteger(Song.Id);
                base.WriteString(Song.Name);
                base.WriteString(Song.Name.Replace("_", " "));
                base.WriteString(Song.Data);
                base.WriteInteger(Song.LengthMiliseconds);
                base.WriteString(Song.Artist);
            }
        }
    }
}
