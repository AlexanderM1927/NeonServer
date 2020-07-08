using Neon.Communication.Packets.Outgoing.Inventory.Furni;
using Neon.Database.Interfaces;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Items;
using System.Collections.Generic;
using System.Linq;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class PickAllCommand : IChatCommand
    {
        public string PermissionRequired => "command_pickall";

        public string Parameters => "";

        public string Description => "Recoger todos tus objetos que tengas en la sala.";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (!Room.CheckRights(Session, true))
            {
                return;
            }

            Room.GetRoomItemHandler().RemoveItems(Session);
            Room.GetGameMap().GenerateMaps();

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `items` SET `room_id` = '0' WHERE `room_id` = @RoomId AND `user_id` = @UserId");
                dbClient.AddParameter("RoomId", Room.Id);
                dbClient.AddParameter("UserId", Session.GetHabbo().Id);
                dbClient.RunQuery();
            }

            List<Item> Items = Room.GetRoomItemHandler().GetWallAndFloor.ToList();
            if (Items.Count > 0)
            {
                Session.SendWhisper("Todavía hay más items en esta sala?, elimina manualmente o utilizar :ejectall para expulsarlos!");
            }

            Session.SendMessage(new FurniListUpdateComposer());
        }
    }
}