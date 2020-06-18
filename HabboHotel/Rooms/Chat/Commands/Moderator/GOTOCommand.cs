using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.GameClients;
using Neon.Communication.Packets.Outgoing.Inventory.Purse;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class GOTOCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_goto"; }
        }

        public string Parameters
        {
            get { return "%room_id%"; }
        }

        public string Description
        {
            get { return ""; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Usted debe especificar el ID de la sala");
                return;
            }

            int RoomID;

            if (!int.TryParse(Params[1], out RoomID))
                Session.SendWhisper("Usted debe escribir correctamente el ID de la sala");
            else
            {
                Room _room = NeonEnvironment.GetGame().GetRoomManager().LoadRoom(RoomID);
                if (_room == null)
                    Session.SendWhisper("Esta sala no existe!");
                else
                {
                    Session.GetHabbo().PrepareRoom(_room.Id, "");
                }
            }
        }
    }
}