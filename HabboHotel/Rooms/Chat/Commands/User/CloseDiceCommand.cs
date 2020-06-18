﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    class CloseDiceCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_close_dice"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Cierra tus dados cuando en una tradición 5 mueren booth."; }
        }

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {
            RoomUser roomUser = Room?.GetRoomUserManager()?.GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (roomUser == null)
            {
                return;
            }

            List<Items.Item> userBooth = Room.GetRoomItemHandler().GetFloor.Where(x => x != null && Gamemap.TilesTouching(
                x.Coordinate, roomUser.Point) && x.Data.InteractionType == Items.InteractionType.DICE).ToList();

            if (userBooth.Count != 5)
            {
                Session.SendWhisper("Debes estar en una cabina con 5 dados.");
                return;
            }

            userBooth.ForEach(x => {
                x.ExtraData = "0";
                x.UpdateState();
            });

            Session.SendWhisper("Se han cerrado los dados de tu stand.");
        }
    }
}