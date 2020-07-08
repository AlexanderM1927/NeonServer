﻿namespace Neon.HabboHotel.Navigator
{
    public class StaffPick
    {
        public int RoomId { get; set; }
        public string Image { get; set; }

        public StaffPick(int roomId, string image)
        {
            RoomId = roomId;
            Image = image;
        }
    }
}
