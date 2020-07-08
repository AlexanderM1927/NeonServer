using Neon.Communication.Packets.Outgoing.Rooms.Chat;
using Neon.HabboHotel.GameClients;
using System;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User.Fun
{
    internal class KissCommand : IChatCommand
    {
        public string PermissionRequired => "command_golpe";

        public string Parameters => "%target%";

        public string Description => "Golpear a alguien si la sala lo permite.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("@green@ Introduce el nombre del usuario que deseas besar. { :beso NOMBRE }");
                return;
            }

            //if (!Room.BesosEnabled && !Session.GetHabbo().GetPermissions().HasRight("room_override_custom_config"))
            //{
            //    Session.SendWhisper("@red@ Oops, el dueño de la sala no permite que des besos a otros en su sala.");
            //    return;
            //}

            GameClient TargetClient = NeonEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Ocurrió un problema, al parecer el usuario no se encuentra online o usted no escribio bien el nombre");
                return;
            }

            RoomUser TargetUser = Room.GetRoomUserManager().GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
            if (TargetUser == null)
            {
                Session.SendWhisper("@red@ Ocurrió un error, escribe correctamente el nombre, el usuario NO se encuentra online o en la sala.");
                return;
            }

            if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
            {
                Session.SendWhisper("@red@ No está mal que intentes besarte a ti mismo... pero puede parecer extraño.");
                return;
            }

            if (TargetUser.TeleportEnabled)
            {
                Session.SendWhisper("Oops, No puedes golpear a alguien si usas teleport.");
                return;
            }

            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (ThisUser == null)
            {
                return;
            }

            if (!((Math.Abs(TargetUser.X - ThisUser.X) >= 2) || (Math.Abs(TargetUser.Y - ThisUser.Y) >= 2)))
            {
                Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "@red@ *He besado a " + Params[1] + "*", 0, ThisUser.LastBubble));
                ThisUser.ApplyEffect(9);
                Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "@cyan@ Oh vaya... me han dado un beso :$", 0, ThisUser.LastBubble));
                TargetUser.ApplyEffect(9);
            }
            else
            {
                Session.SendWhisper("@green@ ¡Oops, " + Params[1] + " no está lo suficientemente cerca!");
            }
        }
    }
}
