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
    class ColourList : IChatCommand
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
            get { return "Información de Neon."; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            Session.SendMessage(new RoomNotificationComposer("Lista de colores:",
                 "<font color='#FF8000'><b>COLORES:</b>\n" +
                 "<font size=\"12\" color=\"#1C1C1C\">El comando :color te permitirá fijar un color que tu desees en tu bocadillo de chat, para poder seleccionar el color deberás especificarlo después de hacer el comando, como por ejemplo:<br><i>:color red</i></font>" +
                 "<font size =\"13\" color=\"#0B4C5F\"><b>Stats:</b></font>\n" +
                 "<font size =\"11\" color=\"#1C1C1C\">  <b> · Users: </b> \r  <b> · Rooms: </b> \r  <b> · Uptime: </b>minutes.</font>\n\n" +
                 "", "quantum", ""));
        }
    }
}