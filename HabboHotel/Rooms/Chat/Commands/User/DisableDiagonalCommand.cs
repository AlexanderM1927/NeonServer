namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class DisableDiagonalCommand : IChatCommand
    {
        public string PermissionRequired => "command_disable_diagonal";

        public string Parameters => "";

        public string Description => "Desactivar la opción de andar en diagonal en tu sala.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (!Room.CheckRights(Session, true))
            {
                Session.SendWhisper("Oops, solo el dueño de la sala puede ejecutar el comando!");
                return;
            }

            Room.GetGameMap().DiagonalEnabled = !Room.GetGameMap().DiagonalEnabled;
            Session.SendWhisper("Nadie puede caminar en diagonal en la sala");
        }
    }
}
