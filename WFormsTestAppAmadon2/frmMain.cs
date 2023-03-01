using AmadonBlazorLibrary.Data;
using AmadonStandardLib.Classes;
using AmadonStandardLib.Helpers;
using System.Text.Json;
using JsonFormatterPlus;
using AmadonStandardLib.UbClasses;

namespace WFormsTestAppAmadon2
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            EventsControl.SendMessage += EventsControl_SendMessage;
        }

        private void EventsControl_SendMessage(string message)
        {
            txLog.AppendText(message + Environment.NewLine);
            toolStripStatusLabelMessages.Text = message;
            txInitializationMessages.AppendText(message + Environment.NewLine);
            Application.DoEvents();
        }

        private void StaticObjects_ShowMessage(string message, bool isError = false, bool isFatal = false)
        {
            txLog.AppendText(message + Environment.NewLine);
            toolStripStatusLabelMessages.Text = message;
            txInitializationMessages.AppendText(message + Environment.NewLine);
            Application.DoEvents();
        }

        private void btInicializeParamLog_Click(object sender, EventArgs e)
        {
            if (!DataInitializer.InitTranslations())
            {
                StaticObjects_ShowMessage("**** ERROR: InitTranslations");
            }
        }

        private void btTest_Click(object sender, EventArgs e)
        {
            //string branch = "correcoes";
            ////string url = "https://github.com/Rogreis/UbReviewer.git";
            //string repository = "C:\\ProgramData\\UbStudyHelp\\PtAlternative";
            //string username = "rogreis";
            //string password = "Uversa_250";
            //string email = "rogreis@gmail.com";

            ////if (!GitHelper.Instance.Push(repository, username, password, email, branch))
            ////{
            ////    StaticObjects_ShowMessage("**** ERROR: btTest_Click");
            ////}
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (!DataInitializer.InitLogger())
            {
                StaticObjects_ShowMessage("**** ERROR: InitLogger");
            }

            if (!DataInitializer.InitParameters())
            {
                StaticObjects_ShowMessage("**** ERROR: InitParameters");
            }
        }

        void ShowJson(string title, string json)
        {
            txInitializationMessages.Text = title + Environment.NewLine + Environment.NewLine + JsonFormatter.Format(json);
        }

        private void btSearchTest_Click(object sender, EventArgs e)
        {

            if (!DataInitializer.InitLogger())
            {
                StaticObjects_ShowMessage("**** ERROR: InitLogger");
            }

            if (!DataInitializer.InitParameters())
            {
                StaticObjects_ShowMessage("**** ERROR: InitParameters");
            }

            if (!DataInitializer.InitTranslations())
            {
                StaticObjects_ShowMessage("**** ERROR: InitTranslations");
            }

            SearchData data = new SearchData();
            data.Translation = StaticObjects.Book.LeftTranslation;
            data.IndexPathRoot = StaticObjects.Parameters.IndexSearchFolders;
            data.Part1Included = true;
            data.Part2Included = true;
            data.Part3Included = true;
            data.Part4Included = true;
            data.CurrentPaperOnly = false;
            data.CurrentPaper = 1;
            data.QueryString = "terminology";

            string jsonString = SearchBookService.DoSearch(data);
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                WriteIndented = true,
            };
            if (jsonString != null && !string.IsNullOrWhiteSpace(jsonString))
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                SearchData searchDataReturned = JsonSerializer.Deserialize<SearchData>(jsonString, options);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                if (searchDataReturned == null)
                {
                    txInitializationMessages.AppendText("Error");
                }
                else ShowJson($"Results: {searchDataReturned.SearchResults.Count}", jsonString);
            }
        }

        private void btSearchIndex_Click(object sender, EventArgs e)
        {

            if (!DataInitializer.InitTranslations())
            {
                StaticObjects_ShowMessage("**** ERROR: InitTranslations");
            }

            SearchIndexData data = new SearchIndexData();
            data.IndexPathRoot = StaticObjects.Parameters.IndexSearchFolders;
            data.Query = "god AND absolute"; //, "dynamic AND perfect";  // 1 result

            string jsonString = SearchIndexService.DoSearch(data);
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                WriteIndented = true,
            };
            if (jsonString != null && !string.IsNullOrWhiteSpace(jsonString))
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                SearchIndexData searchDataReturned = JsonSerializer.Deserialize<SearchIndexData>(jsonString, options);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                if (searchDataReturned == null)
                {
                    txInitializationMessages.AppendText("Error");
                }
                else ShowJson($"Results: {searchDataReturned.ResultsList.Count}", jsonString);
            }
        }

        private void btTOC_test_Click(object sender, EventArgs e)
        {
            // 
            if (!DataInitializer.InitTranslations())
            {
                StaticObjects_ShowMessage("**** ERROR: InitTranslations");
            }

            string jsonString = TOC_Service.GetToc(StaticObjects.Book.LeftTranslation.LanguageID);
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                WriteIndented = true,
            };
            if (jsonString != null && !string.IsNullOrWhiteSpace(jsonString))
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                TOC_Table toc = JsonSerializer.Deserialize<TOC_Table>(jsonString, options);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                if (toc == null)
                {
                    txInitializationMessages.AppendText("Error");
                }
                else ShowJson($"Results: {toc.Parts.Count} parts", jsonString);
            }
        }

        private void btSettings_Click(object sender, EventArgs e)
        {

        }
    }
}