using AmadonStandardLib.Classes;
using AmadonStandardLib.Helpers;
using AmadonStandardLib.UbClasses;
using System.Text.Json;

namespace AmadonBlazorLibrary.Data
{
    public class TOC_Service
    {
        public static string GetToc(short translationId)
        {
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                WriteIndented = true, 
                IncludeFields = true, 
            };
            Translation translation = StaticObjects.Book.GetTranslation(translationId);
            TOC_Table table = translation.TOC;
            table.Title = $"TOC {translation.Description}";
            var jsonString = JsonSerializer.Serialize(table, options);
            return jsonString;
        }

        public static Task<string> GetTocTable(short translationId)
        {
            return Task.FromResult(GetToc(translationId));
        }
    }
}
