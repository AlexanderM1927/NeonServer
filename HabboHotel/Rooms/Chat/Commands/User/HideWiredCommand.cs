using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Neon.Communication.Packets.Outgoing.Inventory.Furni;
using System.Globalization;
using Neon.Database.Interfaces;
using Neon.Communication.Packets.Outgoing;
using Neon.HabboHotel.Items;
using Neon.Communication.Packets.Outgoing.Rooms.Engine;
using Neon.Communication.Packets.Outgoing.Rooms.Chat;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    class HideWiredCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return ""; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Esconder Wired No seu quarto ."; }
        }

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {

            if (!Room.CheckRights(Session, false, false))
            {
                Session.SendWhisper("¡No tienes derechos en esa sala!", 34);
                return;
            }

            Room.HideWired = !Room.HideWired;
            if (Room.HideWired)
                Session.SendWhisper("Wired está oculto.", 34);
            else
                Session.SendWhisper("Wired fue mostrado.", 34);

            using (IQueryAdapter con = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                con.SetQuery("UPDATE `rooms` SET `hide_wired` = @enum WHERE `id` = @id LIMIT 1");
                con.AddParameter("enum", NeonEnvironment.BoolToEnum(Room.HideWired));
                con.AddParameter("id", Room.Id);
                con.RunQuery();
            }

            _ = new List<ServerPacket>();

            List<ServerPacket> list = Room.HideWiredMessages(Room.HideWired);

            Room.SendMessage(list);


        }
    }
}