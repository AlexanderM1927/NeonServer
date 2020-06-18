﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Items;
using Neon.Communication.Packets.Outgoing.Inventory.Purse;
using Neon.Communication.Packets.Outgoing.Inventory.Furni;

using Neon.Database.Interfaces;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Neon.Communication.Packets.Incoming.Rooms.Furni
{
    class CreditFurniRedeemEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            Room Room;

            if (!NeonEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            if (!Room.CheckRights(Session, true))
                return;

            if (NeonEnvironment.GetDBConfig().DBData["exchange_enabled"] != "1")
            {
                Session.SendNotification("De momento no puedes canjear tus monedas para llevarlo a su monedero.");
                return;
            }

            Item Exchange = Room.GetRoomItemHandler().GetItem(Packet.PopInt());
            if (Exchange == null)
                return;

            if (Exchange.GetBaseItem().ItemName.StartsWith("CF_"))
            {

                string[] Split = Exchange.GetBaseItem().ItemName.Split('_');
                int Value = int.Parse(Split[1]);

                if (Value > 0)
                {
                    Session.GetHabbo().Credits += Value;
                    Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
                }
            }

            if (Exchange.GetBaseItem().ItemName.StartsWith("CFC_"))
            {

                string[] Split = Exchange.GetBaseItem().ItemName.Split('_');
                int Value = int.Parse(Split[1]);

                if (Value > 0)
                {
                    Session.GetHabbo().Duckets += Value;
                    Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Duckets, Value));
                    Session.SendMessage(RoomNotificationComposer.SendBubble("duckets", "Has convertido correctamente " + Value + " Duckets, haz click aquí para abrir la tienda.", "catalog/open"));
                }
            }

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("DELETE FROM `items` WHERE `id` = '" + Exchange.Id + "' LIMIT 1");
            }

            Session.SendMessage(new FurniListUpdateComposer());
            Room.GetRoomItemHandler().RemoveFurniture(null, Exchange.Id, false);
            Session.GetHabbo().GetInventoryComponent().RemoveItem(Exchange.Id);

        }
    }
}
