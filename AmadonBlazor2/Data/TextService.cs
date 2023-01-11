using AmadonBlazorLibrary.Data;
using AmadonBlazorLibrary.Helpers;
using AmadonBlazorLibrary.UbClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AmadonBlazor2.Data
{
    public class PaperText
    {
        public List<string> Titles { get; set; } = new List<string>();
        public List<string> Lines { get; set; } = new List<string>();
    }

    public class TextService
    {
        private static bool Initialized = false;  // Controls the initialization process to be executed once

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

        public static TOC_Entry Entry { get; set; } = null;

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
            if (list != null)
            {
                Paragraph par = list?.Find(p => p.Section == entry.Section && p.ParagraphNo == entry.ParagraphNo);
                Columntext(sb, par, isEdit, insertAnchor);
            }
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

        public static bool Initialize()
        {
            if (!Initialized)
            {
                try
                {
                    Initializer initializer = Initializer.Instance;
                    initializer.Log();
                    initializer.GetParameters();
                    initializer.StartTubRepository();

                    if (Entry == null)
                    {
                        Entry = StaticObjects.Parameters.Entry; 
                    }
                    Initialized = true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return Initialized;
        }

        /// <summary>
        /// Service api
        /// </summary>
        /// <param name="href"></param>
        /// <returns>Json string for the object PaperText</returns>
        public static Task<string> GetHtml(string href = null)
        {
            try
            {
                TOC_Entry entry = href == null? StaticObjects.Parameters.Entry : TOC_Entry.FromHref(href);

                List<Paragraph> leftParagraphs = null;
                List<Paragraph> rightParagraphs = GetParagraphs(StaticObjects.Book.RightTranslation, entry);
                List<Paragraph> middleParagraphs = null;
                List<Paragraph> compareParagraphs = null;

                PaperText papertext = new PaperText();
                switch (StaticObjects.Parameters.TextShowOption)
                {
                    case TextShowOption.RightOnly:
                        papertext.Titles.Add(FormatTitle(StaticObjects.Book.RightTranslation, entry));
                        break;
                    case TextShowOption.LeftRight:
                        papertext.Titles.Add(FormatTitle(StaticObjects.Book.LeftTranslation, entry));
                        papertext.Titles.Add(FormatTitle(StaticObjects.Book.RightTranslation, entry));
                        leftParagraphs = GetParagraphs(StaticObjects.Book.LeftTranslation, entry);
                        break;
                    case TextShowOption.LeftRightCompare:
                        papertext.Titles.Add(FormatTitle(StaticObjects.Book.LeftTranslation, entry));
                        papertext.Titles.Add(FormatTitle(StaticObjects.Book.RightTranslation, entry));
                        papertext.Titles.Add("Compare");
                        leftParagraphs = GetParagraphs(StaticObjects.Book.LeftTranslation, entry);
                        compareParagraphs = null; // TO DO implement compare
                        break;
                    case TextShowOption.LeftMiddleRight:
                        papertext.Titles.Add(FormatTitle(StaticObjects.Book.LeftTranslation, entry));
                        papertext.Titles.Add(FormatTitle(StaticObjects.Book.MiddleTranslation, entry));
                        papertext.Titles.Add(FormatTitle(StaticObjects.Book.RightTranslation, entry));
                        leftParagraphs = GetParagraphs(StaticObjects.Book.LeftTranslation, entry);
                        middleParagraphs = GetParagraphs(StaticObjects.Book.MiddleTranslation, entry);
                        break;
                    case TextShowOption.LeftMiddleRightCompare:
                        papertext.Titles.Add(FormatTitle(StaticObjects.Book.LeftTranslation, entry));
                        papertext.Titles.Add(FormatTitle(StaticObjects.Book.MiddleTranslation, entry));
                        papertext.Titles.Add(FormatTitle(StaticObjects.Book.RightTranslation, entry));
                        papertext.Titles.Add("Compare");
                        leftParagraphs = GetParagraphs(StaticObjects.Book.LeftTranslation, entry);
                        middleParagraphs = GetParagraphs(StaticObjects.Book.MiddleTranslation, entry);
                        compareParagraphs = null; // TO DO implement compare
                        break;
                }

                foreach (Paragraph p in rightParagraphs)
                {
                    StringBuilder sb = new StringBuilder();
                    GetParagraphsLine(sb, p, leftParagraphs, middleParagraphs, compareParagraphs);
                    papertext.Lines.Add(sb.ToString());
                }

                var options = new JsonSerializerOptions
                {
                    AllowTrailingCommas = true,
                    WriteIndented = true,
                };
                var jsonString = JsonSerializer.Serialize(papertext, options);
                return Task.FromResult(jsonString);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
