namespace Neon
{
    internal class NeonStaticGameSettings
    {
        /// <summary>
        ///     The amount of credits a user will recieve every x minutes
        /// </summary>
        public const int UserCreditsUpdateAmount = 0;

        /// <summary>
        ///     The amount of pixels a user will recieve every x minutes
        /// </summary>
        public const int UserDiamondUpdateAmount = 5;

        /// <summary>
        ///     The amount of pixels extra to VIP Users
        /// </summary>
        public const int UserVipPixelsUpdateAmount = 3;

        /// <summary>
        ///     The time a user will have to wait for Credits/Diamonds update in minutes
        /// </summary>
        public const int UserCreditsUpdateTimer = 30;

        /// <summary>
        /// 
        /// </summary>
        public const int BonusRareUpdateTimer = 120;

        /// <summary>
        ///     The maximum amount of furniture that can be placed in a room.
        /// </summary>
        public const int RoomFurnitureLimit = 10500;

        /// <summary>
        ///     The maximum amount of pet instances that can be placed into a room.
        /// </summary>
        public const int RoomPetPlacementLimit = 25;

        /// <summary>
        ///     The amount of pets a user can place in a room that isn't owned by them.
        /// </summary>
        public const int UserPetPlacementRoomLimit = 5;

        /// <summary>
        ///     The maximum life time of a room promotion.
        /// </summary>
        public const int RoomPromotionLifeTime = 120;

        /// <summary>
        ///     The maximum amount of friends a user can have.
        /// </summary>
        public const int MessengerFriendLimit = 5000;

        /// <summary>
        ///     The amount of credits a group costs.
        /// </summary>
        public const int GroupPurchaseAmount = 150;

        /// <summary>
        ///     The maximum amount of banned words that a client can send per session.
        /// </summary>
        public const int BannedPhrasesAmount = 4;

        /// <summary>
        /// The maximum amount of members a group can exceed before being eligible for deletion.
        /// </summary>
        public const int GroupMemberDeletionLimit = 500;

        /// <summary>
        /// The hotel status
        /// </summary>
        public static bool IsGoingToBeClose = false;

        /// <summary>
        /// Hotel is open for users
        /// </summary>
        public static bool HotelOpenForUsers = true;
    }
}
