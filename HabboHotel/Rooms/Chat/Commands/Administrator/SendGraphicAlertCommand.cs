
using Neon.Communication.Packets.Outgoing.Notifications;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Administrator
{
    class SendGraphicAlertCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_ha"; }
        }

        public string Parameters
        {
            get { return "%image%"; }
        }

        public string Description
        {
            get { return "Envía un mensaje de alerta con imagen a todo el hotel."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor escribe el nombre de la imagen a enviar.");
                return;
            }

            string image = Params[2];

            NeonEnvironment.GetGame().GetClientManager().SendMessage(new GraphicAlertComposer(image));
            return;
        }
    }
}
