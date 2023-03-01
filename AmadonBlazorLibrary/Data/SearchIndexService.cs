using AmadonStandardLib.Classes;
using System.Text.Json;

namespace AmadonBlazorLibrary.Data
{
    public class SearchIndexService
    {
        public static string DoSearch(SearchIndexData searchIndexData)
        {
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                WriteIndented = true,
            };
            LuceneIndexSearch IndexSearch = new();
            // When an erro occurs, error message is set inside SearchIndexData
            IndexSearch.Execute(searchIndexData);
            var jsonString = JsonSerializer.Serialize(searchIndexData, options);
            return jsonString;
        }

        public static Task<string> Search(SearchIndexData searchIndexData)
        {
            return Task.FromResult(DoSearch(searchIndexData));
        }
    }
}
