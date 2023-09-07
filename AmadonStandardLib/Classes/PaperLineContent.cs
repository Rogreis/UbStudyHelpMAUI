using AmadonStandardLib.Helpers;
using AmadonStandardLib.UbClasses;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmadonStandardLib.Classes
{

    public enum ColumnInfo
    {
        Left,
        Middle,
        Right
    }



    public class PaperTitleContent
    {
        public string TranslationName { get; set; } = "";
        public string TranslationDescription { get; set; } = "";
        public override string ToString()
        {
            return $"{TranslationName}<br />{TranslationDescription}";
        }
    }

    public class PaperColumnContent
    {
        public string Id { get; set; } = "";
        public string HtmlId { get; set; } = "";
        public string Anchor { get; set; } = "";
        public string CssClass { get; set; } = "";
        public string Htmltext { get; set; } = "";
        public bool Highlighted { get; set; } = false;

        /// <summary>
        /// This property is set at runtime when we know which column will be used
        /// </summary>
        public ColumnInfo Column { get; set; } = ColumnInfo.Left;

        private string FormatTextTitle(string text)
        {
            return $"<span class=\"text-warning\">{text}</span>";
        }


        private string Letter(ColumnInfo columnInfo)
        {
            switch (columnInfo) 
            {
                case ColumnInfo.Left: return "_L";
                case ColumnInfo.Middle: return "_M";
                default: return "_R";
            }
        }

        public PaperColumnContent()
        {
        }

        public PaperColumnContent(Paragraph p, ColumnInfo columnInfo)
        {
            if (StaticObjects.Parameters.ShowParagraphIdentification) Id= $"<span class=\"text-secondary\">{p.ID} </span>";
            HtmlId = p.AName + Letter(columnInfo);
            if (columnInfo == ColumnInfo.Left) Anchor = $"<a name =\"{p.AName}\"/>";
            Highlighted = p.TranslationId == StaticObjects.Book.GetTocSearchTranslation().LanguageID && p.Entry * StaticObjects.Parameters.Entry;
            if (Highlighted) CssClass = "class=\"highlightedPar\"";
            Htmltext = p.Text;


            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<div class=\"p-2 \">");
            switch (p.Format)
            {
                case ParagraphHtmlType.BookTitle:
                    Htmltext = $"<h1>{Anchor}{FormatTextTitle(p.Text)}</h1>";
                    break;
                case ParagraphHtmlType.PaperTitle:
                    Htmltext = $"<h2>{Anchor}{CssClass}{FormatTextTitle(p.Text)}</h2>";
                    break;
                case ParagraphHtmlType.SectionTitle:
                    Htmltext = $"<h3>{Anchor}{CssClass}{FormatTextTitle(p.Text)}</h3>";
                    break;
                case ParagraphHtmlType.NormalParagraph:
                    Htmltext = $"{Anchor}{CssClass}{Id} {p.Text}";
                    break;
                case ParagraphHtmlType.IdentedParagraph:
                    Htmltext = $"<bloquote>{Anchor}{CssClass}{Id} {p.Text}</bloquote>";
                    break;
            }
        }

        public void Unhighlight()
        {
            Htmltext= HighlightTexts.UnhighlightString(Htmltext);
        }

        public void Highlight(string expression)
        {
            if (!string.IsNullOrEmpty(expression))
            {
                Htmltext = HighlightTexts.UnhighlightString(Htmltext);
            }
            else
            {
                Unhighlight();
            }
        }

    }

    public class PaperLineContent
    {
        public TOC_Entry Entry { get; set; } = new TOC_Entry();

        public PaperColumnContent LeftLine { get; set; } = new PaperColumnContent(); // Left text never can be null
        public PaperColumnContent? MiddleLine { get; set; }
        public PaperColumnContent? RightLine { get; set; }

        public PaperLineContent(Paragraph? leftParagraph, Paragraph? middleParagraph, Paragraph? rightParagraph) 
        {
            if (leftParagraph != null) { LeftLine = new PaperColumnContent(leftParagraph, ColumnInfo.Left); }
            if (middleParagraph != null) { MiddleLine = new PaperColumnContent(middleParagraph, ColumnInfo.Middle); }
            if (rightParagraph != null) { RightLine = new PaperColumnContent(rightParagraph, ColumnInfo.Right); }
        }

        public void Unhighlight()
        {
            LeftLine.Unhighlight();
            MiddleLine?.Unhighlight();
            MiddleLine?.Unhighlight();
            RightLine?.Unhighlight();
        }

        public void Highlight(string expression)
        {
            LeftLine.Highlight(expression);
            MiddleLine?.Highlight(expression);
            MiddleLine?.Highlight(expression);
            RightLine?.Highlight(expression);
        }
    }

    public class PaperBookContent
    {
        public TOC_Entry Entry { get; set; } = new TOC_Entry();

        public int ColumnsNumber { get; set; } = 2;

        public List<PaperTitleContent> Titles { get; set; } = new List<PaperTitleContent>();

        public List<PaperLineContent> Lines { get; set; } = new List<PaperLineContent>();

        public List<string> ParagraphsAnchor
        {
            get
            {
                return Lines
                    .SelectMany(line => new[] { line.LeftLine, line.MiddleLine, line.RightLine }) // Flatten the nested TextContent objects into a single sequence
                    .Where(textContent => textContent != null) // Filter out null TextContent objects
                    .Select(textContent => textContent!.HtmlId) // Extract the HtmlId
                    .ToList(); // Convert to a List
            }
        }


        public void Unhighlight()
        {
            foreach (var lineContent in Lines)
            {
                lineContent.LeftLine?.Unhighlight();
                lineContent.MiddleLine?.Unhighlight();
                lineContent.RightLine?.Unhighlight();
            }
        }

        public void Highlight(string expression)
        {
            foreach (var lineContent in Lines)
            {
                lineContent.LeftLine?.Highlight(expression);
                lineContent.MiddleLine?.Highlight(expression);
                lineContent.RightLine?.Highlight(expression);
            }
        }


        public PaperBookContent(int columnsNumber)
        {
            ColumnsNumber= columnsNumber;
        }

        public void AddTitle(Translation trans)
        {
            Titles.Add(new PaperTitleContent() { TranslationName = trans.PaperTranslation, TranslationDescription = trans.Description });
        }

        /// <summary>
        /// Add the lines to be shown
        /// </summary>
        /// <param name="leftParagraphs"></param>
        /// <param name="middleParagraphs"></param>
        /// <param name="rightParagraphs"></param>
        public void AddParagraphs(List<Paragraph>? leftParagraphs, List<Paragraph>? middleParagraphs, List<Paragraph>? rightParagraphs)
        {
            for (int indexParagraph= 0; indexParagraph< leftParagraphs.Count; indexParagraph++)
            {
                Lines.Add(new PaperLineContent(leftParagraphs.ElementAt(indexParagraph), middleParagraphs.ElementAt(indexParagraph), rightParagraphs.ElementAt(indexParagraph)));
            }
        }


    }
}
