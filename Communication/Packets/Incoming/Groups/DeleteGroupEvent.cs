using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.Database.Interfaces;
using Neon.HabboHotel.Groups;
using Neon.HabboHotel.Rooms;

namespace Neon.Communication.Packets.Incoming.Groups
{
    internal class DeleteGroupEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!NeonEnvironment.GetGame().GetGroupManager().TryGetGroup(Packet.PopInt(), out Group Group))
            {
                Session.SendMessage(new RoomNotificationComposer("Oops!",
                 "¡No se ha encontrado este grupo!", "nothing", ""));
                return;
            }

            if (Group.CreatorId != Session.GetHabbo().Id && !Session.GetHabbo().GetPermissions().HasRight("group_delete_override"))
            {
                Session.SendMessage(new RoomNotificationComposer("Oops!",
                 "¡Sólo el dueño del grupo puede eliminarlo!", "nothing", ""));
                return;
            }

            if (Group.MemberCount >= NeonStaticGameSettings.GroupMemberDeletionLimit && !Session.GetHabbo().GetPermissions().HasRight("group_delete_limit_override"))
            {
                Session.SendMessage(new RoomNotificationComposer("Sucesso",
                 "El grupo sobre pasa el límite de miembros permitido (" + NeonStaticGameSettings.GroupMemberDeletionLimit + "), contacta con uno de los miembros del equipo administrativo.", "nothing", ""));
                return;
            }

            Room Room = NeonEnvironment.GetGame().GetRoomManager().LoadRoom(Group.RoomId);

            if (Room != null)
            {
                Room.Group = null;
                Room.RoomData.Group = null;//Eu não tenho certeza se isso é necessário ou não, por causa da herança, mas tudo bem.
            }

            //Removê-lo do cache.
            NeonEnvironment.GetGame().GetGroupManager().DeleteGroup(Group.Id);

            //Agora as coisas.
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("DELETE FROM `groups` WHERE `id` = '" + Group.Id + "'");
                dbClient.RunQuery("DELETE FROM `group_memberships` WHERE `group_id` = '" + Group.Id + "'");
                dbClient.RunQuery("DELETE FROM `group_requests` WHERE `group_id` = '" + Group.Id + "'");
                dbClient.RunQuery("UPDATE `rooms` SET `group_id` = '0' WHERE `group_id` = '" + Group.Id + "' LIMIT 1");
                dbClient.RunQuery("UPDATE `user_stats` SET `groupid` = '0' WHERE `groupid` = '" + Group.Id + "' LIMIT 1");
                dbClient.RunQuery("DELETE FROM `items_groups` WHERE `group_id` = '" + Group.Id + "'");
            }

            //Descarregá-lo pela última vez.
            NeonEnvironment.GetGame().GetRoomManager().UnloadRoom(Room, true);

            //Wulles Rainha
            Session.SendMessage(new RoomNotificationComposer("Sucesso",
                 "¡Has borrado satisfactoriamente tu grupo!", "nothing", ""));
            return;
        }
    }
}
