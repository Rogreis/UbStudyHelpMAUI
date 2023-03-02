using AmadonStandardLib.UbClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadonStandardLib.InterchangeData
{
    /// <summary>
    /// Stores a search result
    /// </summary>
    public class SearchResult : InterchangeDataBase
    {
        public TOC_Entry Entry { get; set; }

        public int OriginalPosition { get; set; } = -1;

        public string Text
        {
            get
            {
                return Entry.Text;
            }
        }


        public SearchResult(TOC_Entry entry)
        {
            Entry = entry;
        }

        public override string ToString()
        {
            return Entry.ToShortString();
        }

    }
}
