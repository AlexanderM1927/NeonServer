using Neon.Communication.Packets.Outgoing.Inventory.Purse;
using Neon.Database.Interfaces;
using Neon.HabboHotel.Items;
using System;
using System.Data;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class ConvertCreditsCommand : IChatCommand
    {
        public string PermissionRequired => "command_convert_credits";

        public string Parameters => "";

        public string Description => "Lleva tus monedas de inventario a Monedero";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            int TotalValue = 0;

            try
            {
                DataTable Table = null;
                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT `id` FROM `items` WHERE `user_id` = '" + Session.GetHabbo().Id + "' AND (`room_id`=  '0' OR `room_id` = '')");
                    Table = dbClient.getTable();
                }

                if (Table == null)
                {
                    Session.SendWhisper("De momento usted no tiene monedas en su inventario!");
                    return;
                }

                foreach (DataRow Row in Table.Rows)
                {
                    Item Item = Session.GetHabbo().GetInventoryComponent().GetItem(Convert.ToInt32(Row[0]));
                    if (Item == null)
                    {
                        continue;
                    }

                    if (!Item.GetBaseItem().ItemName.StartsWith("CF_"))
                    {
                        continue;
                    }

                    if (Item.RoomId > 0)
                    {
                        continue;
                    }

                    string[] Split = Item.GetBaseItem().ItemName.Split('_');
                    int Value = int.Parse(Split[1]);

                    using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("DELETE FROM `items` WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }

                    Session.GetHabbo().GetInventoryComponent().RemoveItem(Item.Id);

                    TotalValue += Value;

                    if (Value > 0)
                    {
                        Session.GetHabbo().Credits += Value;
                        Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
                    }
                }

                if (TotalValue > 0)
                {
                    Session.SendNotification("Todos sus creditos en inventario se llevaron a su monedero con un !\r\r(Total de: " + TotalValue + " creditos!");
                }
                else
                {
                    Session.SendNotification("Al parecer no tiene ningun otro articulo intercambiable!");
                }
            }
            catch
            {
                Session.SendNotification("Oops, ocurrio un error mientras se intercambiaban sus creditos, contacte un administrador!");
            }
        }
    }
}
