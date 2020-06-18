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
    internal class PubliAlert : IChatCommand
    {
        public string PermissionRequired
        {
            get
            {
                return "command_publi_alert";
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
                return "Manda un Evento a todo el Hotel!";
            }
        }
        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            string Message = CommandManager.MergeParams(Params, 1);
            NeonEnvironment.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("Se ha abierto oleada de publicidad..",
                 "¡Hay una nueva oleada de publicidad en activo! Si quieres ganar <b>distintas recompensas</b> por participar acude a la sala de publicidad.<br><br>¿Quién ha abierto la oleada? <b> <font color=\"#58ACFA\">  "
                 + Session.GetHabbo().Username + "</font></b><br>Si quieres participar haz click en el botón inferior de <b>Ir a la sala del evento</b>, y ahí dentro podrás participar.<br><br>¿De qué trata este evento?<br><br><font color='#084B8A'><b>Trata de seguir las instrucciones de los guías de la oleada para participar y así ganar tu premio!</b></font><br><br>¡Te esperamos!", "zpam", "Ir a la sala de la oleada", "event:navigator/goto/" + Session.GetHabbo().CurrentRoomId));

        }
    }
}

