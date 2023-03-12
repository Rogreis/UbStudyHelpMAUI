using AmadonStandardLib.Classes;
using AmadonStandardLib.Helpers;
using AmadonStandardLib.InterchangeData;
using AmadonStandardLib.UbClasses;
using System.Text.Json;

namespace AmadonBlazorLibrary.Data
{
    public class TOC_Service
    {
        public static TOCdata GetToc(TOCdata data)
        {
            Translation translation = StaticObjects.Book.GetTranslation(data.TranslationId1);
            data.Toc = translation.TOC;
            data.Toc.Title = $"TOC {translation.Description}";
            return data;
        }

        public static Task<TOCdata> GetTocTable(TOCdata data)
        {
            return Task.FromResult(GetToc(data));
        }
    }
}
