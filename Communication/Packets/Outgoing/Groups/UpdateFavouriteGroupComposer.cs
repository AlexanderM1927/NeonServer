﻿using Neon.HabboHotel.Groups;

namespace Neon.Communication.Packets.Outgoing.Groups
{
    internal class UpdateFavouriteGroupComposer : ServerPacket
    {
        public UpdateFavouriteGroupComposer(int Id, Group Group, int VirtualId)
            : base(ServerPacketHeader.UpdateFavouriteGroupMessageComposer)
        {
            base.WriteInteger(VirtualId);//Sends 0 on .COM
            base.WriteInteger(Group != null ? Group.Id : 0);
            base.WriteInteger(3);
            base.WriteString(Group != null ? Group.Name : string.Empty);
        }
    }
}
