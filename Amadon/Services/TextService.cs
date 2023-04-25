﻿using AmadonStandardLib.Classes;
using AmadonStandardLib.Helpers;
using AmadonStandardLib.UbClasses;
using System.Text;
using System.Text.Json;

namespace Amadon.Services
{

    public class TextService
    {

        private static string ColumnSize = "50%";

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
                sb.AppendLine($"<td width=\"{ColumnSize}\">");
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
        private static void GetParagraphsLine(StringBuilder sb, Paragraph leftParagraph, 
                                              List<Paragraph> rightParagraphs, 
                                              List<Paragraph> middleParagraphs,
                                              List<Paragraph> compareParagraphs)
        {
            // Only first column has anchor
            Columntext(sb, leftParagraph, false, true);
            GetText(sb, middleParagraphs, leftParagraph.Entry, false, false);
            GetText(sb, rightParagraphs, leftParagraph.Entry, false, false);
            GetText(sb, compareParagraphs, leftParagraph.Entry, false, false);
        }

        /// <summary>
        /// Format a paper title
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        private static string FormatTitle(Translation trans, TOC_Entry entry)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class=\"mt-4 p-3 bg-primary text-white rounded\"> ");
            sb.AppendLine($"  <h2>{trans.PaperTranslation} {entry.Paper}</h2>  ");
            sb.AppendLine($"  <p>{trans.Description}</p>  ");
            sb.AppendLine("</div> ");
            /*
                pt-0 padding-top
                ps-2 padding-left (2 = approximately equal to 10 pixels, assuming the default font-size of 16px).
                pe-0 padding-right
                pb-5 for padding-bottom: 20 (.25rem * 5 in Bootstrap 5 is approximately equal to              * */
            return $"<th width=\"{ColumnSize}\"><div class=\"m-3 parClosed\">{sb}</div></th>";
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

            // Get the paragraphs texts folowing what was required by user (StaticObjects.Parameters.TextShowOption)
            List<Paragraph>? leftParagraphs = null;
            List<Paragraph>? rightParagraphs = null;
            List<Paragraph>? middleParagraphs = null;
            List<Paragraph>? compareParagraphs = null;

            // Left is always shown
            leftParagraphs = GetParagraphs(StaticObjects.Book.LeftTranslation, paperTextFormatted.Entry);
            StaticObjects.Parameters.ShowMiddle = false;

            switch (StaticObjects.Parameters.TextShowOption)
            {
                case TextShowOption.LeftOnly:
                    paperTextFormatted.Titles.Add(FormatTitle(StaticObjects.Book.RightTranslation, paperTextFormatted.Entry));
                    ColumnSize = "100%";
                    break;
                case TextShowOption.LeftRight:
                    paperTextFormatted.Titles.Add(FormatTitle(StaticObjects.Book.LeftTranslation, paperTextFormatted.Entry));
                    paperTextFormatted.Titles.Add(FormatTitle(StaticObjects.Book.RightTranslation, paperTextFormatted.Entry));
                    rightParagraphs = GetParagraphs(StaticObjects.Book.RightTranslation, paperTextFormatted.Entry);
                    ColumnSize = "50%";
                    break;
                case TextShowOption.LeftRightCompare:
                    paperTextFormatted.Titles.Add(FormatTitle(StaticObjects.Book.LeftTranslation, paperTextFormatted.Entry));
                    paperTextFormatted.Titles.Add(FormatTitle(StaticObjects.Book.RightTranslation, paperTextFormatted.Entry));
                    paperTextFormatted.Titles.Add("Compare");
                    rightParagraphs = GetParagraphs(StaticObjects.Book.RightTranslation, paperTextFormatted.Entry);
                    leftParagraphs = GetParagraphs(StaticObjects.Book.LeftTranslation, paperTextFormatted.Entry);
                    compareParagraphs = null; // TO DO implement compare
                    ColumnSize = "33%";
                    break;
                case TextShowOption.LeftMiddleRight:
                    paperTextFormatted.Titles.Add(FormatTitle(StaticObjects.Book.LeftTranslation, paperTextFormatted.Entry));
                    paperTextFormatted.Titles.Add(FormatTitle(StaticObjects.Book.MiddleTranslation, paperTextFormatted.Entry));
                    paperTextFormatted.Titles.Add(FormatTitle(StaticObjects.Book.RightTranslation, paperTextFormatted.Entry));
                    rightParagraphs = GetParagraphs(StaticObjects.Book.RightTranslation, paperTextFormatted.Entry);
                    middleParagraphs = GetParagraphs(StaticObjects.Book.MiddleTranslation, paperTextFormatted.Entry);
                    ColumnSize = "33%";
                    break;
                case TextShowOption.LeftMiddleRightCompare:
                    paperTextFormatted.Titles.Add(FormatTitle(StaticObjects.Book.LeftTranslation, paperTextFormatted.Entry));
                    paperTextFormatted.Titles.Add(FormatTitle(StaticObjects.Book.MiddleTranslation, paperTextFormatted.Entry));
                    paperTextFormatted.Titles.Add(FormatTitle(StaticObjects.Book.RightTranslation, paperTextFormatted.Entry));
                    paperTextFormatted.Titles.Add("Compare");
                    rightParagraphs = GetParagraphs(StaticObjects.Book.RightTranslation, paperTextFormatted.Entry);
                    middleParagraphs = GetParagraphs(StaticObjects.Book.MiddleTranslation, paperTextFormatted.Entry);
                    compareParagraphs = null; // TO DO implement compare
                    ColumnSize = "25%";
                    break;
            }

            // Format line
            foreach(Paragraph p in leftParagraphs)
            {
                StringBuilder sb = new StringBuilder();
                GetParagraphsLine(sb, p, rightParagraphs, middleParagraphs, compareParagraphs);
                paperTextFormatted.Lines.Add(sb.ToString());
            }

            return Task.FromResult(paperTextFormatted);
        }
    }
}
