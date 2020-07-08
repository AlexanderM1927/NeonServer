namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class DisableWhispersCommand : IChatCommand
    {
        public string PermissionRequired => "command_disable_whispers";

        public string Parameters => "";

        public string Description => "Activar o desactivar la capacidad de recibir susurros.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            Session.GetHabbo().ReceiveWhispers = !Session.GetHabbo().ReceiveWhispers;
            Session.SendWhisper("Usted " + (Session.GetHabbo().ReceiveWhispers ? "ahora" : "ya no") + " recibe susurros!");
        }
    }
}
