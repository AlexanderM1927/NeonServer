using Neon.Database.Interfaces;



namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class DisableMimicCommand : IChatCommand
    {
        public string PermissionRequired => "command_disable_mimic";

        public string Parameters => "";

        public string Description => "Activar o desactivar la opción de que copien tu ropa.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            Session.GetHabbo().AllowMimic = !Session.GetHabbo().AllowMimic;
            Session.SendWhisper("Usted " + (Session.GetHabbo().AllowMimic == true ? "ahora" : "ya no") + " protege su look.");

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `allow_mimic` = @AllowMimic WHERE `id` = '" + Session.GetHabbo().Id + "'");
                dbClient.AddParameter("AllowMimic", NeonEnvironment.BoolToEnum(Session.GetHabbo().AllowMimic));
                dbClient.RunQuery();
            }
        }
    }
}