using AmadonStandardLib.Classes;
using AmadonStandardLib.Helpers;
using AmadonStandardLib.UbClasses;
using System.Text;
using System.Text.Json;

namespace AmadonBlazorLibrary.Data
{
    public class PaperText
    {
        public List<string> Titles { get; set; }= new List<string>();
        public List<string> Lines { get; set; }= new List<string>();
    }

    public class TextService
    {
        /// <summary>
        /// Get the paragraphs list from a translations
        /// </summary>
        /// <param name="t"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        private static List<Paragraph> GetParagraphs(Translation t, TOC_Entry entry) 
        {
            return t?.Paper(entry.Paper).Paragraphs;
        }

        /// <summary>
        /// Format a table column "td" for a paragraph
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="par"></param>
        /// <param name="isEdit"></param>
        /// <param name="insertAnchor"></param>
        private static void Columntext(StringBuilder sb, Paragraph par, bool isEdit, bool insertAnchor)
        {
            if (par != null)
            {
                sb.AppendLine("<td>");
                sb.AppendLine(par.GetHtml(isEdit, insertAnchor));
                sb.AppendLine("</td>");
            }
        }

        /// <summary>
        /// Get the formatted text when the paragraphs list is not null
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="list"></param>
        /// <param name="entry"></param>
        /// <param name="isEdit"></param>
        /// <param name="insertAnchor"></param>
        private static void GetText(StringBuilder sb, List<Paragraph> list, TOC_Entry entry, bool isEdit, bool insertAnchor)
        {
            Paragraph par = list?.Find(p => p.Section == entry.Section && p.ParagraphNo == entry.ParagraphNo);
            Columntext(sb, par, isEdit, insertAnchor);
        }

        /// <summary>
        /// Decision about which translations will be shown
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="rightParagraph"></param>
        /// <param name="leftParagraphs"></param>
        /// <param name="middleParagraphs"></param>
        /// <param name="compareParagraphs"></param>
        private static void GetParagraphsLine(StringBuilder sb, Paragraph rightParagraph, 
                                            List<Paragraph> leftParagraphs, 
                                            List<Paragraph> middleParagraphs,
                                            List<Paragraph> compareParagraphs)
        {
            GetText(sb, leftParagraphs, rightParagraph.Entry, false, false);
            GetText(sb, middleParagraphs, rightParagraph.Entry, false, false);
            Columntext(sb, rightParagraph, false, true);
            GetText(sb, compareParagraphs, rightParagraph.Entry, false, false);
        }

        /// <summary>
        /// Format a paper title
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        private static string FormatTitle(Translation trans, TOC_Entry entry)
        {
            return $"<th><div class=\"p-3 mb-2 parClosed\"><h2>{trans.PaperTranslation} {entry.Paper}</h2></div></th>";
        }

        /// <summary>
        /// Service api
        /// </summary>
        /// <param name="href"></param>
        /// <returns>Json string for the object PaperText</returns>
        public static Task<PaperTextFormatted> GetHtml()
        {
            PaperTextFormatted paperTextFormatted = new PaperTextFormatted();
            paperTextFormatted.Entry= StaticObjects.Parameters.Entry;
            PaperText papertext = new PaperText();
            switch (StaticObjects.Parameters.TextShowOption)
            {
                case TextShowOption.LeftOnly:
                    papertext.Titles.Add(FormatTitle(StaticObjects.Book.RightTranslation, paperTextFormatted.Entry));
                    break;
                case TextShowOption.LeftRight:
                    papertext.Titles.Add(FormatTitle(StaticObjects.Book.LeftTranslation, paperTextFormatted.Entry));
                    papertext.Titles.Add(FormatTitle(StaticObjects.Book.RightTranslation, paperTextFormatted.Entry));
                    paperTextFormatted.leftParagraphs = GetParagraphs(StaticObjects.Book.LeftTranslation, paperTextFormatted.Entry);
                    break;
                case TextShowOption.LeftRightCompare:
                    papertext.Titles.Add(FormatTitle(StaticObjects.Book.LeftTranslation, paperTextFormatted.Entry));
                    papertext.Titles.Add(FormatTitle(StaticObjects.Book.RightTranslation, paperTextFormatted.Entry));
                    papertext.Titles.Add("Compare");
                    paperTextFormatted.leftParagraphs = GetParagraphs(StaticObjects.Book.LeftTranslation, paperTextFormatted.Entry);
                    paperTextFormatted.compareParagraphs = null; // TO DO implement compare
                    break;
                case TextShowOption.LeftMiddleRight:
                    papertext.Titles.Add(FormatTitle(StaticObjects.Book.LeftTranslation, paperTextFormatted.Entry));
                    papertext.Titles.Add(FormatTitle(StaticObjects.Book.MiddleTranslation, paperTextFormatted.Entry));
                    papertext.Titles.Add(FormatTitle(StaticObjects.Book.RightTranslation, paperTextFormatted.Entry));
                    paperTextFormatted.leftParagraphs = GetParagraphs(StaticObjects.Book.LeftTranslation, paperTextFormatted.Entry);
                    paperTextFormatted.middleParagraphs = GetParagraphs(StaticObjects.Book.MiddleTranslation, paperTextFormatted.Entry);
                    break;
                case TextShowOption.LeftMiddleRightCompare:
                    papertext.Titles.Add(FormatTitle(StaticObjects.Book.LeftTranslation, paperTextFormatted.Entry));
                    papertext.Titles.Add(FormatTitle(StaticObjects.Book.MiddleTranslation, paperTextFormatted.Entry));
                    papertext.Titles.Add(FormatTitle(StaticObjects.Book.RightTranslation, paperTextFormatted.Entry));
                    papertext.Titles.Add("Compare");
                    paperTextFormatted.leftParagraphs = GetParagraphs(StaticObjects.Book.LeftTranslation, paperTextFormatted.Entry);
                    paperTextFormatted.middleParagraphs = GetParagraphs(StaticObjects.Book.MiddleTranslation, paperTextFormatted.Entry);
                    paperTextFormatted.compareParagraphs = null; // TO DO implement compare
                    break;
            }

            foreach(Paragraph p in paperTextFormatted.rightParagraphs)
            {
                StringBuilder sb = new StringBuilder();
                GetParagraphsLine(sb, p, paperTextFormatted.leftParagraphs, paperTextFormatted.middleParagraphs, paperTextFormatted.compareParagraphs);
                papertext.Lines.Add(sb.ToString());
            }

            return Task.FromResult(paperTextFormatted);
        }
    }
}
