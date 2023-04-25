using AmadonStandardLib.Classes;
using AmadonStandardLib.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace AmadonStandardLib.UbClasses
{
    public class Translation
    {
        public const short NoTranslation = -1;
        private string? LocalRepositoryFolder = null;
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

        /// <summary>
        /// List of available anootations for this translation
        /// </summary>
        //[JsonIgnore]
        //public List<UbAnnotationsStoreData> Annotations { get; set; } = new List<UbAnnotationsStoreData>();


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
            };
            var root = JsonSerializer.Deserialize<JsonRootobject>(jsonString, options);

            //this.LanguageID = root.LanguageID;
            //this.Description = root.Description;
            //this.TIN = root.TIN;
            //this.TUB = root.TUB;
            //this.Version = root.Version;
            //this.TextButton = root.TextButton;
            //this.CultureID = root.CultureID;
            //this.UseBold = root.UseBold;
            //this.RightToLeft = root.RightToLeft;
            //this.StartingYear = root.StartingYear;
            //this.EndingYear = root.EndingYear;
            //this.PaperTranslation = root.PaperTranslation;

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
            if (this.IsEditingTranslation)
                return GetPaperEdit(paperNo);
            return Papers.Find(p => p.PaperNo == paperNo);
        }

        ///// <summary>
        ///// Get an annotation object for this entry
        ///// </summary>
        ///// <param name="entry"></param>
        ///// <returns></returns>
        //public UbAnnotationsStoreData GetAnnotation(TOC_Entry entry)
        //{
        //    return Annotations.FirstOrDefault(a => a.Entry == entry);
        //}

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


        ///// <summary>
        ///// Store a created or modified annotation
        ///// </summary>
        ///// <param name="annotation"></param>
        //public void StoreAnnotation(UbAnnotationsStoreData annotation)
        //{
        //    if (Annotations.Exists(a => a.Entry == annotation.Entry))
        //    {
        //        Annotations.Remove(Annotations.Find(a => a.Entry == annotation.Entry));
        //    }
        //    Annotations.Add(annotation);
        //}


        public Translation(Translation trans, string localRepositoryFolder)
        {
            LocalRepositoryFolder = localRepositoryFolder;
            this.LanguageID = trans.LanguageID;
            this.Description = trans.Description;
            this.TIN = trans.TIN;
            this.TUB = trans.TUB;
            this.Version = trans.Version;
            this.TextButton = trans.TextButton;
            this.CultureID = trans.CultureID;
            this.UseBold = trans.UseBold;
            this.RightToLeft = trans.RightToLeft;
            this.StartingYear = trans.StartingYear;
            this.EndingYear = trans.EndingYear;
            this.PaperTranslation = trans.PaperTranslation;
        }

        private Paper GetPaperEdit(short paperNo)
        {
            FormatTable table = StaticObjects.Book.GetFormatTable();
            Paper paper = new Paper();
            paper.Paragraphs = new List<Paragraph>();
            Translation EnglishTranslation = StaticObjects.Book.GetTranslation(0);
            string folderPaper = Path.Combine(LocalRepositoryFolder, $"Doc{paperNo:000}");
            foreach (string filePath in Directory.GetFiles(folderPaper, "*.md"))
            {
                Paragraph paragraph = new Paragraph(filePath);
                paragraph.FormatInt = EnglishTranslation.GetFormat(paragraph);
                paper.Paragraphs.Add(paragraph);
            }
            return paper;
        }


        public Paragraph GetParagraph(short paperNo, short section, short paragraphNo)
        {
            Paragraph par = new Paragraph(LocalRepositoryFolder);
            return par;
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



        #region Index
        private TUB_TOC_Entry? JsonIndexEntry(string fileNamePath, bool isPaperTitle, short paperNoParam = -1)
        {
            char[] separators = { '_' };
            string fileName = Path.GetFileNameWithoutExtension(fileNamePath);

            string[] parts = fileName.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            short paperNo = Convert.ToInt16(parts[1]);
            short sectionNo = Convert.ToInt16(parts[2]);
            short paragraphNo = Convert.ToInt16(parts[3]);
            if (!isPaperTitle && sectionNo == 0 && paragraphNo == 0)
            {
                return null;
            }
            string text = File.ReadAllText(fileNamePath, Encoding.UTF8);
            return new TUB_TOC_Entry()
            {
                Text = paperNoParam > 0 ? paperNoParam.ToString() + " - " + text : text,
                PaperNo = paperNo,
                SectionNo = sectionNo,
                ParagraphNo = paperNo,
                Expanded = sectionNo == 0
            };
        }

        private TUB_TOC_Entry JsonIndexEntry(string text)
        {
            return new TUB_TOC_Entry()
            {
                Text = text,
                Expanded = true
            };
        }

        /// <summary>
        /// Create an object with all the index entry for the editing translation
        /// </summary>
        /// <returns></returns>
        public List<TUB_TOC_Entry> GetTranslationIndex(bool forceGeneration = false)
        {
            string indexJsonFilePath = Path.Combine(StaticObjects.Parameters.TubDataFolder, TocTableFileName);
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                WriteIndented = true,
                IncludeFields = true
            };

            indexJsonFilePath = Path.Combine(StaticObjects.Parameters.TubDataFolder, TocTableFileName);

            if (File.Exists(indexJsonFilePath) && !forceGeneration)
            {
                string json = File.ReadAllText(indexJsonFilePath);
                //var data= JsonSerializer.Deserialize(json, options);
                //return null;
                return JsonSerializer.Deserialize<List<TUB_TOC_Entry>>(json, options);
            }

            List<TUB_TOC_Entry> list = new List<TUB_TOC_Entry>();
            string pathIntroduction = Path.Combine(LocalRepositoryFolder, "Doc000\\Par_000_000_000.md");
            TUB_TOC_Entry intro = JsonIndexEntry(pathIntroduction, true, 0);
            TUB_TOC_Entry part1 = JsonIndexEntry("Parte I");
            TUB_TOC_Entry part2 = JsonIndexEntry("Parte II");
            TUB_TOC_Entry part3 = JsonIndexEntry("Parte III");
            TUB_TOC_Entry part4 = JsonIndexEntry("Parte IV");

            list.Add(intro);
            list.Add(part1);
            list.Add(part2);
            list.Add(part3);
            list.Add(part4);

            for (short paperNo = 0; paperNo < 197; paperNo++)
            {
                string folderPath = Path.Combine(LocalRepositoryFolder, $"Doc{paperNo:000}");
                TUB_TOC_Entry? jsonIndexPaper = null;
                string pathTitle = Path.Combine(folderPath, $"Par_{paperNo:000}_000_000.md");
                if (paperNo == 0)
                {
                    jsonIndexPaper = intro;
                }
                else
                {
                    jsonIndexPaper = JsonIndexEntry(pathTitle, true, paperNo);
                }
                if (paperNo == 0)
                {
                    // do nothing
                }
                else if (paperNo < 32)
                {
                    part1.Nodes.Add(jsonIndexPaper);
                }
                else if (paperNo < 57)
                {
                    part2.Nodes.Add(jsonIndexPaper);
                }
                else if (paperNo < 120)
                {
                    part3.Nodes.Add(jsonIndexPaper);
                }
                else
                {
                    part4.Nodes.Add(jsonIndexPaper);
                }

                foreach (string mdFile in Directory.GetFiles(folderPath, $"Par_{paperNo:000}_???_000*.md"))
                {
                    TUB_TOC_Entry paperSection = JsonIndexEntry(mdFile, false);
                    if (paperSection != null)
                        jsonIndexPaper.Nodes.Add(paperSection);
                };
            }

            //// Serialize the index
            //string jsonString = JsonSerializer.Serialize<List<TUB_TOC_Entry>>(list, options);
            //File.WriteAllText(indexJsonFilePath, jsonString);
            ////File.WriteAllText(Path.Combine(StaticObjects.Parameters.TUB_Files_RepositoryFolder, TocTableFileName), jsonString);
            ////File.WriteAllText(Path.Combine(StaticObjects.Parameters.EditBookRepositoryFolder, TocTableFileName), jsonString);

            return list;
        }

        #endregion



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
