using Neon.Database.Interfaces;


namespace Neon.HabboHotel.Rooms.Chat.Commands.Administrator
{
    internal class DeleteGroupCommand : IChatCommand
    {
        public string PermissionRequired => "command_delete_group";

        public string Parameters => "";

        public string Description => "Elimina un grupo de la base de dato.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
            {
                return;
            }

            if (Room.Group == null)
            {
                Session.SendWhisper("Oops, al parecer no hay un grupo aquí.", 34);
                return;
            }

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("DELETE FROM `groups` WHERE `id` = '" + Room.Group.Id + "'");
                dbClient.RunQuery("DELETE FROM `group_memberships` WHERE `group_id` = '" + Room.Group.Id + "'");
                dbClient.RunQuery("DELETE FROM `group_requests` WHERE `group_id` = '" + Room.Group.Id + "'");
                dbClient.RunQuery("UPDATE `rooms` SET `group_id` = '0' WHERE `group_id` = '" + Room.Group.Id + "' LIMIT 1");
                dbClient.RunQuery("UPDATE `user_stats` SET `groupid` = '0' WHERE `groupid` = '" + Room.Group.Id + "' LIMIT 1");
                dbClient.RunQuery("DELETE FROM `items_groups` WHERE `group_id` = '" + Room.Group.Id + "'");
            }

            NeonEnvironment.GetGame().GetGroupManager().DeleteGroup(Room.RoomData.Group.Id);

            Room.Group = null;
            Room.RoomData.Group = null;

            NeonEnvironment.GetGame().GetRoomManager().UnloadRoom(Room, true);

            Session.SendNotification("Grupo eliminado satisfactoriamente.");
            return;
        }
    }
}
