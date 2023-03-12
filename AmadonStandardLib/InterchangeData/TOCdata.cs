using AmadonStandardLib.UbClasses;
using System.Collections.Generic;

namespace AmadonStandardLib.InterchangeData
{

    public class Item
    {
        public string Text { get; set; } = "";
        public List<Item> Children { get; set; }= new List<Item>();
    }

    /// <summary>
    /// Data passing class for TOC (in/out)
    /// </summary>
    public class TOCdata : InterchangeDataBase
    {
        // Input data
        public short TranslationId1 { get; set; } = -1;

        public short TranslationId2 { get; set; } = -1;

        public short TranslationId3 { get; set; } = -1;

        // Output data
        public TOC_Table? Toc { get; set; } = null;

        private void GetPartPapersSections(TOC_Entry entry, List<Item> children)
        {
            foreach (TOC_Entry entryPaper in entry.Papers)
            {
                Item item = new Item();
                item.Text = entry.Text;
                children.Add(item);

                foreach (TOC_Entry entrySection in entry.Sections)
                {
                    Item itemSection = new Item();
                    itemSection.Text = entrySection.Text;
                    itemSection.Children.Add(item);
                }
            }
        }


        public List<Item> Items
        {
            get
            {
                Item[] itemPart = new Item[4];

                List<Item> itens = new List<Item>();
                if (Toc != null)
                {
                    int indPart = 0;
                    foreach (TOC_Entry entry in Toc.Parts)
                    {
                        itemPart[indPart] = new Item();
                        itemPart[indPart].Text = entry.Text;
                        itens.Add(itemPart[indPart]);
                        GetPartPapersSections(entry, itemPart[indPart].Children);
                    }
                }
                return itens;

                //    List<Item> items = new List<Item>();
                //    if (Toc != null)
                //    {
                //        foreach (TOC_Entry part in Toc.Parts)
                //        {
                //            Item item = new Item();
                //            item.Text = part.Title;
                //            item.Children = Children(part);
                //            items.Add(item);
                //        }
                //    }
                //    return items;
            }

        }
    }
}
