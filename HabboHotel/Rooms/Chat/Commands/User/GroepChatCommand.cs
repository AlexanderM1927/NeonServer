using Neon.Communication.Packets.Outgoing.Messenger;
using Neon.Database.Interfaces;
using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class GroepChatCommand : IChatCommand
    {
        public string PermissionRequired => "command_groepchat";

        public string Parameters => "";

        public string Description => "Activa los grupos ON/OFF";


        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length < 2)
            {
                Session.SendWhisper("Ha ocurrido un error, especifica ON / OFF");
                return;
            }

            if (!Room.CheckRights(Session, true))
            {
                Session.SendWhisper("No tienes permisos.");
                return;
            }

            if (Room.Group == null)
            {
                Session.SendWhisper("Esta sala no tiene grupo, si lo acabas de crear haz :unload");
                return;
            }

            if (Room.Group.Id != Session.GetHabbo().GetStats().FavouriteGroupId)
            {
                Session.SendWhisper("Solo se puede crear un grupo si lo tienes como favorito.");
                return;
            }

            string mode = Params[1].ToLower();
            Groups.Group group = Room.Group;

            if (mode == "on")
            {
                if (group.HasChat)
                {
                    Session.SendWhisper("Este grupo ya tiene chat.");
                    return;
                }

                group.HasChat = true;

                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("UPDATE groups SET has_chat = '1' WHERE id = @id");
                    dbClient.AddParameter("id", group.Id);
                    dbClient.RunQuery();
                }

                GameClient Client = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(Session.GetHabbo().Id);
                if (Client != null)
                {
                    Client.SendMessage(new FriendListUpdateComposer(group, 1));
                }

            }
            else if (mode == "off")
            {
                if (!group.HasChat)
                {
                    Session.SendWhisper("Este grupo no tiene chat aun.");
                    return;
                }

                group.HasChat = false;

                using (IQueryAdapter adap = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    adap.SetQuery("UPDATE groups SET has_chat = '0' WHERE id = @id");
                    adap.AddParameter("id", group.Id);
                    adap.RunQuery();
                }
                GameClient Client = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(Session.GetHabbo().Id);
                if (Client != null)
                {
                    Client.SendMessage(new FriendListUpdateComposer(group, -1));
                }
            }
            else
            {
                Session.SendNotification("Ha ocurrido un error");
            }


        }
    }
}
