using Neon.HabboHotel.Cache;
using Neon.HabboHotel.Rooms;
using System.Linq;

namespace Neon.Communication.Packets.Outgoing.Rooms.Settings
{
    internal class GetRoomBannedUsersComposer : ServerPacket
    {
        public GetRoomBannedUsersComposer(Room Instance)
            : base(ServerPacketHeader.GetRoomBannedUsersMessageComposer)
        {
            base.WriteInteger(Instance.Id);

            base.WriteInteger(Instance.BannedUsers().Count);//Count
            foreach (int Id in Instance.BannedUsers().ToList())
            {
                UserCache Data = NeonEnvironment.GetGame().GetCacheManager().GenerateUser(Id);

                if (Data == null)
                {
                    base.WriteInteger(0);
                    base.WriteString("Unknown Error");
                }
                else
                {
                    base.WriteInteger(Data.Id);
                    base.WriteString(Data.Username);
                }
            }
        }
    }
}
