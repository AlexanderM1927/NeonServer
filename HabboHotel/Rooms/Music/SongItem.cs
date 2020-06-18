using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Users;
using Neon.Communication.Packets.Incoming;
using System.Collections.Concurrent;

using Neon.Database.Interfaces;
using log4net;
using Neon.HabboHotel.Items;

namespace Neon.HabboHotel.Rooms.Music
{
    public class SongItem
    {
        public readonly ItemData baseItem;
        public readonly int itemID;
        public readonly int songID;

        public SongItem(int itemID, int songID, int baseItem)
        {
            this.itemID = itemID;
            this.songID = songID;
            this.baseItem = null;
            NeonEnvironment.GetGame().GetItemManager().GetItem(baseItem, out this.baseItem);
        }

        public SongItem(Item item)
        {
            itemID = item.Id;
            songID = int.Parse(item.ExtraData);
            baseItem = item.Data;
        }

        public Item ToUserItem(Habbo Habbo)
        {
            return ItemFactory.CreateSingleItemNullable(baseItem, Habbo, songID.ToString(), "", 0, 0, 0);
        }

        public void SaveToDatabase(int roomID)
        {
            Room Room = null;
            if (NeonEnvironment.GetGame().GetRoomManager().TryGetRoom(roomID, out Room))
            {
                Item Jukebox = Room.GetRoomMusicManager().LinkedItem;
                if (Jukebox != null)
                {
                    using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                        dbClient.RunQuery("INSERT INTO room_items_songs (itemid, roomid, jukeboxid, songid) VALUES (" + itemID + "," + roomID + "," + Jukebox.Id + "," + songID + ")");
                }
            }
        }

        public void RemoveFromDatabase()
        {
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                dbClient.RunQuery("DELETE FROM room_items_songs WHERE itemid = " + itemID);
        }
    }
}