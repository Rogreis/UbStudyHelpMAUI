using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using AmadonStandardLib.Helpers;

namespace Amadon.Services
{
    internal static class HelpService
    {
        public static string GetHtml()
        {
            try
            {
                string helpText = File.ReadAllText("Docs//SearchHelp.md");
                return MarkdownToHtml.Convert(helpText);
            }
            catch (Exception ex)
            {
                return $"<p>Error: {ex.Message}";
            }
        }
    }
}
