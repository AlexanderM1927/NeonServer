namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class StaffAlertCommand : IChatCommand
    {
        public string PermissionRequired => "command_staff_alert";

        public string Parameters => "%message%";

        public string Description => "Envía un mensaje escrito por usted a los miembros actuales del personal en línea.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor ingrese un mensaje para enviar.");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 1);
            NeonEnvironment.GetGame().GetClientManager().StaffAlert3("[Staff Alert] " + Message + "" + " - " + Session.GetHabbo().Username);
            return;
        }
    }
}