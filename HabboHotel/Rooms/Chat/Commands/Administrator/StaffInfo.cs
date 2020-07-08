using Neon.Communication.Packets.Outgoing.Notifications;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class StaffInfo : IChatCommand
    {
        public string PermissionRequired => "command_staffinfo";

        public string Parameters => "";

        public string Description => "Observa una lista de todos los staffs conectados.";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            Dictionary<Habbo, uint> clients = new Dictionary<Habbo, uint>();

            StringBuilder content = new StringBuilder();
            content.Append("Estado de los Staff conectados en " + NeonEnvironment.GetConfig().data["hotel.name"] + ":\r\n");

            foreach (GameClient client in NeonEnvironment.GetGame().GetClientManager()._clients.Values)
            {
                if (client != null && client.GetHabbo() != null && client.GetHabbo().Rank > 3)
                {
                    clients.Add(client.GetHabbo(), (Convert.ToUInt16(client.GetHabbo().Rank)));
                }
            }

            foreach (KeyValuePair<Habbo, uint> client in clients.OrderBy(key => key.Value))
            {
                if (client.Key == null)
                {
                    continue;
                }

                content.Append("¥ " + client.Key.Username + " [Rango: " + client.Key.Rank + "] - Se encuentra en la sala: " + ((client.Key.CurrentRoom == null) ? "En ninguna sala." : client.Key.CurrentRoom.RoomData.Name) + "\r\n");
            }

            Session.SendMessage(new MOTDNotificationComposer(content.ToString()));

            return;
        }
    }
}