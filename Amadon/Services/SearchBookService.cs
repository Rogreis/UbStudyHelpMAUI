using AmadonStandardLib.Classes;
using AmadonStandardLib.InterchangeData;
using System.Text.Json;

namespace Amadon.Services
{
    public class SearchBookService
    {

        public static SearchData DoSearch(SearchData searchData)
        {
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                WriteIndented = true,
            };
            LuceneBookSearch luceneBookSearch = new();
            // When an erro occurs, error message is set inside SearchData
            luceneBookSearch.Execute(searchData);
            return searchData;
         }

        public static Task<SearchData> Search(SearchData searchData)
        {
            return Task.FromResult(DoSearch(searchData));
        }

    }
}
