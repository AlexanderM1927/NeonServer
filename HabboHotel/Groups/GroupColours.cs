﻿namespace Neon.HabboHotel.Groups
{
    public class GroupColours
    {
        public int Id { get; private set; }
        public string Colour { get; private set; }
        public GroupColours(int id, string colour)
        {
            Id = id;
            Colour = colour;
        }
    }
}