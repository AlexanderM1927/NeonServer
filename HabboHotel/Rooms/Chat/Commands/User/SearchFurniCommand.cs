using System;
using System.Text;
using System.Data;

using Neon.Database.Interfaces;
using Neon.Communication.Packets.Outgoing.Notifications;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class SearchFurniCommand : IChatCommand
    {
        public string PermissionRequired => "command_enable";

        public string Parameters => "";

        public string Description => "Locate catalog items containing a certain name";

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {
            StringBuilder b = new StringBuilder();
            _ = b.Append("Los items listados contienen " + Params[1] + "\n\n");
            using (IQueryAdapter db = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                db.SetQuery("SELECT * FROM `catalog_items` WHERE `catalog_name` LIKE '%" + Params[1] + "%'");
                db.RunQuery();
                DataTable t = db.getTable();
                foreach (DataRow r in t.Rows)
                {
                    _ = b.Append("Fué encontrado: " + Convert.ToString(r["catalog_name"]) + " en: " + Convert.ToString(r["page_id"]) + "\n");
                }
            }

            Session.SendMessage(new MOTDNotificationComposer(b.ToString()));
        }
    }
}