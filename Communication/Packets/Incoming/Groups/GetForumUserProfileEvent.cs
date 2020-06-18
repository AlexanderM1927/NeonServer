using System.Collections.Generic;

using Neon.HabboHotel.Users;
using Neon.HabboHotel.Groups;
using Neon.HabboHotel.GameClients;

using Neon.Database.Interfaces;
using Neon.Communication.Packets.Outgoing.Users;

namespace Neon.Communication.Packets.Incoming.Groups.Forums
{
    class GetForumUserProfileEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string username = Packet.PopString();

            Habbo targetData = NeonEnvironment.GetHabboByUsername(username);
            if (targetData == null)
            {
                Session.SendNotification("Ha ocurrido un error buscando el perfil del usuario.");
                return;
            }

            List<Group> groups = NeonEnvironment.GetGame().GetGroupManager().GetGroupsForUser(targetData.Id);

            int friendCount = 0;
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT COUNT(0) FROM `messenger_friendships` WHERE (`user_one_id` = @userid OR `user_two_id` = @userid)");
                dbClient.AddParameter("userid", targetData.Id);
                friendCount = dbClient.getInteger();
            }

            Session.SendMessage(new ProfileInformationComposer(targetData, Session, groups, friendCount));
        }
    }
}
