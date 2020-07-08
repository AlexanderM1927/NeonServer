namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class GOTOCommand : IChatCommand
    {
        public string PermissionRequired => "command_goto";

        public string Parameters => "%room_id%";

        public string Description => "";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Usted debe especificar el ID de la sala");
                return;
            }


            if (!int.TryParse(Params[1], out int RoomID))
            {
                Session.SendWhisper("Usted debe escribir correctamente el ID de la sala");
            }
            else
            {
                Room _room = NeonEnvironment.GetGame().GetRoomManager().LoadRoom(RoomID);
                if (_room == null)
                {
                    Session.SendWhisper("Esta sala no existe!");
                }
                else
                {
                    Session.GetHabbo().PrepareRoom(_room.Id, "");
                }
            }
        }
    }
}