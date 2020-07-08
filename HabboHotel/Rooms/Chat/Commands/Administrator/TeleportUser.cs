using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator.Fun
{
    internal class TeleportUserCommand : IChatCommand
    {
        public string PermissionRequired => "command_teleport_user";

        public string Parameters => "<usuario>";

        public string Description => "Obten la habilidad de teletransportar a un usuario";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length != 2)
            {
                Session.SendWhisper("Introduce el nombre del usuario a quien deseas teleportar!", 34);
                return;
            }

            GameClient TargetClient = NeonEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
            RoomUser Controller = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);

            if (TargetClient != null)
            {
                Session.GetHabbo().Opponent = TargetClient.GetHabbo().Username;
                Session.GetHabbo().IsControlling = true;
                User.TeleportEnabled = !User.TeleportEnabled;
                if (Gamemap.TileDistance(Controller.X, Controller.Y, User.X, User.Y) > 1)
                {
                    User.MoveTo(Controller.SquareInFront);
                }

                Session.GetHabbo().IsControlling = false;
                User.TeleportEnabled = !User.TeleportEnabled;
                Room.GetGameMap().GenerateMaps();
                return;
            }

            else
            {
                Session.SendMessage(RoomNotificationComposer.SendBubble("definitions", "No se ha encontrado el usuario " + Params[1] + ".", ""));
            }
        }
    }
}
