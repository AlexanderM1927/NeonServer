using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Neon.Database.Interfaces;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    class EnableFriends : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_enable_friends"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Activar o desactivar las solicitudes de amistad."; }
        }

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