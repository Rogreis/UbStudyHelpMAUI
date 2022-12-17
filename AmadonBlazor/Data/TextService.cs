using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UbStandardObjects.Objects;
using UbStandardObjects;
using AmadonBlazor.Classes;

namespace AmadonBlazor.Data
{
    public class TextService
    {

        public static Task<string> GetHtml(string href)
        {
            TOC_Entry entry = TOC_Entry.FromHref(href);
            ParametersMAUI parameters = new ParametersMAUI();
            HtmlFormatMAUI formatter = new HtmlFormatMAUI((Parameters)parameters);
            string html = formatter.FormatPaper(entry.Paper, StaticObjects.Book.LeftTranslation, null, StaticObjects.Book.RightTranslation);
            return Task.FromResult(html);
        }
    }
}
