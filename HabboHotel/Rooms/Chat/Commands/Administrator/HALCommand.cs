using Neon.Communication.Packets.Outgoing;
using Neon.Communication.Packets.Outgoing.Moderation;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Administrator
{
    class HALCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_hal"; }
        }

        public string Parameters
        {
            get { return "%message%"; }
        }

        public string Description
        {
            get { return "Envia un mensaje entero a todo el Hotel con un Link"; }
        }

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
