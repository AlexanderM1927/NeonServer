using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class KickCommand : IChatCommand
    {
        public string PermissionRequired => "command_kick";

        public string Parameters => "%username% %reason%";

        public string Description => "Expulsa a un usuario de la sala y le das una razon";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Escribe el nombre del usuario.");
                return;
            }

            GameClient TargetClient = NeonEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Ocurrio un error, al parecer no existe el usuario o no se encuentra online.");
                return;
            }

            if (TargetClient.GetHabbo() == null)
            {
                Session.SendWhisper("Ocurrio un error, al parecer no existe el usuario o no se encuentra online.");
                return;
            }

            if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
            {
                Session.SendWhisper("Get a life.");
                return;
            }

            if (!TargetClient.GetHabbo().InRoom)
            {
                Session.SendWhisper("El usuario no se encuentra en la sala");
                return;
            }

            if (!NeonEnvironment.GetGame().GetRoomManager().TryGetRoom(TargetClient.GetHabbo().CurrentRoomId, out Room TargetRoom))
            {
                return;
            }

            if (Params.Length > 2)
            {
                TargetClient.SendNotification("Un moderador te ha expulsado de la sala por la siguiente razon: " + CommandManager.MergeParams(Params, 2));
            }
            else
            {
                TargetClient.SendNotification("Un moderador te ha expulsado de la sala.");
            }

            TargetRoom.GetRoomUserManager().RemoveUserFromRoom(TargetClient, true, false);
        }
    }
}
