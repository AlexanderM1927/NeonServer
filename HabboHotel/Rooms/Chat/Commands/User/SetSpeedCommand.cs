using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Neon.Database.Interfaces;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    class SetSpeedCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_setspeed"; }
        }

        public string Parameters
        {
            get { return "%value%"; }
        }

        public string Description
        {
            get { return "Graduar la velocidad de los rollers de 0 a 10."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (!Room.CheckRights(Session, true))
                return;

            if (Params.Length == 1)
            {
                Session.SendWhisper("Ingresa que velocidad desea para los roller.");
                return;
            }

            int Speed;
            if (int.TryParse(Params[1], out Speed))
            {
                Session.GetHabbo().CurrentRoom.GetRoomItemHandler().SetSpeed(Speed);
                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunQuery("UPDATE `rooms` SET `roller_speed` = " + Speed + " WHERE `id` = '" + Room.Id + "' LIMIT 1");
                }
            }
            else
                Session.SendWhisper("Cantidad invalida, solo es permitido en numeros.");
        }
    }
}