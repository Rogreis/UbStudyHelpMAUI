using Amadon.Models;
using UbStandardObjects.Objects;

namespace AmadonBlazor.Classes
{
    internal class TocTreeviewBuilder
    {

        private XamlItemGroup FindParentTOC_Entry(XamlItemGroup group, TOC_Entry TOC_Entry)
        {
            if (group.GroupId == TOC_Entry.ID)
                return group;

            if (group.Children != null)
            {
                foreach (var currentGroup in group.Children)
                {
                    var search = FindParentTOC_Entry(currentGroup, TOC_Entry);

                    if (search != null)
                        return search;
                }
            }

            return null;
        }

        public XamlItemGroup GroupData(Translation trans)
        {
            string title = trans.Description;
            var papers = trans.TableOfContents.FindAll(e => e.Section == 0 && e.ParagraphNo == 0); //.OrderBy(x => x.Paper);

            var companyGroup = new XamlItemGroup();
            companyGroup.Name = title;

            foreach (var entryPaper in papers)
            {
                var itemGroup = new XamlItemGroup();
                itemGroup.Name = entryPaper.Ident;
                itemGroup.GroupId = entryPaper.ID;

                // Section for this paper
                var sections = trans.TableOfContents.FindAll(e => e.Paper == entryPaper.Paper && e.ParagraphNo == 0); //.OrderBy(x => x.Section);
                foreach (var emp in sections)
                {
                    var item = new XamlItem();
                    item.ItemId = emp.ID;
                    item.Key = emp.Text;
                    itemGroup.XamlItems.Add(item);
                }
                companyGroup.Children.Add(itemGroup);
            }
            return companyGroup;
        }



    }
}
