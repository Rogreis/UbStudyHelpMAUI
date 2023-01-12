using AmadonBlazorLibrary.Helpers;
using AmadonBlazorLibrary.UbClasses;

namespace AmadonBlazor2.Data
{
    public class TOC_Service
    {
        public static Task<TOC_Table> GetTocTableAsync(bool useRightTranslation)
        {
            TOC_Table table = useRightTranslation ? StaticObjects.Book.RightTranslation.TOC : StaticObjects.Book.LeftTranslation.TOC;
            table.Title = $"Tabela de conteúdos em {DateTime.Now}";
            return Task.FromResult(table);
        }
    }
}
