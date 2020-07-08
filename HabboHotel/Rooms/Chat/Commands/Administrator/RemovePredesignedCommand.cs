namespace Neon.HabboHotel.Rooms.Chat.Commands.Administrator
{
    internal class RemovePredesignedCommand : IChatCommand
    {
        public string PermissionRequired => "command_removepredesigned";

        public string Parameters => "";

        public string Description => "Elimina la Sala de la lista de Salas pre-diseñadas";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Room == null)
            {
                return;
            }
            //if (!NeonEnvironment.GetGame().GetCatalog().GetPredesignedRooms().Exists((uint)Room.Id))
            //{
            //    Session.SendWhisper("La sala no existe en la lista.");
            //    return;
            //}

            uint predesignedId = 0U;
            using (Database.Interfaces.IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT id FROM catalog_predesigned_rooms WHERE room_id = " + Room.Id + ";");
                predesignedId = (uint)dbClient.getInteger();

                dbClient.runFastQuery("DELETE FROM catalog_predesigned_rooms WHERE room_id = " + Room.Id + " AND id = " +
                    predesignedId + ";");
            }

            NeonEnvironment.GetGame().GetCatalog().GetPredesignedRooms().predesignedRoom.Remove(predesignedId);
            Session.SendWhisper("La Sala se eliminó correctamente de la lista.");
        }
    }
}