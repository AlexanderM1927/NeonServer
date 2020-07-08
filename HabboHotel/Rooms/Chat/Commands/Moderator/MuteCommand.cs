using Neon.Database.Interfaces;
using Neon.HabboHotel.Users;



namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class MuteCommand : IChatCommand
    {
        public string PermissionRequired => "command_mute";

        public string Parameters => "%username% %time%";

        public string Description => "Mutea a un usuario por cierto tiempo";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Introduce el nombre del usuario a mutear y el tiempo expresado en Segundos (Maximo 600).");
                return;
            }

            Habbo Habbo = NeonEnvironment.GetHabboByUsername(Params[1]);
            if (Habbo == null)
            {
                Session.SendWhisper("Ocurrio un error mientras se buscaba al usuario en la base de datos.");
                return;
            }

            if (Habbo.GetPermissions().HasRight("mod_tool") && !Session.GetHabbo().GetPermissions().HasRight("mod_mute_any"))
            {
                Session.SendWhisper("Oops, al parecer no se puede mutear a este usuario.");
                return;
            }

            if (double.TryParse(Params[2], out double Time))
            {
                if (Time > 600 && !Session.GetHabbo().GetPermissions().HasRight("mod_mute_limit_override"))
                {
                    Time = 600;
                }

                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunQuery("UPDATE `users` SET `time_muted` = '" + Time + "' WHERE `id` = '" + Habbo.Id + "' LIMIT 1");
                }

                if (Habbo.GetClient() != null)
                {
                    Habbo.TimeMuted = Time;
                    Habbo.GetClient().SendNotification("Usted ha sido muteado " + Time + " segundos!");
                }

                Session.SendWhisper("Muteaste a  " + Habbo.Username + " por " + Time + " segundos.");
            }
            else
            {
                Session.SendWhisper("Por favor introduce numeros enteros.");
            }
        }
    }
}