using Neon.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace Neon.HabboHotel.Rooms.Music
{
    public class SongManager
    {

        private readonly Dictionary<int, SongData> songs;

        public SongManager()
        {
            songs = new Dictionary<int, SongData>();

            Init();
        }

        public void Init()
        {
            if (songs.Count > 0)
            {
                songs.Clear();
            }

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM jukebox_songs_data");
                DataTable dTable = dbClient.getTable();

                foreach (DataRow dRow in dTable.Rows)
                {
                    SongData song = new SongData(Convert.ToInt32(dRow["id"]), Convert.ToString(dRow["name"]), Convert.ToString(dRow["artist"]), Convert.ToString(dRow["song_data"]), Convert.ToDouble(dRow["length"]));
                    songs.Add(song.Id, song);
                }
            }
        }

        public SongData GetSong(int SongId)
        {

            songs.TryGetValue(SongId, out SongData song);

            return song;
        }
    }
}