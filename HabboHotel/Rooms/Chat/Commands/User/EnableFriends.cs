using Neon.Database.Interfaces;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class EnableFriends : IChatCommand
    {
        public string PermissionRequired => "command_enable_friends";

        public string Parameters => "";

        public string Description => "Activar o desactivar las solicitudes de amistad.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            Session.GetHabbo().AllowFriendRequests = !Session.GetHabbo().AllowFriendRequests;
            Session.SendWhisper("Ahora mismo " + (Session.GetHabbo().AllowFriendRequests == true ? "aceptas" : "no aceptas") + " nuevas peticiones de amistad");

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `block_newfriends` = '0' WHERE `id` = '" + Session.GetHabbo().Id + "'");
                dbClient.RunQuery();
            }
        }
    }
}