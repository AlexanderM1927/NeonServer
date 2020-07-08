using Neon.Communication.Packets.Outgoing.Inventory.Bots;
using Neon.Database.Interfaces;
using Neon.HabboHotel.Users.Inventory.Bots;
using System;
using System.Linq;


namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class KickBotsCommand : IChatCommand
    {
        public string PermissionRequired => "command_kickbots";

        public string Parameters => "";

        public string Description => "Expulsar a todos los BOTs dentro de tu sala.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (!Room.CheckRights(Session, true))
            {
                Session.SendWhisper("Oops, Al parecer el dueño de la sala no permite que ejecutes este comando");
                return;
            }

            foreach (RoomUser User in Room.GetRoomUserManager().GetUserList().ToList())
            {
                if (User == null || User.IsPet || !User.IsBot)
                {
                    continue;
                }

                if (!Room.GetRoomUserManager().TryGetBot(User.BotData.Id, out RoomUser BotUser))
                {
                    return;
                }

                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("UPDATE `bots` SET `room_id` = '0' WHERE `id` = @id LIMIT 1");
                    dbClient.AddParameter("id", User.BotData.Id);
                    dbClient.RunQuery();
                }

                Session.GetHabbo().GetInventoryComponent().TryAddBot(new Bot(Convert.ToInt32(BotUser.BotData.Id), Convert.ToInt32(BotUser.BotData.ownerID), BotUser.BotData.Name, BotUser.BotData.Motto, BotUser.BotData.Look, BotUser.BotData.Gender));
                Session.SendMessage(new BotInventoryComposer(Session.GetHabbo().GetInventoryComponent().GetBots()));
                Room.GetRoomUserManager().RemoveBot(BotUser.VirtualId, false);
            }

            Session.SendWhisper("Todos los bots fueron removidos.");
        }
    }
}
