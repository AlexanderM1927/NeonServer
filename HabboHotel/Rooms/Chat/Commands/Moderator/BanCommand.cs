using Neon.Database.Interfaces;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Moderation;
using Neon.HabboHotel.Users;
using System;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class BanCommand : IChatCommand
    {

        public string PermissionRequired => "command_ban";

        public string Parameters => "%usuario% %duración% %razón% ";

        public string Description
        {
            get { return "Realiza una petición de baneo."; ; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor introduzca el nombre del usuario.");
                return;
            }

            Habbo Habbo = NeonEnvironment.GetHabboByUsername(Params[1]);
            if (Habbo == null)
            {
                Session.SendWhisper("El usuario " + Params[1] + " no existe.");
                return;
            }

            if (Habbo.GetPermissions().HasRight("mod_soft_ban") && !Session.GetHabbo().GetPermissions().HasRight("mod_ban_any"))
            {
                Session.SendWhisper("Vaya... al parecer no puedes banear a " + Params[1] + ".");
                return;
            }

            string Hours = Params[2];

            double Expire;
            if (string.IsNullOrEmpty(Hours) || Hours == "perm")
            {
                Expire = NeonEnvironment.GetUnixTimestamp() + 78892200;
            }
            else
            {
                Expire = (NeonEnvironment.GetUnixTimestamp() + (Convert.ToDouble(Hours) * 3600));
            }

            string Reason;
            if (Params.Length >= 4)
            {
                Reason = CommandManager.MergeParams(Params, 3);
            }
            else
            {
                Reason = "Sin razón.";
            }

            string Username = Habbo.Username;
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE `user_info` SET `bans` = `bans` + '1' WHERE `user_id` = '" + Habbo.Id + "' LIMIT 1");
            }

            NeonEnvironment.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.USERNAME, Habbo.Username, Reason, Expire);

            GameClient TargetClient = NeonEnvironment.GetGame().GetClientManager().GetClientByUsername(Username);
            if (TargetClient != null)
            {
                TargetClient.Disconnect();
            }

            Session.SendWhisper("Excelente, ha sido baneado el usuario '" + Username + "' por " + Hours + " hhora(s) con la razon '" + Reason + "'!");
        }
    }
}