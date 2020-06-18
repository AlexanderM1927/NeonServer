using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.Database.Interfaces;
using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class PrefixCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_prefix"; }
        }

        public string Parameters
        {
            get { return "%prefix%"; }
        }

        public string Description
        {
            get { return "Borra tu prefijo."; }
        }

        public void Execute(GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor, escribe \":prefix off\" para desactivar tu prefijo.");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 1);

            if (Message == "off")
            {
                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunQuery("UPDATE `users` SET `tag` = NULL WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                }
                Session.GetHabbo()._tag = string.Empty;
                Session.SendWhisper("Prefijo borrado correctamente.", 34);
            }
        }
    }
}
