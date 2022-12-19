using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UbStandardObjects.Objects;
using UbStandardObjects;
using AmadonBlazor.Classes;
using System.Text.Json;

namespace AmadonBlazor.Data
{
    public class ParagraphsLine
    {
        public string ParLeft { get; set; }
        public string ParMiddle { get; set; }
        public string ParRight { get; set; }
        public string HtmlCompare { get; set; }
    }

    public class TextService
    {
        private static List<Paragraph> GetParagraphs(Translation t, TOC_Entry entry) 
        {
            return t?.Paper(entry.Paper).Paragraphs;
        }

        private static string GetText(List<Paragraph> list, TOC_Entry entry, bool isEdit, bool insertAnchor)
        {
            Paragraph par = list?.Find(p => p.Section == entry.Section && p.ParagraphNo == entry.ParagraphNo);
            return par?.GetHtml(isEdit, insertAnchor);
        }

        private static ParagraphsLine GetParagraphsLine(Paragraph rightParagraph, 
                                                        List<Paragraph> leftParagraphs, 
                                                        List<Paragraph> middleParagraphs,
                                                        List<Paragraph> compareParagraphs)
        {
            ParagraphsLine pLine= new ParagraphsLine();
            pLine.ParLeft = GetText(leftParagraphs, rightParagraph.Entry, false, false);
            pLine.ParMiddle = GetText(middleParagraphs, rightParagraph.Entry, false, false);
            pLine.ParRight = rightParagraph.GetHtml(false, true);
            pLine.HtmlCompare = GetText(compareParagraphs, rightParagraph.Entry, false, false);
            return pLine;
        }

        public static Task<string> GetHtml(string href)
        {
            TOC_Entry entry = TOC_Entry.FromHref(href);

            List<Paragraph> leftParagraphs = null;
            List<Paragraph> rightParagraphs = GetParagraphs(StaticObjects.Book.RightTranslation, entry);
            List<Paragraph> middleParagraphs = null;
            List<Paragraph> compareParagraphs = null;

            switch (StaticObjects.Parameters.TextShowOption)
            {
                case TextShowOption.RightOnly:
                    break;
                case TextShowOption.LeftRight:
                    leftParagraphs = GetParagraphs(StaticObjects.Book.LeftTranslation, entry);
                    break;
                case TextShowOption.LeftRightCompare:
                    leftParagraphs = GetParagraphs(StaticObjects.Book.LeftTranslation, entry);
                    compareParagraphs = null; // TO DO implement compare
                    break;
                case TextShowOption.LeftMiddleRight:
                    leftParagraphs = GetParagraphs(StaticObjects.Book.LeftTranslation, entry);
                    middleParagraphs = GetParagraphs(StaticObjects.Book.MiddleTranslation, entry);
                    break;
                case TextShowOption.LeftMiddleRightCompare:
                    leftParagraphs = GetParagraphs(StaticObjects.Book.LeftTranslation, entry);
                    middleParagraphs = GetParagraphs(StaticObjects.Book.MiddleTranslation, entry);
                    compareParagraphs = null; // TO DO implement compare
                    break;
            }

            List<ParagraphsLine> list = new();
            foreach(Paragraph p in rightParagraphs)
            {
                list.Add(GetParagraphsLine(p, leftParagraphs, middleParagraphs, compareParagraphs));
            }

            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                WriteIndented = true,
            };
            var jsonString = JsonSerializer.Serialize(list, options);
            return Task.FromResult(jsonString);
        }
    }
}
