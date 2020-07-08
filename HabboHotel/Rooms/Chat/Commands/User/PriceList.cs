using Neon.Communication.Packets.Outgoing.Notifications;
using Neon.HabboHotel.GameClients;
using System.Text;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class PriceList : IChatCommand
    {
        public string PermissionRequired => "command_info";

        public string Parameters => "";

        public string Description => "Ver la lista de precios de raros.";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            StringBuilder List = new StringBuilder("");
            List.AppendLine("                          ¥ LISTA DE PRECIOS DE KEKO¥");
            List.AppendLine("   SOFÁ VIP: Duckets   »   SOFÁ VIP: Duckets   »   SOFÁ VIP: Duckets");
            List.AppendLine("   SOFÁ VIP: Duckets   »   SOFÁ VIP: Duckets   »   SOFÁ VIP: Duckets");
            List.AppendLine("   SOFÁ VIP: Duckets   »   SOFÁ VIP: Duckets   »   SOFÁ VIP: Duckets");
            List.AppendLine("   SOFÁ VIP: Duckets   »   SOFÁ VIP: Duckets   »   SOFÁ VIP: Duckets");
            List.AppendLine("   SOFÁ VIP: Duckets   »   SOFÁ VIP: Duckets   »   SOFÁ VIP: Duckets");
            List.AppendLine("   SOFÁ VIP: Duckets   »   SOFÁ VIP: Duckets   »   SOFÁ VIP: Duckets");
            List.AppendLine("Esta lista todavía está en construcción por Javas, su última actualización fue el día 14 de Julio de 2019.");
            Session.SendMessage(new MOTDNotificationComposer(List.ToString()));


        }
    }
}