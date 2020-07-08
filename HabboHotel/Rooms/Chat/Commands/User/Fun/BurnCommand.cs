using Neon.Communication.Packets.Outgoing.Rooms.Chat;
using Neon.HabboHotel.GameClients;
using System;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User.Fun
{
    internal class BurnCommand : IChatCommand
    {
        public string PermissionRequired => "command_golpe";

        public string Parameters => "%target%";

        public string Description => "Quemar a alguien si la sala lo permite.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("@green@ Introduce el nombre del usuario que deseas quemar. { :quemar NOMBRE }");
                return;
            }

            //if (!Room.QuemarEnabled && !Session.GetHabbo().GetPermissions().HasRight("room_override_custom_config"))
            //{
            //    Session.SendWhisper("@red@ Oops, el dueño de la sala no permite que quemes a otros en su sala.");
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
                Session.SendWhisper("@red@ No está mal que intentes quemarte a ti mismo... pero puede parecer extraño y pensarán que estás loco...");
                return;
            }

            if (TargetUser.TeleportEnabled)
            {
                Session.SendWhisper("Oops, No puedes quemar a alguien si usas teleport.");
                return;
            }

            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (ThisUser == null)
            {
                return;
            }

            if (!((Math.Abs(TargetUser.X - ThisUser.X) >= 2) || (Math.Abs(TargetUser.Y - ThisUser.Y) >= 2)))
            {
                Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "@red@ *¡Empiezo a quemar a " + Params[1] + "!", 0, ThisUser.LastBubble));
                ThisUser.ApplyEffect(5);
                Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "@cyan@ ¡Ayuda! ¡Me están quemando!", 0, ThisUser.LastBubble));
                TargetUser.ApplyEffect(25);
            }
            else
            {
                Session.SendWhisper("@green@ ¡Oops, " + Params[1] + " no está lo suficientemente cerca!");
            }
        }
    }
}
