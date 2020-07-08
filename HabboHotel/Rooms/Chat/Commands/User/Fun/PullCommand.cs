using Neon.Communication.Packets.Outgoing.Rooms.Chat;
using Neon.HabboHotel.GameClients;
using System;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User.Fun
{
    internal class PullCommand : IChatCommand
    {
        public string PermissionRequired => "command_pull";

        public string Parameters => "%target%";

        public string Description => "Atraer a un usuario haia tí.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Introduce el Nombre del usuario el cual quieres atraer");
                return;
            }

            if (!Room.PullEnabled && !Session.GetHabbo().GetPermissions().HasRight("room_override_custom_config"))
            {
                Session.SendWhisper("Oops, el dueño de la sala no permite que hales a otros en su sala");
                return;
            }

            GameClient TargetClient = NeonEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Ocurrio un error, no se consigue al usuario, tal vez no esta online.");
                return;
            }

            RoomUser TargetUser = Room.GetRoomUserManager().GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
            if (TargetUser == null)
            {
                Session.SendWhisper("Ocurrio un error, escribe correctamente el nombre, el usuario NO se encuentra online o en la sala.");
                return;
            }

            if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
            {
                Session.SendWhisper("Vamos! No te puedes jalar a ti mismo..");
                return;
            }

            if (TargetUser.TeleportEnabled)
            {
                Session.SendWhisper("Oops, you cannot push a user whilst they have their teleport mode enabled.");
                return;
            }

            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (ThisUser == null)
            {
                return;
            }

            if (ThisUser.SetX - 1 == Room.GetGameMap().Model.DoorX)
            {
                Session.SendWhisper("Por favor no lo tires de la sala :(!");
                return;
            }


            string PushDirection = "down";
            if (TargetClient.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId && (Math.Abs(ThisUser.X - TargetUser.X) < 3 && Math.Abs(ThisUser.Y - TargetUser.Y) < 3))
            {
                Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*He atraído a " + Params[1] + "*", 0, ThisUser.LastBubble));

                if (ThisUser.RotBody == 0)
                {
                    PushDirection = "up";
                }

                if (ThisUser.RotBody == 2)
                {
                    PushDirection = "right";
                }

                if (ThisUser.RotBody == 4)
                {
                    PushDirection = "down";
                }

                if (ThisUser.RotBody == 6)
                {
                    PushDirection = "left";
                }

                if (PushDirection == "up")
                {
                    TargetUser.MoveTo(ThisUser.X, ThisUser.Y - 1);
                }

                if (PushDirection == "right")
                {
                    TargetUser.MoveTo(ThisUser.X + 1, ThisUser.Y);
                }

                if (PushDirection == "down")
                {
                    TargetUser.MoveTo(ThisUser.X, ThisUser.Y + 1);
                }

                if (PushDirection == "left")
                {
                    TargetUser.MoveTo(ThisUser.X - 1, ThisUser.Y);
                }

                return;
            }
            else
            {
                Session.SendWhisper("Este usuario no esta lo suficientemente cerca, acercate!!");
                return;
            }
        }
    }
}
