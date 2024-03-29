﻿using Neon.Communication.Packets.Outgoing.Help;
using Neon.HabboHotel.GameClients;
using System;

namespace Neon.Communication.Packets.Incoming.Help
{
    internal class SubmitBullyReportEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            //0 = sent, 1 = blocked, 2 = no chat, 3 = already reported.
            if (Session == null)
            {
                return;
            }

            int UserId = Packet.PopInt();
            if (UserId == Session.GetHabbo().Id)//Hax
            {
                return;
            }

            if (Session.GetHabbo().AdvertisingReportedBlocked)
            {
                Session.SendMessage(new SubmitBullyReportComposer(1));//This user is blocked from reporting.
                return;
            }

            GameClient Client = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(Convert.ToInt32(UserId));
            if (Client == null)
            {
                Session.SendMessage(new SubmitBullyReportComposer(0));//Just say it's sent, the user isn't found.
                return;
            }

            if (Session.GetHabbo().LastAdvertiseReport > NeonEnvironment.GetUnixTimestamp())
            {
                Session.SendNotification("Solo puedes realizar reportes cada 5 minutos");
                return;
            }

            if (Client.GetHabbo().GetPermissions().HasRight("mod_tool"))//Reporting staff, nope!
            {
                Session.SendNotification("En este momento, no se puede informar de los miembros del personal a través de esta herramienta .");
                return;
            }

            //This user hasn't even said a word, nope!
            if (!Client.GetHabbo().HasSpoken)
            {
                Session.SendMessage(new SubmitBullyReportComposer(2));
                return;
            }

            //Already reported, nope.
            if (Client.GetHabbo().AdvertisingReported && Session.GetHabbo().Rank < 2)
            {
                Session.SendMessage(new SubmitBullyReportComposer(3));
                return;
            }

            if (Session.GetHabbo().Rank <= 1)
            {
                Session.GetHabbo().LastAdvertiseReport = NeonEnvironment.GetUnixTimestamp() + 300;
            }
            else
            {
                Session.GetHabbo().LastAdvertiseReport = NeonEnvironment.GetUnixTimestamp();
            }

            Client.GetHabbo().AdvertisingReported = true;
            Session.SendMessage(new SubmitBullyReportComposer(0));
            //NeonEnvironment.GetGame().GetClientManager().ModAlert("New advertising report! " + Client.GetHabbo().Username + " has been reported for advertising by " + Session.GetHabbo().Username +".");
            NeonEnvironment.GetGame().GetClientManager().DoAdvertisingReport(Session, Client);
            return;
        }
    }
}