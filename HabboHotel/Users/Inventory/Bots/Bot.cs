﻿namespace Neon.HabboHotel.Users.Inventory.Bots
{
    public class Bot
    {
        private int _id;
        private int _ownerId;
        private string _name;
        private string _motto;
        private string _figure;
        private string _gender;

        public Bot(int Id, int OwnerId, string Name, string Motto, string Figure, string Gender)
        {
            this.Id = Id;
            this.OwnerId = OwnerId;
            this.Name = Name;
            this.Motto = Motto;
            this.Figure = Figure;
            this.Gender = Gender;
        }

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public int OwnerId
        {
            get => _ownerId;
            set => _ownerId = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Motto
        {
            get => _motto;
            set => _motto = value;
        }

        public string Figure
        {
            get => _figure;
            set => _figure = value;
        }

        public string Gender
        {
            get => _gender;
            set => _gender = value;
        }
    }
}
