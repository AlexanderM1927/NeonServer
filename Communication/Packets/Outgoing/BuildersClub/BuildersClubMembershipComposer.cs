﻿namespace Neon.Communication.Packets.Outgoing.BuildersClub
{
    internal class BuildersClubMembershipComposer : ServerPacket
    {
        public BuildersClubMembershipComposer()
            : base(ServerPacketHeader.BuildersClubMembershipMessageComposer)
        {
            base.WriteInteger(int.MaxValue);
            base.WriteInteger(100);
            base.WriteInteger(0);
            base.WriteInteger(int.MaxValue);
        }
    }
}
