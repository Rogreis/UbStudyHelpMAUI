using AmadonStandardLib.UbClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadonStandardLib.Classes
{
    /// <summary>
    /// Stores a search result
    /// </summary>
    public class SearchResult
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

    }
}
