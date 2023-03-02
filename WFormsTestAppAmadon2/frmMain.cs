using AmadonBlazorLibrary.Data;
using AmadonStandardLib.Classes;
using AmadonStandardLib.Helpers;
using System.Text.Json;
using JsonFormatterPlus;
using AmadonStandardLib.UbClasses;
using System.Net.Http.Headers;
using AmadonStandardLib.InterchangeData;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WFormsTestAppAmadon2
{
    public partial class frmMain : Form
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            WriteIndented = true,
        };

        public frmMain()
        {
            InitializeComponent();
            EventsControl.SendMessage += EventsControl_SendMessage;
        }

        private void EventsControl_SendMessage(string message)
        {
            if (message == null)
            {
                toolStripStatusLabelMessages.Text = "";
                Application.DoEvents();
                return;
            }
            toolStripStatusLabelMessages.Text = message;
            txInitializationMessages.AppendText(message + Environment.NewLine);
            Application.DoEvents();
        }

        private void StaticObjects_ShowMessage(string message, bool isError = false, bool isFatal = false)
        {
            EventsControl_SendMessage(message);
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
            txInitializationMessages.AppendText(title + Environment.NewLine + Environment.NewLine + JsonFormatter.Format(json));
        }

        private async void Initialize()
        {
            InitResult initResult = new InitResult();
            string json = await InitializationService.InitAll(initResult);
            InitResult? result = InitResult.Deserialize<InitResult>(json);
            InterchangeDataBase.DumpProperties(result);
            txLog.Text = StaticObjects.Logger.GetLog();
        }

        private void btInicializeParamLog_Click(object sender, EventArgs e)
        {
            Initialize();
        }



        private void btSearchTest_Click(object sender, EventArgs e)
        {
            Initialize();

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
            SearchData? result = InitResult.Deserialize<SearchData>(jsonString);
            InterchangeDataBase.DumpProperties(result);
            txLog.Text = StaticObjects.Logger.GetLog();
        }

        private void btSearchIndex_Click(object sender, EventArgs e)
        {
            Initialize();

            SearchIndexData data = new SearchIndexData();
            data.IndexPathRoot = StaticObjects.Parameters.IndexSearchFolders;
            data.Query = "god AND absolute"; //, "dynamic AND perfect";  // 1 result

            string jsonString = SearchIndexService.DoSearch(data);
            SearchIndexData? result = InitResult.Deserialize<SearchIndexData>(jsonString);
            InterchangeDataBase.DumpProperties(result);
            txLog.Text = StaticObjects.Logger.GetLog();
        }

        private void btTOC_test_Click(object sender, EventArgs e)
        {
            Initialize();

            TOCdata toCdata = new TOCdata();
            toCdata.TranslationId= StaticObjects.Book.LeftTranslation.LanguageID;
            string jsonString = TOC_Service.GetToc(toCdata);
            TOCdata? result = InitResult.Deserialize<TOCdata>(jsonString);
            InterchangeDataBase.DumpProperties(result);
            txLog.Text = StaticObjects.Logger.GetLog();
        }

        private void btSettings_Click(object sender, EventArgs e)
        {

            SettingsData settingsData = new SettingsData();

        }
    }
}