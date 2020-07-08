using Neon.Communication.Packets.Outgoing.Rooms.Engine;
using Neon.Database.Interfaces;


namespace Neon.HabboHotel.Rooms.Chat.Commands.User.Fun
{
    internal class FacelessCommand : IChatCommand
    {
        public string PermissionRequired => "command_faceless";

        public string Parameters => "";

        public string Description => "Eliminar la cara de tu personaje.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null || User.GetClient() == null)
            {
                return;
            }

            string[] headParts;
            string[] figureParts = Session.GetHabbo().Look.Split('.');
            foreach (string Part in figureParts)
            {
                if (Part.StartsWith("hd"))
                {
                    headParts = Part.Split('-');
                    if (!headParts[1].Equals("99999"))
                    {
                        headParts[1] = "99999";
                    }
                    else
                    {
                        return;
                    }

                    Session.GetHabbo().Look = Session.GetHabbo().Look.Replace(Part, "hd-" + headParts[1] + "-" + headParts[2]);
                    break;
                }
            }
            Session.GetHabbo().Look = NeonEnvironment.GetGame().GetAntiMutant().RunLook(Session.GetHabbo().Look);
            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE `users` SET `look` = '" + Session.GetHabbo().Look + "' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
            }

            Session.SendMessage(new UserChangeComposer(User, true));
            Session.GetHabbo().CurrentRoom.SendMessage(new UserChangeComposer(User, false));
            return;
        }
    }
}
