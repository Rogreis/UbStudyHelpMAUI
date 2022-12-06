using static System.Environment;
using UbStandardObjects;
using Amadon.Classes;

namespace Amadon;

public partial class App : Application
{

    private string DataFolder()
    {

        string processName = System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        var commonpath = GetFolderPath(SpecialFolder.CommonApplicationData);
        return Path.Combine(commonpath, processName);
    }


    private string MakeProgramDataFolder(string fileName)
    {
        string folder = DataFolder();
        Directory.CreateDirectory(folder);
        return Path.Combine(folder, fileName);
    }

    private bool LocalInitialization()
    {
        // Log for errors
        string pathLog = MakeProgramDataFolder("UbStudyHelp.log");

        StaticObjects.Logger = new LogMAUI();
        StaticObjects.Logger.Initialize(pathLog, false);
        StaticObjects.Logger.Info("»»»» Startup");

        StaticObjects.PathParameters = MakeProgramDataFolder("UbStudyHelp.json");
        if (!File.Exists(StaticObjects.PathParameters))
        {
            StaticObjects.Logger.Info("Parameters not found, creating a new one: " + StaticObjects.PathParameters);
        }
        StaticObjects.Parameters = ParametersMAUI.Deserialize(StaticObjects.PathParameters);



        // Set folders and URLs used
        // This must be set in the parameters
        StaticObjects.Parameters.ApplicationFolder = System.AppDomain.CurrentDomain.BaseDirectory;
        StaticObjects.Parameters.TUB_Files_RepositoryFolder = MakeProgramDataFolder("TUB_Files");
        // FIX ME
        StaticObjects.Parameters.TUB_Files_RepositoryFolder = @"C:\ProgramData\UbStudyHelp\TUB_Files";
        StaticObjects.Parameters.ApplicationFolder = @"C:\ProgramData\UbStudyHelp";
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
        MainPage = new AppShell();
	}
}
