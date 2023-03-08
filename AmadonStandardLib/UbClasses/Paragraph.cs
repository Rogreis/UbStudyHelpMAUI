﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AmadonStandardLib.UbClasses
{
    /// <summary>
    /// Represents the translation status of each paragraph being worked.
    /// </summary>
    public enum ParagraphStatus
    {
        Started = 0,
        Working = 1,
        Doubt = 2,
        Ok = 3,
        Closed = 4
    }

    /// <summary>
    /// Represents the html format for a paragraph
    /// </summary>
    public enum ParagraphHtmlType
    {
        BookTitle = 0,
        PaperTitle = 1,
        SectionTitle = 2,
        NormalParagraph = 3,
        IdentedParagraph = 4
    }

    public class Paragraph
    {

        public string TranslatorNote { get; set; } = "";

        public string Comment { get; set; } = "";

        public DateTime LastDate { get; set; }


        [JsonPropertyName("TranslationID")]
        public short TranslationId { get; set; } = 0;
        public short Paper { get; set; }
        public short PK_Seq { get; set; }
        public short Section { get; set; }
        public short ParagraphNo { get; set; }
        public short Page { get; set; }
        public short Line { get; set; }
        public virtual string Text { get; set; } = "";
        public int FormatInt { get; set; }

        [JsonIgnore]
        private TOC_Entry? entry = null;

        /// <summary>
        /// Status is not to be exported to json files for UbStudyHelp
        /// </summary>
        [JsonIgnore]
        [JsonPropertyName("Status")]
        public int _status { get; set; }

        [JsonIgnore]
        public ParagraphStatus Status
        {
            get
            {
                return (ParagraphStatus)_status;
            }
            set
            {
                _status = (int)value;
            }
        }


        public ParagraphHtmlType Format
        {
            get
            {
                return (ParagraphHtmlType)FormatInt;
            }
        }

        [JsonIgnore]
        public TOC_Entry Entry
        {
            get
            {
                if (entry == null)
                {
                    entry = new TOC_Entry(TranslationId, Paper, Section, ParagraphNo, Page, Line);
                    entry.Text = Text;
                }
                return entry;
            }
        }


        [JsonIgnore]
        public string Identification
        {
            get
            {
                return string.Format("{0}:{1}-{2} ({3}.{4})", Paper, Section, ParagraphNo, Page, Line); ;
            }
        }

        public string ID
        {
            get
            {
                return string.Format($"{Paper}:{Section}-{ParagraphNo}");
            }
        }


        [JsonIgnore]
        public string AName
        {
            get
            {
                return string.Format("U{0}_{1}_{2}", Paper, Section, ParagraphNo); ;
            }
        }

        [JsonIgnore]
        public bool IsPaperTitle
        {
            get
            {
                return Section == 0 && ParagraphNo == 0;
            }
        }

        [JsonIgnore]
        public bool IsSectionTitle
        {
            get
            {
                return ParagraphNo == 0;
            }
        }

        [JsonIgnore]
        public string ParaIdent
        {
            get
            {
                return Paper.ToString("000") + PK_Seq.ToString("000");
            }
        }


        [JsonIgnore]
        public bool IsEditTranslation { get; set; } = false;

        [JsonIgnore]
        public bool IsDivider
        {
            get
            {
                return Text.StartsWith("* * *") || Text.StartsWith("~ ~ ~");
            }
        }


        public Paragraph()
        {
        }


        public Paragraph(short translationId)
        {
            TranslationId = translationId;
        }

        public Paragraph(string filePath)
        {
            Text = MarkdownToHtml(File.ReadAllText(filePath));
            char[] sep = { '_' };
            string fileName = Path.GetFileNameWithoutExtension(filePath).Remove(0, 4);
            string[] parts = fileName.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            Paper = Convert.ToInt16(parts[0]);
            Section = Convert.ToInt16(parts[1]);
            ParagraphNo = Convert.ToInt16(parts[2]);
            IsEditTranslation = true;
        }

        public Paragraph(string repositoryPath, string ident)
        {
            char[] sep = { '_' };
            string[] parts = ident.Remove(0, 1).Split(sep, StringSplitOptions.RemoveEmptyEntries);
            Paper = Convert.ToInt16(parts[0]);
            Section = Convert.ToInt16(parts[1]);
            ParagraphNo = Convert.ToInt16(parts[2]);
            IsEditTranslation = true;
            string filePath = FullPath(repositoryPath, this);
            Text = MarkdownToHtml(File.ReadAllText(filePath));
        }


        public Paragraph(string repositoryPath, short paperNo, short sectionNo, short paragraphNo)
        {
            Paper = paperNo;
            Section = sectionNo;
            ParagraphNo = paragraphNo;
            string filePath = FullPath(repositoryPath, this);
            if (!File.Exists(filePath))
            {
                throw new Exception($"Paragraph does not exist: {paperNo} {sectionNo} {paragraphNo}");
            }
            IsEditTranslation = true;
            Text = MarkdownToHtml(File.ReadAllText(filePath));
        }


        public static string RelativeFilePath(Paragraph p)
        {
            return $"Doc{p.Paper:000}/Par_{p.Paper:000}_{p.Section:000}_{p.ParagraphNo:000}.md";
        }

        public static string Url(Paragraph p)
        {
            return $"https://github.com/Rogreis/PtAlternative/blob/correcoes/Doc{p.Paper:000}/Par_{p.Paper:000}_{p.Section:000}_{p.ParagraphNo:000}.md";
        }


        public static string RelativeFilePathWindows(Paragraph p)
        {
            return $@"Doc{p.Paper:000}\Par_{p.Paper:000}_{p.Section:000}_{p.ParagraphNo:000}.md";
        }

        public static string FullPath(string repositoryPath, short paperNo, short sectionNo, short paragraphNo)
        {
            return Path.Combine(repositoryPath, $@"Doc{paperNo:000}\Par_{paperNo:000}_{sectionNo:000}_{paragraphNo:000}.md");
            //return Path.Combine(repositoryPath, $@"Par_{paperNo:000}_{sectionNo:000}_{paragraphNo:000}.md");
        }

        public static string FullPath(string repositoryPath, Paragraph p)
        {
            return Path.Combine(repositoryPath, RelativeFilePathWindows(p));
        }


        private string GetCssClass(bool isEdit)
        {
            string cssClass = "commonText";

            if (isEdit)
            {
                switch (Status)
                {
                    case ParagraphStatus.Started:
                        cssClass = "parStarted";
                        break;
                    case ParagraphStatus.Working:
                        cssClass = "parWorking";
                        break;
                    case ParagraphStatus.Ok:
                        cssClass = "parOk";
                        break;
                    case ParagraphStatus.Doubt:
                        cssClass = "parDoubt";
                        break;
                    case ParagraphStatus.Closed:
                        cssClass = "parClosed";
                        break;
                }
            }
            return cssClass;
        }

        private void FormatText(StringBuilder sb, bool isEdit, bool insertAnchor, string startTag, string endTag)
        {
            sb.Append($"{startTag}{(insertAnchor ? $"<a name =\"{AName}\"/>" : "")} {ID} {Text}{endTag}");
        }

        private void FormatTitle(StringBuilder sb, bool isEdit, bool insertAnchor, string startTag, string endTag)
        {
            sb.Append($"{startTag}{(insertAnchor ? $"<a name =\"{AName}\"/>" : "")} {Text}{endTag}");
        }


        // Add this if using nested MemberwiseClone.
        // This is a class, which is a reference type, so cloning is more difficult.
        public Paragraph DeepCopy()
        {
            // Clone the root ...
            Paragraph other = (Paragraph)this.MemberwiseClone();
            return other;
        }


        public string GetHtml(bool isEdit, bool insertAnchor)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<div class=\"p-3 mb-2 {GetCssClass(isEdit)}\">");
            switch (Format)
            {
                case ParagraphHtmlType.BookTitle:
                    FormatText(sb, isEdit, false, "<h1>", "</h1>");
                    break;
                case ParagraphHtmlType.PaperTitle:
                    FormatTitle(sb, isEdit, insertAnchor, "<h2>", "</h2>");
                    break;
                case ParagraphHtmlType.SectionTitle:
                    FormatTitle(sb, isEdit, insertAnchor, "<h3>", "</h3>");
                    break;
                case ParagraphHtmlType.NormalParagraph:
                    FormatText(sb, isEdit, insertAnchor, "<p>", "</p>");
                    break;
                case ParagraphHtmlType.IdentedParagraph:
                    FormatText(sb, isEdit, insertAnchor, "<bloquote><p>", "</p></bloquote>");
                    break;
            }
            sb.AppendLine("</div>");
            return sb.ToString();
        }

        /// <summary>
        /// Convert paragraph markdown to HTML
        /// The markdown used for TUB paragraphs has just italics
        /// </summary>
        /// <param name="markDownText"></param>
        /// <returns></returns>
        private string MarkdownToHtml(string markDownText)
        {
            int position = 0;
            bool openItalics = true;

            string newText = markDownText;
            var regex = new Regex(Regex.Escape("*"));
            while (position >= 0)
            {
                position = newText.IndexOf('*');
                if (position >= 0)
                {
                    newText = regex.Replace(newText, openItalics ? "<i>" : "</i>", 1);
                    openItalics = !openItalics;
                }
            }
            return newText;
        }

        public static string FolderPath(short paperNo)
        {
            return $"Doc{paperNo:000}";
        }


        public static string FilePath(short paperNo, short section, short paragraphNo)
        {
            return $"{FolderPath(paperNo)}\\Par_{paperNo:000}_{section:000}_{paragraphNo:000}.md";
        }

        public static string FilePath(string ident)
        {
            // 120:0-1 (0.0)
            char[] separators = { ':', '-', ' ' };
            string[] parts = ident.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            short paperNo = Convert.ToInt16(parts[0]);
            short section = Convert.ToInt16(parts[1]);
            short paragraphNo = Convert.ToInt16(parts[2]);
            return $"Doc{paperNo:000}\\Par_{paperNo:000}_{section:000}_{paragraphNo:000}.md";
        }

        //public void SetNote(Note note)
        //{

        //    if (note != null)
        //    {
        //        TranslatorNote = note.TranslatorNote;
        //        Comment = note.Notes;
        //        LastDate = note.LastDate;
        //        _status = note.Status;
        //    }
        //    else
        //    {
        //        TranslatorNote = "";
        //        Comment = "";
        //        LastDate = DateTime.MinValue;
        //        _status = 0;
        //    }
        //}


        public bool SaveText(string repositoryPath)
        {
            try
            {
                File.WriteAllText(FullPath(repositoryPath, this), Text);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public bool SaveNotes(string repositoryPath)
        //{
        //    try
        //    {
        //        Notes.SaveNotes(this);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}


        public override string ToString()
        {
            string partText = Text;
            if (string.IsNullOrEmpty(Text))
            {
                partText = "";
            }
            return $"{Identification} {partText}";
        }


    }
}
