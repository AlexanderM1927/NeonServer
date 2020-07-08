using Neon.HabboHotel.Catalog.PredesignedRooms;
using System.Linq;
using System.Text;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Administrator
{
    internal class AddPredesignedCommand : IChatCommand
    {
        public string PermissionRequired => "command_addpredesigned";

        public string Parameters => "";

        public string Description => "Agrega la Sala a las Salas pre-diseñadas";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Room == null)
            {
                return;
            }
            //if (NeonEnvironment.GetGame().GetCatalog().GetPredesignedRooms().Exists((uint)Room.Id))
            //{
            //    Session.SendWhisper("La sala ya está agregada en la lista.");
            //    return;
            //}

            StringBuilder itemAmounts = new StringBuilder(), floorItemsData = new StringBuilder(), wallItemsData = new StringBuilder(),
                decoration = new StringBuilder();
            System.Collections.Generic.ICollection<Items.Item> floorItems = Room.GetRoomItemHandler().GetFloor;
            System.Collections.Generic.ICollection<Items.Item> wallItems = Room.GetRoomItemHandler().GetWall;
            foreach (Items.Item roomItem in floorItems)
            {
                int itemCount = floorItems.Count(item => item.BaseItem == roomItem.BaseItem);
                if (!itemAmounts.ToString().Contains(roomItem.BaseItem + "," + itemCount + ";"))
                {
                    itemAmounts.Append(roomItem.BaseItem + "," + itemCount + ";");
                }

                floorItemsData.Append(roomItem.BaseItem + "$$$$" + roomItem.GetX + "$$$$" + roomItem.GetY + "$$$$" + roomItem.GetZ +
                    "$$$$" + roomItem.Rotation + "$$$$" + roomItem.ExtraData + ";");
            }
            foreach (Items.Item roomItem in wallItems)
            {
                int itemCount = wallItems.Count(item => item.BaseItem == roomItem.BaseItem);
                if (!itemAmounts.ToString().Contains(roomItem.BaseItem + "," + itemCount + ";"))
                {
                    itemAmounts.Append(roomItem.BaseItem + "," + itemCount + ";");
                }

                wallItemsData.Append(roomItem.BaseItem + "$$$$" + roomItem.wallCoord + "$$$$" + roomItem.ExtraData + ";");
            }

            decoration.Append(Room.RoomData.FloorThickness + ";" + Room.RoomData.WallThickness + ";" +
                Room.RoomData.Model.WallHeight + ";" + Room.RoomData.Hidewall + ";" + Room.RoomData.Wallpaper + ";" +
                Room.RoomData.Landscape + ";" + Room.RoomData.Floor);

            using (Database.Interfaces.IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO catalog_predesigned_rooms(room_model,flooritems,wallitems,catalogitems,room_id,room_decoration) VALUES('" + Room.RoomData.ModelName +
                    "', '" + floorItemsData + "', '" + wallItemsData + "', '" + itemAmounts + "', " + Room.Id + ", '" + decoration + "');");
                uint predesignedId = (uint)dbClient.InsertQuery();

                NeonEnvironment.GetGame().GetCatalog().GetPredesignedRooms().predesignedRoom.Add(predesignedId,
                    new PredesignedRooms(predesignedId, (uint)Room.Id, Room.RoomData.ModelName,
                        floorItemsData.ToString().TrimEnd(';'), wallItemsData.ToString().TrimEnd(';'),
                        itemAmounts.ToString().TrimEnd(';'), decoration.ToString()));
            }

            Session.SendWhisper("La sala se ha guardado correctamente a la lista.", 34);



        }
    }
}