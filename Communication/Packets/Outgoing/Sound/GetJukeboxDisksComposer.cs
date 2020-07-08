using Neon.HabboHotel.Items;
using Neon.HabboHotel.Rooms.Music;
using System.Collections.Generic;
using System.Linq;

namespace Neon.Communication.Packets.Outgoing.Rooms.Music
{
    internal class GetJukeboxDisksComposer : ServerPacket
    {
        public GetJukeboxDisksComposer(Dictionary<int, Item> songs)
            : base(ServerPacketHeader.GetJukeboxDisksMessageComposer)
        {
            base.WriteInteger(songs.Count);

            foreach (Item userItem in songs.Values.ToList())
            {
                int songID = int.Parse(userItem.ExtraData);
                SongData Data = NeonEnvironment.GetGame().GetMusicManager().GetSong(songID);

                base.WriteInteger(userItem.Id);
                base.WriteInteger(Data.Id);
            }
        }
    }
}
