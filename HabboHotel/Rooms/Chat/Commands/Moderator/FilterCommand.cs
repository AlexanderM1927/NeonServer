using Neon.Database.Interfaces;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class FilterCommand : IChatCommand
    {
        public string PermissionRequired => "command_filter";

        public string Parameters => "%Palabra%";

        public string Description => "Agrega una palabra al Filtro.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Introduce la palabra que quieres agregar al Filtro.");
                return;
            }
            string BannedWord = Params[1];
            if (!string.IsNullOrWhiteSpace(BannedWord))
            {
                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("INSERT INTO wordfilter (`word`, `addedby`, `bannable`) VALUES " +
                        "(@ban, '" + Session.GetHabbo().Username + "', '1');");
                    dbClient.AddParameter("ban", BannedWord.ToLower());
                    dbClient.RunQuery();
                    Session.SendWhisper("'" + BannedWord + "' Ha sido agregado correctamente al Filtro");
                }
            }
        }
    }
}