using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using AmadonStandardLib.Helpers;
using System.IO;

namespace AmadonStandardLib.UbClasses
{
    public class Paper
    {

        private string RepositoryFolder { get; set; } = "";
        private short paperEditNo = -1;
        private FormatTable? Format = null;




        [JsonIgnore]
        public short PaperNo
        {
            get
            {
                if (Paragraphs.Count > 0)
                {
                    return Paragraphs[0].Paper;
                }
                else
                {
                    return -1;
                }
            }
        }

        [JsonIgnore]
        public string Title
        {
            get
            {
                if (Paragraphs.Count > 0)
                {
                    return Paragraphs[0].Text;
                }
                else
                {
                    return "";
                }
            }
        }


        public List<Paragraph> Paragraphs { get; set; } = new List<Paragraph>();

        public TOC_Entry Entry
        {
            get
            {
                return new TOC_Entry(Paragraphs[0]);
            }
        }

        /// <summary>
        /// Constructor parameterless
        /// </summary>
        public Paper()
        {

        }

        /// <summary>
        /// Constructor from a json string 
        /// </summary>
        /// <param name="jsonString"></param>
        public Paper(string jsonString)
        {
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true
            };

            var fullPaper = JsonSerializer.Deserialize<FullPaper>(jsonString, options);
            Paragraphs.AddRange(fullPaper.Paragraphs);
            fullPaper = null;
        }

        public Paper(short paperNo, string repositoryFolder)
        {
            paperEditNo = paperNo;
            RepositoryFolder = repositoryFolder;
            if (Format == null)
            {
                Format = StaticObjects.Book.GetFormatTable();
            }
            else
            {
                throw new Exception("Error in PaperEdit Constructor: Format table not available - could not generate edit paper.");
            }
            GetParagraphsFromRepository();
        }

        /// <summary>
        /// Return an specific paragraph
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public Paragraph? GetParagraph(TOC_Entry entry)
        {
            Paragraph? par = null;
            if (Paragraphs.Count == 0)
            {
                par= Paragraphs.Find(p => p.Section == entry.Section && p.ParagraphNo == entry.ParagraphNo);
                if (par != null) return par;
                GetParagraphsFromRepository();
            }
            // Always get the paragraph from repository
            string filePath = Paragraph.FullPath(RepositoryFolder, entry.Paper, entry.Section, entry.ParagraphNo);
            if (!File.Exists(filePath)) return null;
            par = new Paragraph(filePath);
            GetNotesData(par);
            return par;
        }

 
        /// <summary>
        /// Read all paragraph from disk
        /// </summary>
        private void GetParagraphsFromRepository()
        {
            foreach (string pathParagraphFile in Directory.GetFiles(RepositoryFolder, $@"Doc{paperEditNo:000}\Par_{paperEditNo:000}_*.md"))
            {
                Paragraph p = new Paragraph(pathParagraphFile);
                Format.GetParagraphFormatData(p);
                //Note note = Notes.GetNote(p);
                //p._status = note.Status;
                Paragraphs.Add(p);
            }
            // Sort

            Paragraphs.Sort(delegate (Paragraph p1, Paragraph p2)
            {
                if (p1.Section < p2.Section)
                {
                    return -1;
                }
                if (p1.Section > p2.Section)
                {
                    return 1;
                }
                if (p1.ParagraphNo < p2.ParagraphNo)
                {
                    return -1;
                }
                if (p1.ParagraphNo > p2.ParagraphNo)
                {
                    return 1;
                }
                return 0;
            });

        }

        /// <summary>
        /// Get all availble notes data
        /// </summary>
        /// <param name="paragraph"></param>
        public void GetNotesData(Paragraph paragraph)
        {
            //Note note = Notes.GetNote(paragraph);
            //paragraph.SetNote(note);
        }


        public void GetNotes()
        {

        }

        public override string ToString()
        {
            return $"Paper {PaperNo}";
        }
    }




}
