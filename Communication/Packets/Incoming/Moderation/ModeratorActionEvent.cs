﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.Communication.Packets.Incoming;
using Neon.HabboHotel.Rooms;
using Neon.Communication.Packets.Outgoing.Moderation;

namespace Neon.Communication.Packets.Incoming.Moderation
{
    class ModeratorActionEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().GetPermissions().HasRight("mod_caution"))
                return;

            if (!Session.GetHabbo().InRoom)
                return;

            Room CurrentRoom = Session.GetHabbo().CurrentRoom;
            if (CurrentRoom == null)
                return;

            int AlertMode = Packet.PopInt(); 
            string AlertMessage = Packet.PopString();
            bool IsCaution = AlertMode != 3;

            AlertMessage = IsCaution ? "Alerta del moderador:\n\n" + AlertMessage : "Mensaje por Moderador:\n\n" + AlertMessage;
            Session.GetHabbo().CurrentRoom.SendMessage(new BroadcastMessageAlertComposer(AlertMessage));
        }
    }
}
