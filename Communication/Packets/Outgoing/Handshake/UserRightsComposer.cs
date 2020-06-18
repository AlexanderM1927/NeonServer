using Neon.HabboHotel.Users;

namespace Neon.Communication.Packets.Outgoing.Handshake
{
    public class UserRightsComposer : ServerPacket
    {
        public UserRightsComposer(Habbo habbo)
            : base(ServerPacketHeader.UserRightsMessageComposer)
        {
            if (habbo.GetClubManager().HasSubscription("habbo_vip"))
            {
                WriteInteger(2);
            }
            else if (habbo.GetClubManager().HasSubscription("habbo_club"))
            {
                WriteInteger(1);
            }
            else
            {
                WriteInteger(0);
            }

            WriteInteger(habbo.Rank);
            if (habbo.Rank > 3)
            {
                WriteBoolean(true);//Is an ambassador
            }
            else
            {
                WriteBoolean(false);//Is an ambassador
            }

        }
    }
}