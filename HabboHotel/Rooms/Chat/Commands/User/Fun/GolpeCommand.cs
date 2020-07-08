using Neon.Communication.Packets.Outgoing.Rooms.Chat;
using Neon.HabboHotel.GameClients;
using System;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User.Fun
{
    internal class GolpeCommand : IChatCommand
    {
        public string PermissionRequired => "command_golpe";

        public string Parameters => "%target%";

        public string Description => "Golpear a alguien si la sala lo permite.";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length == 1 || Params.Length > 2)
            { Session.SendWhisper("@green@ Introduce el nombre del usuario que deseas golpear. { :golpe NOMBRE }"); return; }

            GameClient TargetClient = NeonEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Ocurrió un problema, al parecer el usuario no se encuentra online o usted no escribió bien el nombre");
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
                Session.SendWhisper("@red@ ¿Estás loco o qué te pasa? ¡Maldito masoquista!");
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
                Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "@red@ *" + Params[1] + " ha recibido un golpe en la cara*", 0, ThisUser.LastBubble));
                Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "@cyan@ ¡Menuda ostia men!", 0, ThisUser.LastBubble));

                if (TargetUser.RotBody == 4)
                {
                    TargetUser.Statusses.Add("lay", "1.0 null");
                    TargetUser.Z -= 0.35;
                    TargetUser.isLying = true;
                    TargetUser.UpdateNeeded = true;
                    TargetUser.ApplyEffect(157);
                }

                if (ThisUser.RotBody == 0)
                {
                    TargetUser.Statusses.Add("lay", "1.0 null");
                    TargetUser.Z -= 0.35;
                    TargetUser.isLying = true;
                    TargetUser.UpdateNeeded = true;
                    TargetUser.ApplyEffect(157);
                }

                if (ThisUser.RotBody == 6)
                {
                    TargetUser.Statusses.Add("lay", "1.0 null");
                    TargetUser.Z -= 0.35;
                    TargetUser.isLying = true;
                    TargetUser.UpdateNeeded = true;
                    TargetUser.ApplyEffect(157);
                }

                if (ThisUser.RotBody == 2)
                {
                    TargetUser.Statusses.Add("lay", "1.0 null");
                    TargetUser.Z -= 0.35;
                    TargetUser.isLying = true;
                    TargetUser.UpdateNeeded = true;
                    TargetUser.ApplyEffect(157);
                }

                if (ThisUser.RotBody == 3)
                {
                    TargetUser.Statusses.Add("lay", "1.0 null");
                    TargetUser.Z -= 0.35;
                    TargetUser.isLying = true;
                    TargetUser.UpdateNeeded = true;
                    TargetUser.ApplyEffect(157);
                }

                if (ThisUser.RotBody == 1)
                {
                    TargetUser.Statusses.Add("lay", "1.0 null");
                    TargetUser.Z -= 0.35;
                    TargetUser.isLying = true;
                    TargetUser.UpdateNeeded = true;
                    TargetUser.ApplyEffect(157);
                }

                if (ThisUser.RotBody == 7)
                {
                    TargetUser.Statusses.Add("lay", "1.0 null");
                    TargetUser.Z -= 0.35;
                    TargetUser.isLying = true;
                    TargetUser.UpdateNeeded = true;
                    TargetUser.ApplyEffect(157);
                }

                if (ThisUser.RotBody == 5)
                {
                    TargetUser.Statusses.Add("lay", "1.0 null");
                    TargetUser.Z -= 0.35;
                    TargetUser.isLying = true;
                    TargetUser.UpdateNeeded = true;
                    TargetUser.ApplyEffect(157);
                }

            }
            else
            {
                Session.SendWhisper("@green@ ¡Oops, " + Params[1] + " no está lo suficientemente cerca!");
            }
        }
    }
}
