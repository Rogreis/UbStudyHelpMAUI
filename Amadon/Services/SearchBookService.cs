using AmadonStandardLib.Classes;
using AmadonStandardLib.InterchangeData;
using System.Text.Json;

namespace Amadon.Services
{
    public class SearchBookService
    {

        public static string DoSearch(SearchData searchData)
        {
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                WriteIndented = true,
            };
            LuceneBookSearch luceneBookSearch = new();
            // When an erro occurs, error message is set inside SearchData
            luceneBookSearch.Execute(searchData);
            var jsonString = JsonSerializer.Serialize(searchData, options);
            return jsonString;
        }

        public static Task<string> Search(SearchData searchData)
        {
            return Task.FromResult(DoSearch(searchData));
        }

    }
}
