using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.GameClients;


namespace Neon.HabboHotel.Rooms.Chat.Commands.Events
{
    internal class CatalogUpdateAlert : IChatCommand
    {
        public string PermissionRequired => "command_addpredesigned";
        public string Parameters => "%message%";
        public string Description => "Avisar de una actualización en el catálogo del hotel.";
        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            string Message = CommandManager.MergeParams(Params, 1);
            NeonEnvironment.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("¡Actualización en el catálogo!",
              "¡El catálogo de <font color=\"#2E9AFE\"><b>Keko</b></font> acaba de ser actualizado! Si quieres observar <b>las novedades</b> sólo debes hacer click en el botón de abajo.<br>", "cata", "Ir a la página", "event:catalog/open/" + Message));

            Session.SendWhisper("Catalogo actualizado satisfactoriamente.");
        }
    }
}

