using AmadonStandardLib.UbClasses;
using Lucene.Net.Index;
using Lucene.Net.Search;
using System;
using System.Collections.Generic;

namespace AmadonStandardLib.InterchangeData
{
    /// <summary>
    /// Used to send/receive data to/from the book search engine
    /// </summary>
    public class SearchData : InterchangeDataBase
    {
        public bool OrderedByParagraphs { get; set; } = false;

        public Translation? Translation { get; set; } = null;

        public string IndexPathRoot { get; set; } = "";

        public bool Part1Included { get; set; } = true;

        public bool Part2Included { get; set; } = true;

        public bool Part3Included { get; set; } = true;

        public bool Part4Included { get; set; } = true;

        public bool CurrentPaperOnly { get; set; } = false;

        public string QueryString { get; set; } = "";

        public short CurrentPaper { get; set; } = -1;

        public string currentPage { get; set; } = "1";
        public string totalPages { get; set; } = "0";

        public int maxPaginationPages { get; set; } = 25; // Max number of pages
        public int maxPaginationItems { get; set; } = 5;  // Max number of pages shown in the pagination
        public int paginationFirst { get; set; } = 1;     // First page number in pagination
        public int paginationLast { get; set; } = 5;      // Last page number in pagination

        public int maxItemsPerPage { get; set; } = 20;    // Max number of items shown per page
        public int firstItemToShow { get; set; } = 1;     // Number of the first search result to show
        public int lastItemToShwow { get; set; } = 20;    // Number of the last search result to show


        public List<SearchResult> SearchResults { get; set; } = new List<SearchResult>();

        public List<string> Words { get; set; } = new List<string>();

        public bool paginationVisible { get; set; } = false;
        public bool hasDataToShow { get; set; } = false;

        public bool IsPaperIncluded(int PaperNo) =>
                    Part1Included && PaperNo < 32 ||
                    Part2Included && PaperNo >= 32 && PaperNo <= 56 ||
                    Part3Included && PaperNo >= 57 && PaperNo <= 119 ||
                    Part4Included && PaperNo >= 120;

        public void Clear()
        {
            SearchResults.Clear();
            Words.Clear();
            OrderedByParagraphs = false;
            paginationVisible = false;
            hasDataToShow = false;
        }


        ///// <summary>
        ///// Create the word list to be used when highlighting search text found
        ///// </summary>
        ///// <param name="textPrefix"></param>
        ///// <param name="searchString"></param>
        //public void SetSearchString(string textPrefix, string searchString)
        //{
        //    bool continues = true;
        //    int lastStartPos = 0;

        //    searchString = searchString.Replace('~', ' ');
        //    searchString = searchString.Replace('^', ' ');

        //    while (continues)
        //    {
        //        int pos = searchString.IndexOf(textPrefix, lastStartPos);
        //        continues = pos >= 0 && lastStartPos < searchString.Length;
        //        if (continues)
        //        {
        //            int startPos = pos + textPrefix.Length;
        //            char divisor = searchString.ToCharArray()[startPos] == '"' ? '"' : ' ';
        //            if (divisor == '"')
        //            {
        //                startPos += 1;
        //            }
        //            int endPos = searchString.IndexOf(divisor, startPos);
        //            endPos = (endPos >= 0 ? endPos : searchString.Length);
        //            int size = endPos - startPos;
        //            Words.Add(searchString.Substring(startPos, size));
        //            lastStartPos = endPos + 1;
        //        }
        //        else
        //        {
        //            break;
        //        }
        //        continues = lastStartPos < searchString.Length;
        //    }
        //}

        /// <summary>
        /// Create a list of words to highligh in the search
        /// </summary>
        /// <param name="query"></param>
        public void ExtractTerms(Query query)
        {
            var terms = new HashSet<Term>();
            try
            {
                Words.Clear();

                query.ExtractTerms(terms);
                foreach (Term t in terms)
                {
                    Words.Add(t.Text);
                }
            }
            catch
            {
                // Try to get words when ExtractTerms is not implemented
                string queryString = query.ToString().Replace("?", "").Replace("*", "").Replace("~", "").Replace("(", "").Replace(")", "");
                string[] parts = queryString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                Words.AddRange(parts);
            }
        }

        /// <summary>
        /// Sort the results by paragraphs or original order (ranking done by Lucene)
        /// </summary>
        public void SortResults()
        {
            if (SearchResults.Count == 0)
            {
                return;
            }
            if (!OrderedByParagraphs)
            {
                SearchResults.Sort((a, b) => a.Entry.CompareTo(b.Entry));
                OrderedByParagraphs = true;
            }
            else
            {
                SearchResults.Sort((a, b) => a.OriginalPosition.CompareTo(b.OriginalPosition));
                OrderedByParagraphs = false;
            }
        }




    }
}
