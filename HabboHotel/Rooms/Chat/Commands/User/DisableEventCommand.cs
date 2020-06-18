using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Quasar.Database.Interfaces;


namespace Quasar.HabboHotel.Rooms.Chat.Commands.User
{
    class DisableEventsCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_disable_events"; }
        }


        public string Parameters
        {
            get { return ""; }
        }


        public string Description
        {
            get { return "Activa o desactiva mensajes de eventos."; }
        }


        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            Session.GetHabbo().AllowEvents = !Session.GetHabbo().AllowEvents;
            Session.SendWhisper("Ahora " + (Session.GetHabbo().AllowEvents == true ? "permites" : "no permites") + " las alertas de eventos en el hotel, si quieres volver a recibirlas, realiza el comando nuevamente.");


            using (IQueryAdapter dbClient = QuasarEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `allow_events` = @AllowEvents WHERE `id` = '" + Session.GetHabbo().Id + "'");
                dbClient.AddParameter("AllowEvents", QuasarEnvironment.BoolToEnum(Session.GetHabbo().AllowEvents));
                dbClient.RunQuery();
            }
        }
    }
}