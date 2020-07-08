using Neon.HabboHotel.Catalog.PredesignedRooms;
using System.Collections.Generic;

namespace Neon.HabboHotel.Catalog
{
    public class BCCatalogPage
    {
        public BCCatalogPage(int Id, int ParentId, string Enabled, string Caption, string PageLink, int Icon, int MinRank, int MinVIP,
              string Visible, string Template, string PageStrings1, string PageStrings2, Dictionary<int, BCCatalogItem> Items,
              Dictionary<int, CatalogDeal> Deals, PredesignedContent PredesignedItems,
              ref Dictionary<int, int> flatOffers)
        {
            this.Id = Id;
            this.ParentId = ParentId;
            this.Enabled = Enabled.ToLower() == "1";
            this.Caption = Caption;
            this.PageLink = PageLink;
            this.Icon = Icon;
            MinimumRank = MinRank;
            MinimumVIP = MinVIP;
            this.Visible = Visible.ToLower() == "1";
            this.Template = Template;

            foreach (string Str in PageStrings1.Split('|'))
            {
                if (this.PageStrings1 == null) { this.PageStrings1 = new List<string>(); }
                this.PageStrings1.Add(Str);
            }

            foreach (string Str in PageStrings2.Split('|'))
            {
                if (this.PageStrings2 == null) { this.PageStrings2 = new List<string>(); }
                this.PageStrings2.Add(Str);
            }

            this.Items = Items;
            this.Deals = Deals;
            this.PredesignedItems = PredesignedItems;

            ItemOffers = new Dictionary<int, BCCatalogItem>();
            foreach (int i in flatOffers.Keys)
            {
                if (flatOffers[i] == Id)
                {
                    foreach (BCCatalogItem item in this.Items.Values)
                    {
                        if (item.OfferId == i)
                        {
                            if (!ItemOffers.ContainsKey(i))
                            {
                                ItemOffers.Add(i, item);
                            }
                        }
                    }
                }
            }

            DealOffers = new Dictionary<int, CatalogDeal>();
            foreach (int i in flatOffers.Keys)
            {
                foreach (CatalogDeal deal in this.Deals.Values)
                {
                    if (!DealOffers.ContainsKey(i))
                    {
                        DealOffers.Add(i, deal);
                    }
                }
            }
        }

        public int Id { get; set; }

        public int ParentId { get; set; }

        public bool Enabled { get; set; }

        public string Caption { get; set; }

        public string PageLink { get; set; }

        public int Icon { get; set; }

        public int MinimumRank { get; set; }

        public int MinimumVIP { get; set; }

        public bool Visible { get; set; }

        public string Template { get; set; }

        public List<string> PageStrings1 { get; private set; }

        public List<string> PageStrings2 { get; private set; }

        public Dictionary<int, BCCatalogItem> Items { get; private set; }

        public Dictionary<int, CatalogDeal> Deals { get; private set; }

        public PredesignedContent PredesignedItems { get; private set; }

        public Dictionary<int, BCCatalogItem> ItemOffers { get; private set; }

        public Dictionary<int, CatalogDeal> DealOffers { get; private set; }

        public BCCatalogItem GetItem(int pId)
        {
            if (Items.ContainsKey(pId))
            {
                return Items[pId];
            }

            return null;
        }
    }
}