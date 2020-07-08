using Neon.Communication.Packets.Outgoing.Rooms.Chat;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator.Fun
{
    internal class MakeSayCommand : IChatCommand
    {
        public string PermissionRequired => "command_makesay";

        public string Parameters => "%username% %message%";

        public string Description => "Obligas a un usuario a decir el mensaje que desees.";

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {
            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (ThisUser == null)
            {
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Escribe correctamente el nombre del usuario");
            }
            else
            {
                string Message = CommandManager.MergeParams(Params, 2);
                RoomUser TargetUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Params[1]);
                if (TargetUser != null)
                {
                    if (TargetUser.GetClient() != null && TargetUser.GetClient().GetHabbo() != null)
                    {
                        if (!TargetUser.GetClient().GetHabbo().GetPermissions().HasRight("mod_make_say_any"))
                        {
                            Room.SendMessage(new ChatComposer(TargetUser.VirtualId, Message, 0, TargetUser.LastBubble));
                        }
                        else if (Session.GetHabbo().Rank < TargetUser.GetClient().GetHabbo().Rank)
                        {
                            Session.SendWhisper("El usuario no puede decir eso.");
                        }
                    }
                }
                else
                {
                    Session.SendWhisper("El usuario no se encuentra en la sala.");
                }
            }
        }
    }
}
