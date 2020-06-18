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


namespace Neon.HabboHotel.Rooms.Chat.Commands.Events
{
    internal class CatalogUpdateAlert : IChatCommand
    {
        public string PermissionRequired
        {
            get
            {
                return "command_addpredesigned";
            }
        }
        public string Parameters
        {
            get { return "%message%"; }
        }
        public string Description
        {
            get
            {
                return "Avisar de una actualización en el catálogo del hotel.";
            }
        }
        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            string Message = CommandManager.MergeParams(Params, 1);
            NeonEnvironment.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("¡Actualización en el catálogo!",
              "¡El catálogo de <font color=\"#2E9AFE\"><b>Keko</b></font> acaba de ser actualizado! Si quieres observar <b>las novedades</b> sólo debes hacer click en el botón de abajo.<br>", "cata", "Ir a la página", "event:catalog/open/" + Message));

            Session.SendWhisper("Catalogo actualizado satisfactoriamente.");
        }
    }
}

