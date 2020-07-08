using Neon.HabboHotel.Cache;
using Neon.HabboHotel.Rooms;
using System.Linq;

namespace Neon.Communication.Packets.Outgoing.Rooms.Settings
{
    internal class RoomRightsListComposer : ServerPacket
    {
        public RoomRightsListComposer(Room Instance)
            : base(ServerPacketHeader.RoomRightsListMessageComposer)
        {
            base.WriteInteger(Instance.Id);

            base.WriteInteger(Instance.UsersWithRights.Count);
            foreach (int Id in Instance.UsersWithRights.ToList())
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
