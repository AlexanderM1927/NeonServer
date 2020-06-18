using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Neon.Communication.Packets.Outgoing.Users;
using Neon.Communication.Packets.Outgoing.Notifications;


using Neon.Communication.Packets.Outgoing.Handshake;
using Neon.Communication.Packets.Outgoing.Quests;
using Neon.HabboHotel.Items;
using Neon.Communication.Packets.Outgoing.Inventory.Furni;
using Neon.Communication.Packets.Outgoing.Catalog;
using Neon.HabboHotel.Quests;
using Neon.HabboHotel.Rooms;
using System.Threading;
using Neon.HabboHotel.GameClients;
using Neon.Communication.Packets.Outgoing.Rooms.Avatar;
using Neon.Communication.Packets.Outgoing.Pets;
using Neon.Communication.Packets.Outgoing.Messenger;
using Neon.HabboHotel.Users.Messenger;
using Neon.Communication.Packets.Outgoing.Rooms.Polls;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.Communication.Packets.Outgoing.Availability;
using Neon.Communication.Packets.Outgoing;
using Neon.Communication.Packets.Outgoing.Nux;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    class PriceList : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_info"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Ver la lista de precios de raros."; }
        }

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