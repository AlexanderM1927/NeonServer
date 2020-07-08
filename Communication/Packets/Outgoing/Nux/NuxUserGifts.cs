using Neon.Database.Interfaces;

namespace Neon.Communication.Packets.Outgoing.Nux
{

    internal class NuxUserGiftsManager
    {
        internal NuxUserGifts NuxUserGifts;

        internal void Initialize(IQueryAdapter dbClient)
        {
            NuxUserGifts = null;

            dbClient.SetQuery("SELECT * FROM nuxgifts_frontpage");
            System.Data.DataRow row = dbClient.getRow();

            if (row == null)
            {
                return;
            }

            NuxUserGifts = new NuxUserGifts((string)row["image_url"], (string)row["product_name"]);
        }
    }


    internal class NuxUserGifts
    {
        internal string[] Images, Items;
        internal NuxUserGifts(string images, string items)
        {
            Images = images.Split(';');
            Items = items.Split(';');
        }

        internal ServerPacket Serialize()
        {
            ServerPacket message = new ServerPacket(ServerPacketHeader.NuxItemListComposer);
            message.WriteInteger(1); // Número de páginas.

            message.WriteInteger(1); // ELEMENTO 1
            message.WriteInteger(3); // ELEMENTO 2
            message.WriteInteger(3); // Número total de premios:

            message.WriteString(Images[0]); // image.library.url + string
            message.WriteInteger(1); // items:
            message.WriteString(Items[0]); // item_name (product_x_name)
            message.WriteString(string.Empty); // can be null

            message.WriteString(Images[1]);
            message.WriteInteger(1);
            message.WriteString(Items[1]);
            message.WriteString(string.Empty);

            message.WriteString(Images[2]);
            message.WriteInteger(1);
            message.WriteString(Items[2]);
            message.WriteString(string.Empty);
            return message;

        }
    }
}
