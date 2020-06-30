using Neon.Communication.Packets.Outgoing.Inventory.Furni;
using Neon.Communication.Packets.Outgoing.Rooms.Engine;
using Neon.Communication.Packets.Outgoing.Rooms.Furni;
using Neon.Database.Interfaces;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Items;
using Neon.HabboHotel.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User.Fan
{
    internal class BuildCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_build"; }
        }

        public string Parameters
        {
            get { return "%height%"; }
        }

        public string Description
        {
            get { return ""; }
        }
        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            string height = Params[1];
            if (Session.GetHabbo().Id == Room.OwnerId)
            {
                if (!Room.CheckRights(Session, true))
                    return;
                Item[] items = Room.GetRoomItemHandler().GetFloor.ToArray();
                foreach (Item Item in items.ToList())
                {
                    _ = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(Item.UserID);
                    if (Item.GetBaseItem().InteractionType == InteractionType.STACKTOOL)

                        Room.SendMessage(new UpdateMagicTileComposer(Item.Id, int.Parse(height)));
                }
            }
        }
    }
}