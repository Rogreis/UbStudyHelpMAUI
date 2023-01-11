using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using AmadonBlazorLibrary.Helpers;

namespace AmadonBlazorLibrary.UbClasses
{
    public class Translation
    {
        public const short NoTranslation = -1;
        private string LocalRepositoryFolder = null;
        private string TocTableFileName = "TOC_Table.json";

        public short LanguageID { get; set; }
        public string Description { get; set; }
        public int Version { get; set; }
        public string TIN { get; set; }
        public string TUB { get; set; }
        public string TextButton { get; set; }
        public int CultureID { get; set; }
        public bool UseBold { get; set; }
        public bool RightToLeft { get; set; }
        public int StartingYear { get; set; }
        public int EndingYear { get; set; }
        public string PaperTranslation { get; set; }
        public bool IsEditingTranslation { get; set; } = false;

        public List<Paper> Papers { get; set; } = new List<Paper>();

        /// <summary>
        /// List of available anootations for this translation
        /// </summary>
        //[JsonIgnore]
        //public List<UbAnnotationsStoreData> Annotations { get; set; } = new List<UbAnnotationsStoreData>();


        [JsonIgnore]
        public List<TOC_Entry> TableOfContents
        {
            get
            {
                List<TOC_Entry> toc = new List<TOC_Entry>();
                foreach (Paper paper in Papers)
                {
                    var paragraphEntries = from p in paper.Paragraphs
                                           where p.ParagraphNo == 0
                                           orderby p.PK_Seq ascending
                                           select p.Entry;
                    toc.AddRange(paragraphEntries);
                }

                return toc;
            }
        }


        private TOC_Entry GetFirstPartParagraph(short paperNo, string text)
        {
            Paragraph p = (from paper in Papers
                           from par in paper.Paragraphs
                           where par.Paper == paperNo && par.ParagraphNo == 0
                           select par).First();
            return new TOC_Entry(p, text);
        }

        private void GetPartPapersSections(TOC_Entry entry, short startPaperNo, short endPaperNo)
        {
            entry.Papers = (from paper in Papers
                            where paper.PaperNo >= startPaperNo && paper.PaperNo <= endPaperNo
                            orderby paper.PaperNo ascending
                            select paper.Entry).ToList();
            foreach (TOC_Entry entryPaper in entry.Papers)
            {
                entryPaper.Sections = (from paper in Papers
                                       from p in paper.Paragraphs
                                       where p.Paper == entryPaper.Paper && p.ParagraphNo == 0 && p.Section > 0
                                       orderby p.Section ascending
                                       select p.Entry).ToList();
            }
        }


        [JsonIgnore]
        public TOC_Table TOC
        {
            get
            {
                TOC_Table toc = new TOC_Table();

                toc.Title = "Lista ded Documentos parea " + Description;

                TOC_Entry intro = GetFirstPartParagraph(0, "Introdução");
                GetPartPapersSections(intro, 0, 0);
                TOC_Entry partI = GetFirstPartParagraph(1, "Parte I");
                GetPartPapersSections(partI, 1, 31);
                TOC_Entry partII = GetFirstPartParagraph(32, "Parte II");
                GetPartPapersSections(partII, 32, 56);
                TOC_Entry partIII = GetFirstPartParagraph(56, "Parte III");
                GetPartPapersSections(partIII, 57, 119);
                TOC_Entry partIV = GetFirstPartParagraph(120, "Parte IV");
                GetPartPapersSections(partIV, 119, 196);

                toc.Parts = new List<TOC_Entry>()
                {
                    intro,
                    partI,
                    partII,
                    partIII,
                    partIV
                };

                return toc;
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


        /// <summary>
        /// Verify is all mandatory data is ok
        /// </summary>
        /// <returns></returns>
        public bool CheckData()
        {
            try
            {
                StaticObjects.Logger.InInterval(LanguageID, 0, 196, $"Invalid translation number: {LanguageID}");
                StaticObjects.Logger.IsNull(PaperTranslation, $"Paper name is missing for translation {LanguageID}");
                return true;
            }
            catch (Exception ex)
            {
                StaticObjects.Logger.FatalErrorAsync($"Fatal error in translation data {ex.Message}");
                return false;
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


        #region Index
        private TUB_TOC_Entry JsonIndexEntry(string fileNamePath, bool isPaperTitle, short paperNoParam = -1)
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
            string indexJsonFilePath = Path.Combine(StaticObjects.Parameters.EditBookRepositoryFolder, TocTableFileName);
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                WriteIndented = true,
                IncludeFields = true
            };

            indexJsonFilePath = Path.Combine(StaticObjects.Parameters.EditBookRepositoryFolder, TocTableFileName);

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
                TUB_TOC_Entry jsonIndexPaper = null;
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
