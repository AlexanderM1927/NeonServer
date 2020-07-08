using Neon.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class EmptyItems : IChatCommand
    {
        public string PermissionRequired => "command_empty_items";

        public string Parameters => "%yes%";

        public string Description => "Vaciar el inventario de tu personaje.";

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendMessage(new RoomNotificationComposer("Vaciar el inventario:", "<font color='#B40404'><b>¡ATENCIÓN!</b></font>\n\n<font size=\"11\" color=\"#1C1C1C\">La función de vaciar tu inventario borrará todas tus pertenencias.\n" +
                 "Para confirmar, escribe <font color='#B40404'> <b> :empty yes</b></font>. \n\nUna vez hagas esto, no habrá vuelta atrás.\n\n<font color='#B40404'><i>Si no deseas vaciar tu inventario simplemente ignora este mensaje.</i></font>\n\n" +
                 "Suponiendo que dispongas de más de 3000 objetos, aquellos no visibles en tu inventario también serán borrados.", "builders_club_room_locked", ""));
                return;
            }
            else
            {
                if (Params.Length == 2 && Params[1].ToString() == "yes")
                {
                    Session.GetHabbo().GetInventoryComponent().ClearItems();
                    Session.GetHabbo().GetInventoryComponent().ClearBots();
                    Session.GetHabbo().GetInventoryComponent().ClearPets();
                    Session.SendNotification("Su inventario se vació correctamente.");
                    return;
                }
                else if (Params.Length == 2 && Params[1].ToString() != "yes")
                {
                    Session.SendNotification("Para confirmar escribe, :empty yes.");
                    return;
                }
            }
        }
    }
}
