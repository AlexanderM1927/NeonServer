using Neon.Database.Interfaces;



namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class DisableGiftsCommand : IChatCommand
    {
        public string PermissionRequired => "command_disable_gifts";

        public string Parameters => "";

        public string Description => "Activar o desactivar la recepción de regalos.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            Session.GetHabbo().AllowGifts = !Session.GetHabbo().AllowGifts;
            Session.SendWhisper("Usted " + (Session.GetHabbo().AllowGifts == true ? "ahora" : "ya no") + " acepta regalos.");

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `allow_gifts` = @AllowGifts WHERE `id` = '" + Session.GetHabbo().Id + "'");
                dbClient.AddParameter("AllowGifts", NeonEnvironment.BoolToEnum(Session.GetHabbo().AllowGifts));
                dbClient.RunQuery();
            }
        }
    }
}