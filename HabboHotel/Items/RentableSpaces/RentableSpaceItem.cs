namespace Neon.HabboHotel.Items.RentableSpaces
{
    public class RentableSpaceItem
    {
        public RentableSpaceItem(int ItemId, int OwnerId, string OwnerUsername, int ExpireStamp, int Price)
        {
            this.ItemId = ItemId;
            this.OwnerId = OwnerId;
            this.OwnerUsername = OwnerUsername;
            this.ExpireStamp = ExpireStamp;
            this.Price = Price;
        }

        public bool IsRented()
        {
            return ExpireStamp > NeonEnvironment.GetUnixTimestamp();
        }

        public bool Rented => IsRented();

        public int ItemId { get; set; }


        public int OwnerId { get; set; }

        public string OwnerUsername { get; set; }

        public int ExpireStamp { get; set; }

        public int Price { get; set; }

        public int GetExpireSeconds()
        {
            int i = ExpireStamp - (int)NeonEnvironment.GetUnixTimestamp();
            return i > 0 ? i : 0;
        }

    }
}