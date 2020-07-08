using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.Database.Interfaces;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class SetMaxCommand : IChatCommand
    {
        public string PermissionRequired => "command_setmax";

        public string Parameters => "%límite%";

        public string Description => "Aumenta o reduce el aforo máximo en tu sala.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (!Room.CheckRights(Session, true))
            {
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Introduce una cantidad correcta (en números) de cuantos usuarios pueden ingresar a tu sala.");
                return;
            }

            if (int.TryParse(Params[1], out int MaxAmount))
            {
                if (MaxAmount <= 0)
                {
                    MaxAmount = 10;
                    Session.SendWhisper("Cantidad de visitantes demasiado baja, la cantidad de visitantes se ha establecido en 10.");
                }
                else if (MaxAmount > 250 && !Session.GetHabbo().GetPermissions().HasRight("override_command_setmax_limit"))
                {
                    MaxAmount = 250;
                    Session.SendWhisper("Cantidad de visitantes demasiado alta, la cantidad de visitantes se ha establecido en 250.");
                }
                else
                {
                    Room.UsersMax = MaxAmount;
                }

                Room.RoomData.UsersMax = MaxAmount;
                Room.SendMessage(RoomNotificationComposer.SendBubble("setmax", "" + Session.GetHabbo().Username + " ha establecido el límite de aforo a " + MaxAmount + ".", ""));

                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunQuery("UPDATE `rooms` SET `users_max` = " + MaxAmount + " WHERE `id` = '" + Room.Id + "' LIMIT 1");
                }
            }
            else
            {
                Session.SendWhisper("Cantidad invalida, solo es permitidos numeros.");
            }
        }
    }
}
