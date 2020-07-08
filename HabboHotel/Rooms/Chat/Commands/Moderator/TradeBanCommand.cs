using Neon.Database.Interfaces;
using Neon.HabboHotel.Users;
using System;



namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class TradeBanCommand : IChatCommand
    {
        public string PermissionRequired => "command_trade_ban";

        public string Parameters => "%target% %length%";

        public string Description => "Banea el trade a un usuario";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Introduce el nombre del usuario y el tiempo en dias (min 1 dia, max 365 dias).");
                return;
            }

            Habbo Habbo = NeonEnvironment.GetHabboByUsername(Params[1]);
            if (Habbo == null)
            {
                Session.SendWhisper("Ocurrio un error cuando se hizo la consulta en la base de datos.");
                return;
            }

            if (Convert.ToDouble(Params[2]) == 0)
            {
                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunQuery("UPDATE `user_info` SET `trading_locked` = '0' WHERE `user_id` = '" + Habbo.Id + "' LIMIT 1");
                }

                if (Habbo.GetClient() != null)
                {
                    Habbo.TradingLockExpiry = 0;
                    Habbo.GetClient().SendNotification("Sus tradeo ya fueron desbloqueados, puede seguir comerciando con los demás usuarios.");
                }

                Session.SendWhisper("Desbloqueaste a " + Habbo.Username + " de su trade Ban.");
                return;
            }

            if (double.TryParse(Params[2], out double Days))
            {
                if (Days < 1)
                {
                    Days = 1;
                }

                if (Days > 365)
                {
                    Days = 365;
                }

                double Length = (NeonEnvironment.GetUnixTimestamp() + (Days * 86400));
                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunQuery("UPDATE `user_info` SET `trading_locked` = '" + Length + "', `trading_locks_count` = `trading_locks_count` + '1' WHERE `user_id` = '" + Habbo.Id + "' LIMIT 1");
                }

                if (Habbo.GetClient() != null)
                {
                    Habbo.TradingLockExpiry = Length;
                    Habbo.GetClient().SendNotification("Usted tiene un bloqueo de tradeos por " + Days + " día(s).");
                }

                Session.SendWhisper("Usted le ha bloqueado los tradeos a  " + Habbo.Username + " por " + Days + " día(s).");
            }
            else
            {
                Session.SendWhisper("Introduce dias valido, en numeros enteros.");
            }
        }
    }
}
