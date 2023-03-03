using AmadonStandardLib.Helpers;
using AmadonStandardLib.UbClasses;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace AmadonStandardLib.Classes
{

    public enum TextShowOption
    {
        RightOnly = 0,
        LeftRight = 1,
        LeftMiddleRight = 2,
        LeftRightCompare = 3,
        LeftMiddleRightCompare = 4
    }



    public class Parameters
    {


        //public ControlsAppearance Appearance = new ControlsAppearance();

        public const string FileName = "AmadonParameters.json";

        public static string? PathParameters { get; set; }


        /// <summary>
        /// Last position in the text, default for first paragraph
        /// </summary>
        public TOC_Entry Entry { get; set; } = new TOC_Entry(0, 0, 1, 0, 0, 0);

        public short LanguageIDLeftTranslation { get; set; } = 0;

        public short LanguageIDRightTranslation { get; set; } = 34;

        public short LanguageIDMiddleTranslation { get; set; } = -1;  // -1 indicate not to be shown

        // Show pages options
        public bool ShowMiddle { get; set; } = true;
        public bool ShowRight { get; set; } = true;
        public bool ShowCompare { get; set; } = false;

        public int SearchPageSize { get; set; } = 20;

        public bool ShowParagraphIdentification { get; set; } = true;


        public TextShowOption TextShowOption { get; set; } = TextShowOption.LeftRight;

        /// <summary>
        /// Max items stored for  search and index text
        /// </summary>
        public int MaxExpressionsStored { get; set; } = 50;

        public List<string> SearchStrings { get; set; } = new List<string>();

        public List<string> IndexLetters { get; set; } = new List<string>();

        public bool SimpleSearchIncludePartI { get; set; } = true;

        public bool SimpleSearchIncludePartII { get; set; } = true;

        public bool SimpleSearchIncludePartIII { get; set; } = true;

        public bool SimpleSearchIncludePartIV { get; set; } = true;

        public bool SimpleSearchCurrentPaperOnly { get; set; } = false;

        public double SpliterDistance { get; set; } = 550;  // BUG: Default value needs to be proportional to user screen resolution

        public List<string> SearchIndexEntries { get; set; } = new List<string>();

        public List<TOC_Entry> TrackEntries { get; set; } = new List<TOC_Entry>();

        public string LastTrackFileSaved { get; set; } = "";


        public string InputHtmlFilesPath { get; set; } = "";

        public string IndexDownloadedFiles { get; set; } = "";

        public string IndexOutputFilesPath { get; set; } = "";

        public string SqlServerConnectionString { get; set; } = "";

        public string FontFamilyInfo { get; set; } = "Verdana";

        public virtual double FontSizeInfo { get; set; } = 10;

        //public virtual ColorSerial HighlightColor { get; set; } = new ColorSerial(0, 0, 102, 255); // rgb(0, 102, 255)

        // Quick search
        public string SearchFor { get; set; } = "";

        public string SimilarSearchFor { get; set; } = "";

        public string CloseSearchDistance { get; set; } = "5";

        public string CloseSearchFirstWord { get; set; } = "";

        public string CloseSearchSecondWord { get; set; } = "";

        public List<string> CloseSearchWords { get; set; } = new List<string>();

        public short CurrentTranslation { get; set; } = 0;

        public double AnnotationWindowWidth { get; set; } = 800;

        public double AnnotationWindowHeight { get; set; } = 450;

        public bool UseDarkThemme { get; set; } = true;


        /// <summary>
        /// Currente executable folder initialize every execution
        /// </summary>
        public string ApplicationFolder { get; set; } = "";

        /// <summary>
        /// Current data folder
        /// </summary>
        public string ApplicationDataFolder { get; set; } = "";

        /// <summary>
        /// Folder to store local lucene index search data
        /// </summary>
        public string IndexSearchFolders { get; set; } = "";

        /// <summary>
        /// Folder to store local lucene TUB search data
        /// </summary>
        public string TubSearchFolders { get; set; } = "";

        /// <summary>
		/// Source of TUB Files used only by UbStudyHelp
		/// </summary>
		public string UbStudyHelpTubFilesSourcePath = "";

        /// <summary>
        /// Git associated repository folder
        /// </summary>
        public string TUB_Files_RepositoryFolder { get; set; } = "";

        /// <summary>
        /// Github source for translations
        /// </summary>
        public string TUB_Files_Url { get; set; } = "https://github.com/Rogreis/TUB_Files.git";

        /// <summary>
        /// Is there any editing translation enabled for this user?
        /// </summary>
        public bool IsEditingEnabled { get; set; } = false;

        /// <summary>
        /// Work local repository for paragraphs
        /// </summary>
        public string? EditParagraphsRepositoryFolder { get; set; } = null;

        /// <summary>
        /// Github source for editing translation
        /// </summary>
        public string EditParagraphsUrl { get; set; } = "https://github.com/Rogreis/PtAlternative.git";

        /// <summary>
        /// Full book pages local repository
        /// </summary>
        public string? EditBookRepositoryFolder { get; set; } = null;

        /// <summary>
        /// Github paragraphs repository
        /// </summary>
        public string? UrlRepository { get; set; } = null;

        public float FontSize { get; set; } = 14;

        public string FontFamily { get; set; } = "Verdana,Arial,Helvetica";

        public bool IsDarkTheme { get; set; } = true;

        public string DarkText { get; set; } = "black";

        public string LightText { get; set; } = "white";

        public string DarkTextHighlihted { get; set; } = "yellow";

        public string LightTextHighlihted { get; set; } = "blue";

        public string DarkTextGray { get; set; } = "yellow";

        public string LightTextGray { get; set; } = "bisque";




        /// <summary>
        /// Serialize the parameters instance
        /// </summary>
        /// <param name="p"></param>
        /// <param name="pathParameters"></param>
        public static void Serialize(Parameters p, string pathParameters)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    AllowTrailingCommas = true,
                    WriteIndented = true,
                };
                var jsonString = JsonSerializer.Serialize<Parameters>(p, options);
                File.WriteAllText(pathParameters, jsonString);
            }
            catch { }
        }

        /// <summary>
        /// Deserialize the parameters instance
        /// </summary>
        /// <param name="pathParameters"></param>
        /// <returns></returns>
        public static Parameters Deserialize(string pathParameters)
        {
            try
            {
                StaticObjects.Logger.Info("»»»» Deserialize Parameters");
                var jsonString = File.ReadAllText(pathParameters);
                return StaticObjects.DeserializeObject<Parameters>(jsonString);
            }
            catch
            {
                StaticObjects.Logger.Info("»»»» Deserialize Parameters creating default");
                return new Parameters();
            }
        }


    }
}
