using AmadonStandardLib.Classes;
using AmadonStandardLib.Helpers;
using AmadonStandardLib.InterchangeData;
using AmadonStandardLib.UbClasses;
using System.Text.Json;

namespace AmadonBlazorLibrary.Data
{
    public class TOC_Service
    {
        public static string GetToc(TOCdata data)
        {
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                WriteIndented = true, 
                IncludeFields = true, 
            };
            Translation translation = StaticObjects.Book.GetTranslation(data.TranslationId);
            data.Toc = translation.TOC;
            data.Toc.Title = $"TOC {translation.Description}";
            var jsonString = JsonSerializer.Serialize(data, options);
            return jsonString;
        }

        public static Task<string> GetTocTable(TOCdata data)
        {
            return Task.FromResult(GetToc(data));
        }
    }
}
