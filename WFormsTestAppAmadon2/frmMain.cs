using AmadonBlazorLibrary.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Forms;
using AmadonBlazorLibrary.Classes;

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
            toolStripStatusLabelMessages.Text= message;
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
            //string branch = "changes_for_edition";
            //string url = "https://github.com/Rogreis/UbReviewer.git";
            //string repository = "C:\\Trabalho\\Lixo\\git\\UbReviewer";


            //GitHelper.Instance.Test(repository, branch, url);

            //if (!GitHelper.Instance.Checkout(StaticObjects.Parameters.EditParagraphsRepositoryFolder, branch))
            //{
            //    StaticObjects_ShowMessage("**** ERROR: btTest_Click");
            //}
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
    }
}