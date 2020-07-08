using Neon.Communication.Packets.Outgoing.Groups;
using Neon.Database.Interfaces;
using Neon.HabboHotel.Groups;
namespace Neon.Communication.Packets.Incoming.Groups
{
    internal class UpdateGroupBadgeEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GroupId = Packet.PopInt();
            if (!NeonEnvironment.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group Group))
            {
                return;
            }

            if (Group.CreatorId != Session.GetHabbo().Id)
            {
                return;
            }

            int Count = Packet.PopInt();
            string Badge = "";
            for (int i = 0; i < Count; i++)
            {
                Badge += BadgePartUtility.WorkBadgeParts(i == 0, Packet.PopInt().ToString(), Packet.PopInt().ToString(), Packet.PopInt().ToString());
            }
            Group.Badge = (string.IsNullOrWhiteSpace(Badge) ? "b05114s06114" : Badge);
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `groups` SET `badge` = @badge WHERE `id` = @groupId LIMIT 1");
                dbClient.AddParameter("badge", Group.Badge);
                dbClient.AddParameter("groupId", Group.Id);
                dbClient.RunQuery();
            }
            Session.SendMessage(new GroupInfoComposer(Group, Session));
        }
    }
}