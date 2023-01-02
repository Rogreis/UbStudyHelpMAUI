using AmadonBlazor.Classes;
using Microsoft.Maui.Controls;
using UbStandardObjects;
using UbStandardObjects.Helpers;
using static System.Environment;

namespace AmadonBlazor
{
    public partial class App : Application
    {
        private const string AppName = "AmandonApp";

        private string DataFolder()
        {

            //string processName = System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            var commonpath = GetFolderPath(SpecialFolder.CommonApplicationData);
            return Path.Combine(commonpath, AppName);
        }


        private string MakeProgramDataFolder(string fileName)
        {
            string folder = DataFolder();
            Directory.CreateDirectory(folder);
            return Path.Combine(folder, fileName);
        }


        private bool LocalInitialization()
        {

            var dataFolder = Path.Combine(GetFolderPath(SpecialFolder.CommonApplicationData), AppName);
            Directory.CreateDirectory(dataFolder);

            // Log for errors
            StaticObjects.PathLog = Path.Combine(dataFolder, $"{AppName}.log");

            StaticObjects.Logger = new LogMAUI();
            StaticObjects.Logger.Initialize(StaticObjects.PathLog, false);
            StaticObjects.Logger.Info("»»»» Startup");

            StaticObjects.PathParameters = Path.Combine(dataFolder, $"{AppName}.json");
            if (!File.Exists(StaticObjects.PathParameters))
            {
                StaticObjects.Logger.Info("Parameters not found, creating a new one: " + StaticObjects.PathParameters);
            }
            StaticObjects.Parameters = ParametersMAUI.Deserialize(StaticObjects.PathParameters);
            StaticObjects.Parameters.ApplicationFolder = System.AppDomain.CurrentDomain.BaseDirectory;


            // Get Github data
            GitHelper gitHelper = GitHelper.Instance;

            // TUB files (existing translations) folder
            StaticObjects.Parameters.TUB_Files_RepositoryFolder = Path.Combine(dataFolder, "TUB_Files");
            Directory.CreateDirectory(StaticObjects.Parameters.TUB_Files_RepositoryFolder);
            StaticObjects.Logger.Info($"TUB_Files_RepositoryFolder: {StaticObjects.Parameters.TUB_Files_RepositoryFolder}");
            gitHelper.VerifyRepository("https://github.com/Rogreis/TUB_Files.git", StaticObjects.Parameters.TUB_Files_RepositoryFolder);

            // Reposiroty folder for edit language when exists
            StaticObjects.Parameters.EditParagraphsRepositoryFolder = Path.Combine(dataFolder, "Repo");
            StaticObjects.Logger.Info($"EditParagraphsRepositoryFolder: {StaticObjects.Parameters.EditParagraphsRepositoryFolder}");





            StaticObjects.Book = new BookMAUI();

            GetDataFilesMAUI dataFiles = new((ParametersMAUI)StaticObjects.Parameters);

            if (!StaticObjects.Book.Inicialize(dataFiles, StaticObjects.Parameters.LanguageIDLeftTranslation, StaticObjects.Parameters.LanguageIDRightTranslation))
            {
                EventsControl.FireFatalError("Data not loaded!");
                return false;
            }



            return true;

        }


        public App()
        {
            InitializeComponent();

            Application.Current.UserAppTheme = AppTheme.Dark;
            LocalInitialization();

            MainPage = new MainPage();
        }
    }
}