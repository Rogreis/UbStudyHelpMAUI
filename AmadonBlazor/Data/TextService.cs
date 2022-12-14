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

        public static Task<string> GetHtml()
        {
            ParametersMAUI parameters = new ParametersMAUI();
            HtmlFormatMAUI formatter = new HtmlFormatMAUI((Parameters)parameters);
            string html = formatter.FormatPaper(1, StaticObjects.Book.LeftTranslation, null, StaticObjects.Book.RightTranslation);
            return Task.FromResult(html);
        }
    }
}
