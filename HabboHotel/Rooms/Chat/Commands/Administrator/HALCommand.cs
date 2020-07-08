using Neon.Communication.Packets.Outgoing.Moderation;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Administrator
{
    internal class HALCommand : IChatCommand
    {
        public string PermissionRequired => "command_hal";

        public string Parameters => "%message%";

        public string Description => "Envia un mensaje entero a todo el Hotel con un Link";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 2)
            {
                Session.SendWhisper("Por favor escribe el mensaje y el Link a enviar.");
                return;
            }

            string URL = Params[1];
            string Message = CommandManager.MergeParams(Params, 2);

            NeonEnvironment.GetGame().GetClientManager().SendMessage(new SendHotelAlertLinkEventComposer("Alerta del Equipo Administrativo:\r\n" + Message + "\r\n-" + Session.GetHabbo().Username, URL));
            return;
        }
    }
}
