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
    class CustomLegit : IChatCommand
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
            get { return "Qué nos deparará el destino..."; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            Session.SendMessage(new NuxAlertComposer("helpBubble/add/CHAT_INPUT/Death awaits us..."));
            Session.SendMessage(new NuxAlertComposer("nux/lobbyoffer/hide"));
            NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_Login", 1);
        }
    }
}