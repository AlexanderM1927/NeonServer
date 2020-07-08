using Neon.Communication.Packets.Outgoing.Rooms.Chat;
using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator.Fun
{
    internal class SuperPullCommand : IChatCommand
    {
        public string PermissionRequired => "command_super_pull";

        public string Parameters => "%username%";

        public string Description => "Hala a alguien sin liminte alguno";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Introduce el nombre del usuario que deseas hacer el super pull.");
                return;
            }

            if (!Room.SPullEnabled && !Room.CheckRights(Session, true) && !Session.GetHabbo().GetPermissions().HasRight("room_override_custom_config"))
            {
                Session.SendWhisper("Oops, al parecer el dueño de la sala ha prohibido hacer los super pull en su sala.");
                return;
            }

            GameClient TargetClient = NeonEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Hay un Error, no se encuentra al usuario online o no se encuentra en la sala.");
                return;
            }

            RoomUser TargetUser = Room.GetRoomUserManager().GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
            if (TargetUser == null)
            {
                Session.SendWhisper("Hay un Error, no se encuentra al usuario online o no se encuentra en la sala..");
                return;
            }

            if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
            {
                Session.SendWhisper("Vamos.. Seguramente usted no se querra empujar a si mismo!");
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
                Session.SendWhisper("Por favor, no tire a ese usuario fuera de la Habitacion :c ");
                return;
            }

            if (ThisUser.RotBody % 2 != 0)
            {
                ThisUser.RotBody--;
            }

            if (ThisUser.RotBody == 0)
            {
                TargetUser.MoveTo(ThisUser.X, ThisUser.Y - 1);
            }
            else if (ThisUser.RotBody == 2)
            {
                TargetUser.MoveTo(ThisUser.X + 1, ThisUser.Y);
            }
            else if (ThisUser.RotBody == 4)
            {
                TargetUser.MoveTo(ThisUser.X, ThisUser.Y + 1);
            }
            else if (ThisUser.RotBody == 6)
            {
                TargetUser.MoveTo(ThisUser.X - 1, ThisUser.Y);
            }

            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*super pulls " + Params[1] + " to them*", 0, ThisUser.LastBubble));
            return;
        }
    }
}