﻿using System.Collections;

namespace Neon.Communication.Packets.Outgoing.Navigator
{
    internal class FavouritesComposer : ServerPacket
    {
        public FavouritesComposer(ArrayList favouriteIDs)
            : base(ServerPacketHeader.FavouritesMessageComposer)
        {
            base.WriteInteger(50);
            base.WriteInteger(favouriteIDs.Count);

            foreach (int Id in favouriteIDs.ToArray())
            {
                base.WriteInteger(Id);
            }
        }
    }
}
