using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.GameClients;
using System.Linq;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class MassBadgeCommand : IChatCommand
    {
        public string PermissionRequired => "command_mass_badge";

        public string Parameters => "%badge%";

        public string Description => "Envia una placa a todos los del hotel";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor introduce el codigo de la placa que deseas enviar a todos");
                return;
            }

            foreach (GameClient Client in NeonEnvironment.GetGame().GetClientManager().GetClients.ToList())
            {
                if (Client == null || Client.GetHabbo() == null || Client.GetHabbo().Username == Session.GetHabbo().Username)
                {
                    continue;
                }

                if (!Client.GetHabbo().GetBadgeComponent().HasBadge(Params[1]))
                {
                    Client.GetHabbo().GetBadgeComponent().GiveBadge(Params[1], true, Client);
                    Client.SendMessage(RoomNotificationComposer.SendBubble("cred", "" + Session.GetHabbo().Username + " te acaba de enviar la placa " + Params[1] + ".", ""));
                }
                else
                {
                    Client.SendMessage(RoomNotificationComposer.SendBubble("cred", "" + Session.GetHabbo().Username + " ha intentado enviarte la placa " + Params[1] + " pero ya la tienes.", ""));
                }
            }

            Session.SendWhisper("Usted le ha dado con exito a cada uno de los del hotel " + Params[1] + " placa!");
        }
    }
}
