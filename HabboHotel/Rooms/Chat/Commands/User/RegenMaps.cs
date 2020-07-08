namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class RegenMaps : IChatCommand
    {
        public string PermissionRequired => "command_regen_maps";

        public string Parameters => "";

        public string Description => "Regenerar el mapa de la sala en la que estás.";

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {
            if (!Room.CheckRights(Session, true))
            {
                Session.SendWhisper("Oops, solo el dueño de la sala puede ejecutar este comando!");
                return;
            }

            Room.GetGameMap().GenerateMaps();
            Session.SendWhisper("Excelente, el mapa del juego ha sido regenerado.");
        }
    }
}
