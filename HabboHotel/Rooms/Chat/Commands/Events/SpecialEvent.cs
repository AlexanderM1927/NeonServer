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
    internal class SpecialEvent : IChatCommand
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
                return "Manda un evento a todo el hotel.";
            }
        }
        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            string Message = CommandManager.MergeParams(Params, 1);

            NeonEnvironment.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("¿Qué está pasando en " + NeonEnvironment.GetDBConfig().DBData["hotel.name"] + "...?",
                 "Algo está ocurriendo en Habbi, Custom, HiddenKey y Root han desaparecido en medio de la ceremonia...<br><br>Un ente susurra y pide ayuda a todo Habbi, parece que los espíritus reclaman la presencia de todos nuestros usuarios.<br></font></b><br>Si quieres colaborar haz click en el botón inferior y sigue las instrucciones.<br><br></font>", "2mesex", "¡A la aventura!", "event:navigator/goto/" + Session.GetHabbo().CurrentRoomId));

        }
    }
}

