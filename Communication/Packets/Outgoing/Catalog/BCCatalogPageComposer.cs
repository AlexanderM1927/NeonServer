using Neon.HabboHotel.Catalog;
using Neon.HabboHotel.Items;
using System.Collections.Generic;
using System.Linq;

namespace Neon.Communication.Packets.Outgoing.Catalog
{
    public class BCCatalogPageComposer : ServerPacket
    {
        public BCCatalogPageComposer(BCCatalogPage Page, string CataMode)
            : base(ServerPacketHeader.CatalogPageMessageComposer)
        {
            WriteInteger(Page.Id);
            WriteString(CataMode);
            WriteString(Page.Template);

            WriteInteger(Page.PageStrings1.Count);
            foreach (string s in Page.PageStrings1)
            {
                WriteString(s);
            }

            WriteInteger(Page.PageStrings2.Count);
            foreach (string s in Page.PageStrings2)
            {
                WriteString(s);
            }

            if (!Page.Template.Equals("frontpage") && !Page.Template.Equals("club_buy"))
            {

                WriteInteger(Page.Items.Count);
                foreach (BCCatalogItem Item in Page.Items.Values)
                {
                    WriteInteger(Item.Id);
                    WriteString(Item.Name);
                    WriteBoolean(false);//IsRentable
                    WriteInteger(Item.CostCredits);

                    if (Item.CostDiamonds > 0)
                    {
                        WriteInteger(Item.CostDiamonds);
                        WriteInteger(5); // Diamonds
                    }
                    else if (Item.CostGOTWPoints > 0)
                    {
                        WriteInteger(Item.CostGOTWPoints);
                        WriteInteger(103); // Pixeles
                    }
                    else
                    {
                        WriteInteger(Item.CostPixels);
                        WriteInteger(0); // Type of PixelCost
                    }
                    WriteBoolean(false);
                    if (Item.Data.InteractionType == InteractionType.DEAL)
                    {
                        foreach (CatalogDeal Deal in Page.Deals.Values)
                        {
                            WriteInteger(Deal.ItemDataList.Count);//Count

                            foreach (CatalogItem DealItem in Deal.ItemDataList.ToList())
                            {
                                WriteString(DealItem.Data.Type.ToString());
                                WriteInteger(DealItem.Data.SpriteId);
                                WriteString("");
                                WriteInteger(DealItem.Amount);
                                WriteBoolean(false);
                            }
                            WriteInteger(0);//club_level
                            WriteBoolean(false);
                            WriteBoolean(true);
                            WriteString("");
                        }
                    }
                    else
                    {
                        if (Item.PredesignedId > 0)
                        {
                            WriteInteger(Page.PredesignedItems.Items.Count);
                            foreach (KeyValuePair<int, int> predesigned in Page.PredesignedItems.Items.ToList())
                            {
                                if (NeonEnvironment.GetGame().GetItemManager().GetItem(predesigned.Key, out ItemData Data)) { }
                                WriteString(Data.Type.ToString());
                                WriteInteger(Data.SpriteId);
                                WriteString(string.Empty);
                                WriteInteger(predesigned.Value);
                                WriteBoolean(false);
                            }

                            WriteInteger(0);
                            WriteBoolean(false);
                            WriteBoolean(true); // Niu Rilí
                            WriteString(""); // Niu Rilí
                        }
                        else
                        {
                            WriteInteger(string.IsNullOrEmpty(Item.Badge) ? 1 : 2);//Count 1 item if there is no badge, otherwise count as 2.

                            if (!string.IsNullOrEmpty(Item.Badge))
                            {
                                base.WriteString("b");
                                base.WriteString(Item.Badge);
                            }

                            base.WriteString(Item.Data.Type.ToString());
                            if (Item.Data.Type.ToString().ToLower() == "b")
                            {
                                //This is just a badge, append the name.
                                base.WriteString(Item.Data.ItemName);
                            }
                            else
                            {
                                base.WriteInteger(Item.Data.SpriteId);
                                if (Item.Data.InteractionType == InteractionType.WALLPAPER || Item.Data.InteractionType == InteractionType.FLOOR || Item.Data.InteractionType == InteractionType.LANDSCAPE)
                                {
                                    base.WriteString(Item.Name.Split('_')[2]);
                                }
                                else if (Item.Data.InteractionType == InteractionType.BOT)//Bots
                                {
                                    if (!NeonEnvironment.GetGame().GetCatalog().TryGetBot(Item.ItemId, out CatalogBot CatalogBot))
                                    {
                                        base.WriteString("hd-180-7.ea-1406-62.ch-210-1321.hr-831-49.ca-1813-62.sh-295-1321.lg-285-92");
                                    }
                                    else
                                    {
                                        base.WriteString(CatalogBot.Figure);
                                    }
                                }
                                else if (Item.ExtraData != null)
                                {
                                    base.WriteString(Item.ExtraData != null ? Item.ExtraData : string.Empty);
                                }
                                base.WriteInteger(Item.Amount);
                                base.WriteBoolean(Item.IsLimited); // IsLimited
                                if (Item.IsLimited)
                                {
                                    base.WriteInteger(Item.LimitedEditionStack);
                                    base.WriteInteger(Item.LimitedEditionStack - Item.LimitedEditionSells);
                                }
                            }
                            base.WriteInteger(0); //club_level
                            base.WriteBoolean(false);

                            base.WriteBoolean(true); // Niu Rilí
                            base.WriteString(""); // Niu Rilí
                        }

                    }
                }

                //}
                /*}*/
            }
            else
            {
                base.WriteInteger(0);
            }

            base.WriteInteger(-1);
            base.WriteBoolean(false);

            if (Page.Template == "frontpage4")
            {
                ICollection<Frontpage> FrontPage = NeonEnvironment.GetGame().GetCatalogFrontPageManager().GetBCCatalogFrontPage();
                base.WriteInteger(FrontPage.Count); // count

                foreach (Frontpage front in FrontPage.ToList<Frontpage>())
                {
                    base.WriteInteger(front.Id());
                    base.WriteString(front.FrontName());
                    base.WriteString(front.FrontImage());
                    base.WriteInteger(0);
                    base.WriteString(front.FrontLink());
                    base.WriteInteger(-1);

                }
            }
        }
    }
}