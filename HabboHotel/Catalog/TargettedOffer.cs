﻿using Neon.Communication.Packets.Outgoing;
using Neon.Database.Interfaces;
using System.Collections.Generic;

namespace Neon.HabboHotel.Catalog
{
    internal class TargetedOffersManager
    {
        internal TargetedOffers TargetedOffer;

        internal void Initialize(IQueryAdapter dbClient)
        {
            TargetedOffer = null;

            dbClient.SetQuery("SELECT * FROM targeted_offers WHERE active = 'true' LIMIT 1;");
            System.Data.DataRow row = dbClient.getRow();

            if (row == null)
            {
                return;
            }

            TargetedOffer = new TargetedOffers((int)row["id"], (int)row["limit"], (int)row["time"], row["open"].ToString() == "show", row["active"].ToString() == "true", (string)row["code"],
                (string)row["title"], (string)row["description"], (string)row["image"], (string)row["icon"],
                (string)row["money_type"], (string)row["items"], (string)row["price"], (int)row["expire_time"]);
        }
    }


    internal class TargetedOffers
    {
        internal int Id, Limit, Time, Expire;
        internal bool Open, Active;
        internal string Code, Title, Description, Image, Icon, MoneyType;
        internal string[] Items, Price;
        internal List<TargetedItems> Products;

        internal TargetedOffers(int id, int limit, int time, bool open, bool active, string code, string title,
            string description, string image, string icon, string moneyType, string items, string price, int expire)
        {
            Id = id;
            Limit = limit;
            Time = expire - NeonEnvironment.GetIUnixTimestamp();
            Open = open;
            Active = active;
            Code = code;
            Title = title;
            Description = description;
            Image = image;
            Icon = icon;
            MoneyType = moneyType;
            Items = items.Split(';');
            Price = price.Split(';');
            Expire = expire;

            Products = new List<TargetedItems>();
            foreach (string item in Items)
            {
                string itemType = item.Split(',')[0];
                string itemProduct = item.Split(',')[1];
                Products.Add(new TargetedItems(Id, itemType, itemProduct));
            }
        }

        internal int MoneyCode(string moneyType)
        {
            switch (moneyType)
            {
                case "duckets":
                    return 0;

                case "diamonds":
                    return 5;

                default:
                    return 0;
            }
        }

        internal ServerPacket Serialize()
        {
            ServerPacket message = new ServerPacket(ServerPacketHeader.openBoxTargetedOffert);
            message.WriteInteger(Open ? 4 : 1);
            message.WriteInteger(Id);
            message.WriteString(Code);
            message.WriteString(Code);
            message.WriteInteger(int.Parse(Price[0]));
            message.WriteInteger(int.Parse(Price[1]));
            message.WriteInteger(MoneyCode(MoneyType));
            message.WriteInteger(Limit);
            message.WriteInteger(Time);
            message.WriteString(Title);
            message.WriteString(Description);
            message.WriteString(Image);
            message.WriteString(Icon);
            message.WriteInteger(0);
            message.WriteInteger(Products.Count);
            foreach (TargetedItems product in Products)
            {
                message.WriteString(string.Empty);
            }

            return message;
        }
    }

    internal class TargetedItems
    {
        internal int TargetedId;
        internal string ItemType, Item;

        internal TargetedItems(int targetedId, string itemType, string item)
        {
            TargetedId = targetedId;
            ItemType = itemType;
            Item = item;
        }
    }
}
