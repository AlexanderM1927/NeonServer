using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Neon.Database.Interfaces;


namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    class MutePetsCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_mute_pets"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Silenciar todo lo que digan las mascotas."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            Session.GetHabbo().AllowPetSpeech = !Session.GetHabbo().AllowPetSpeech;
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE `users` SET `pets_muted` = '" + ((Session.GetHabbo().AllowPetSpeech) ? 1 : 0) + "' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
            }

            if (Session.GetHabbo().AllowPetSpeech)
                Session.SendWhisper("Cambio realizado, ahora no puedes escuchar lo que dicen las mascotas.");
            else
                Session.SendWhisper("Cambio realizado, ahora puedes escuchar lo que dicen las mascotas");
        }
    }
}
