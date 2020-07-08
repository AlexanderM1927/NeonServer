using Neon.Database.Interfaces;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class SetSpeedCommand : IChatCommand
    {
        public string PermissionRequired => "command_setspeed";

        public string Parameters => "%value%";

        public string Description => "Graduar la velocidad de los rollers de 0 a 10.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (!Room.CheckRights(Session, true))
            {
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Ingresa que velocidad desea para los roller.");
                return;
            }

            if (int.TryParse(Params[1], out int Speed))
            {
                Session.GetHabbo().CurrentRoom.GetRoomItemHandler().SetSpeed(Speed);
                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunQuery("UPDATE `rooms` SET `roller_speed` = " + Speed + " WHERE `id` = '" + Room.Id + "' LIMIT 1");
                }
            }
            else
            {
                Session.SendWhisper("Cantidad invalida, solo es permitido en numeros.");
            }
        }
    }
}