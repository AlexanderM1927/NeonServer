using Neon.Communication.Packets.Outgoing.Rooms.Engine;
using Neon.Database.Interfaces;
using Neon.HabboHotel.GameClients;


namespace Neon.HabboHotel.Rooms.Chat.Commands.User.Fun
{
    internal class MimicCommand : IChatCommand
    {
        public string PermissionRequired => "command_mimic";

        public string Parameters => "%username%";

        public string Description => "Copiar la ropa de otro usuario conectad@.";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Introduce el nombre del usuari@ al cual le quieres copiar la ropa.");
                return;
            }

            GameClient TargetClient = NeonEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Ocurrió un error, escribe correctamente el nombre o tal vez el usuario no se encuentre online.");
                return;
            }

            if (!TargetClient.GetHabbo().AllowMimic)
            {
                Session.SendWhisper(Params[1] + " tiene desactivada la opción de que le copien el look.");
                return;
            }

            RoomUser TargetUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
            if (TargetUser == null)
            {
                Session.SendWhisper("Ocurrió un error, escribe correctamente el nombre o tal vez el usuario no se encuentre online.");
                return;
            }

            Session.GetHabbo().Gender = TargetUser.GetClient().GetHabbo().Gender;
            Session.GetHabbo().Look = TargetUser.GetClient().GetHabbo().Look;

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `gender` = @gender, `look` = @look WHERE `id` = @id LIMIT 1");
                dbClient.AddParameter("gender", Session.GetHabbo().Gender);
                dbClient.AddParameter("look", Session.GetHabbo().Look);
                dbClient.AddParameter("id", Session.GetHabbo().Id);
                dbClient.RunQuery();
            }

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User != null)
            {
                Session.SendMessage(new AvatarAspectUpdateMessageComposer(Session.GetHabbo().Look, Session.GetHabbo().Gender));
                Session.SendMessage(new UserChangeComposer(User, true));
                Room.SendMessage(new UserChangeComposer(User, false));
            }
        }
    }
}