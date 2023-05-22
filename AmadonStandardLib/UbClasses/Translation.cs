﻿using AmadonStandardLib.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace AmadonStandardLib.UbClasses
{
    public class Translation
    {
        public const short NoTranslation = -1;
        private readonly string TocTableFileName = "TOC_Table.json";
        private List<ItemForToc>? _tableOfContents = null;

        public short LanguageID { get; set; }
        public string Description { get; set; } = "";
        public int Version { get; set; }
        public string TIN { get; set; } = "";
        public string TUB { get; set; } = "";
        public string TextButton { get; set; } = "";
        public int CultureID { get; set; }
        public bool UseBold { get; set; }
        public bool RightToLeft { get; set; }
        public int StartingYear { get; set; }
        public int EndingYear { get; set; }
        public string PaperTranslation { get; set; } = "";
        public bool IsEditingTranslation { get; set; } = false;
        public string Hash { get; set; } = "";
        public string RepositoryName { get; set; } = "";

        public List<Paper> Papers { get; set; } = new List<Paper>();

        // Colocar isto no Json, diferente para cada tradução
        public List<string> PartTitles = new List<string>()
        {
            "Introdução",
            "Part I",
            "Part II",
            "Part III",
            "Part IV",
        };

        // First papers list for each part
        [JsonIgnore]
        public short[] FistPapers = new short[]
        {
            0,
            1,
            32,
            57,
            120,
            197
        };

        private List<TOC_Entry> Sections(Paper p)
        {
            return (from par in p.Paragraphs
                    where par.ParagraphNo == 0 && par.Section > 0
                    orderby par.Section ascending
                    select par.Entry).ToList();
        }

        private void GetSections(Paper paper, List<ItemForToc> children)
        {
            foreach (TOC_Entry entrySection in Sections(paper))
            {
                ItemForToc itemSection = new ItemForToc();
                children.Add(itemSection);
                itemSection.Text = entrySection.Text;
                itemSection.Entry = entrySection;
            }
        }

        private void GetPapers(int partNumber, List<ItemForToc> children)
        {
            List<TOC_Entry> papers = (from paper in Papers
                                      where paper.PaperNo >= FistPapers[partNumber] && paper.PaperNo < FistPapers[partNumber + 1]
                                      orderby paper.PaperNo ascending
                                      select paper.Entry).ToList();

            foreach (TOC_Entry entryPaper in papers)
            {
                ItemForToc itemPaper = new ItemForToc();
                children.Add(itemPaper);
                itemPaper.Text = $"{entryPaper.Paper}  {entryPaper.Text}";
                itemPaper.Entry = entryPaper;
                Paper paper = Papers[entryPaper.Paper];
                GetSections(paper, itemPaper.WorkChildren);
            }
        }


        private void GetIntroToc(List<ItemForToc> children)
        {
            Paper paperIntro = Papers[0];
            foreach (TOC_Entry entrySection in Sections(paperIntro))
            {
                ItemForToc itemSection = new ItemForToc();
                children.Add(itemSection);
                itemSection.Text = entrySection.Text;
                itemSection.Entry = entrySection;
            }
        }

        public bool CreateTableOfContents()
        {
            if (_tableOfContents != null)
            {
                return true;
            }

            if (Papers == null || Papers.Count == 0)
            {
                LibraryEventsControl.FireFatalError($"Error creating table of contents for translation {ToString()}: Papers not got from file.");
                return false;
            }

            try
            {
                List<ItemForToc> items = new List<ItemForToc>();
                for (int index = 0; index < 5; index++)
                {
                    ItemForToc itemPart = new ItemForToc();
                    items.Add(itemPart);
                    itemPart.Text = PartTitles[index];
                    itemPart.Entry = new TOC_Entry((short)(index + 1000), 0, 0, 0, 0, 0);
                    if (FistPapers[index] == 0)
                    {
                        GetIntroToc(itemPart.WorkChildren);
                    }
                    else
                    {
                        GetPapers(index, itemPart.WorkChildren);
                    }
                }
                _tableOfContents = items;
                return true;
            }
            catch (Exception ex)
            {
                LibraryEventsControl.FireFatalError($"Error creating table of contents for translation {ToString()}: ", ex);
                return false;
            }
        }


        [JsonIgnore]
        public List<ItemForToc>? TableOfContents
        {
            get
            {
                if (!CreateTableOfContents()) return null;
                return _tableOfContents;
            }
        }


        [JsonIgnore]
        public string Copyright
        {
            get
            {
                string year = (StartingYear == EndingYear) ? EndingYear.ToString() : StartingYear.ToString() + "," + EndingYear.ToString();
                return "Copyright ©  " + year + " Urantia Foundation. All rights reserved.";
            }
        }

        [JsonIgnore]
        public string Identification
        {
            get
            {
                return LanguageID.ToString() + " - " + Description;
            }
        }

        public Translation()
        {
        }

        public void GetData(string jsonString)
        {
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };

            var root = JsonSerializer.Deserialize<JsonRootobject>(jsonString, options);

            if (root.Papers != null)
            {
                Papers = new List<Paper>();
                foreach (JsonPaper jsonPaper in root.Papers)
                {
                    this.Papers.Add(new Paper()
                    {
                        Paragraphs = new List<Paragraph>(jsonPaper.Paragraphs)
                    });
                    // Fix the translation number not set in json file for each paragraph
                    foreach (Paragraph p in jsonPaper.Paragraphs)
                    {
                        p.Entry.TranslationId = LanguageID;
                    }
                }
            }

        }


 
        public virtual Paper Paper(short paperNo)
        {
            return Papers.Find(p => p.PaperNo == paperNo);
        }


        /// <summary>
        /// Retuirn the format for an specific paragraph
        /// Get data from English translation
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        public int GetFormat(Paragraph par)
        {
            Paper paper = Paper(par.Paper);
            Paragraph parFound = paper.Paragraphs.Find(p => p.Section == par.Section && p.ParagraphNo == par.ParagraphNo);
            return parFound.FormatInt;
        }


        /// <summary>
        /// Get all papers data for a non editing translation
        /// </summary>
        /// <param name="jsonString"></param>
        public void GetPapersData(string jsonString)
        {
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                WriteIndented = true,
            };
            var root = JsonSerializer.Deserialize<JsonRootobject>(jsonString, options);

            if (root.Papers != null)
            {
                Papers = new List<Paper>();
                foreach (JsonPaper jsonPaper in root.Papers)
                {
                    this.Papers.Add(new Paper()
                    {
                        Paragraphs = new List<Paragraph>(jsonPaper.Paragraphs)
                    });
                    // Fix the translation number not set in json file for each paragraph
                    foreach (Paragraph p in jsonPaper.Paragraphs)
                    {
                        p.Entry.TranslationId = LanguageID;
                    }
                }
            }

        }

        public override string ToString()
        {
            return LanguageID.ToString() + " - " + Description;
        }
    }

    #region Classes to import json file
    internal class JsonRootobject
    {
        public short LanguageID { get; set; }
        public string Description { get; set; }
        public string TIN { get; set; }
        public string TUB { get; set; }
        public int Version { get; set; }
        public string TextButton { get; set; }
        public int CultureID { get; set; }
        public bool UseBold { get; set; }
        public bool RightToLeft { get; set; }
        public short StartingYear { get; set; }
        public short EndingYear { get; set; }
        public string PaperTranslation { get; set; }
        public JsonPaper[] Papers { get; set; }
    }

    internal class JsonPaper
    {
        public Paragraph[] Paragraphs { get; set; }
    }

    internal class TranslationsRoot
    {
        public Translation[] AvailableTranslations { get; set; }
    }


    /// <summary>
    /// Classe used to deserialize json string
    /// </summary>
    public class Translations
    {
        public static List<Translation> DeserializeJson(string jsonString)
        {
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true
            };

            var translationsRoot = JsonSerializer.Deserialize<TranslationsRoot>(jsonString, options);
            List<Translation> list = new List<Translation>();
            list.AddRange(translationsRoot.AvailableTranslations);
            return list;
        }
    }

    /// <summary>
    /// Used only to convert from json
    /// </summary>
    internal class FullPaper
    {
        public Paragraph[] Paragraphs { get; set; }
    }

    #endregion



}
