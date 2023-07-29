using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using AmadonStandardLib.Helpers;
using System.Text.RegularExpressions;

namespace Amadon.Services
{
    internal static class HelpService
    {
        public static string StartPage = "wwwroot/help.html";

        public static void ExtractButtonAttributes(string input)
        {
            var pageMatch = Regex.Match(input, "page=\"(.*?)\"");
            var textMatch = Regex.Match(input, "text=\"(.*?)\"");

            if (pageMatch.Success && textMatch.Success)
            {
                string page = pageMatch.Groups[1].Value;
                string text = textMatch.Groups[1].Value;

                Console.WriteLine($"Page: {page}, Text: {text}");
            }
            else
            {
                Console.WriteLine("No match found");
            }
        }


        public async static Task<string> GetHelpFile(string relativePath)
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(relativePath);
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }


        public async static Task<string> GetHtml(string htmlPageRelativePath)
        {
            try
            {
                string html = await GetHelpFile(htmlPageRelativePath);
                return html;
            }
            catch (Exception ex)
            {
                return $"<p>Error: {ex.Message}</p>";
            }
        }
    }
}
