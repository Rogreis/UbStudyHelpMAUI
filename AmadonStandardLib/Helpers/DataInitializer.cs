using AmadonStandardLib.Classes;
using AmadonStandardLib.UbClasses;
using System;
using System.IO;
using static System.Environment;

namespace AmadonStandardLib.Helpers
{
    public class DataInitializer
    {
        private static string DataFolder()
        {

            string processName = System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            var commonpath = GetFolderPath(SpecialFolder.CommonApplicationData);
            return Path.Combine(commonpath, processName);
        }


        private static string MakeProgramDataFolder(string? fileName = null)
        {
            string folder = DataFolder();
            Directory.CreateDirectory(folder);
            if (fileName != null)
            {
                folder = Path.Combine(folder, fileName);
                Directory.CreateDirectory(folder);
            }
            return folder;
        }

        /// <summary>
        /// Inicialize the list of available translations
        /// </summary>
        /// <param name="dataFiles"></param>
        /// <returns></returns>
        private static bool InicializeTranslations(GetDataFiles dataFiles)
        {
            try
            {
                if (StaticObjects.Book.Translations == null)
                {
                    StaticObjects.Book.Translations = dataFiles.GetTranslations();
                }
                EventsControl.FireSendMessage($"{StaticObjects.Book.Translations.Count} translation found in available translations file");
                return true;
            }
            catch (Exception ex)
            {
                string message = $"Could not initialize available translations. See log.";
                StaticObjects.Logger.Error(message, ex);
                EventsControl.FireFatalError(message);
                return false;
            }
        }

        /// <summary>
        /// Initialize the format table used for editing translations
        /// </summary>
        private static bool GetFormatTable(GetDataFiles dataFiles)
        {
            try
            {
                if (StaticObjects.Book.FormatTableObject == null)
                {
                    StaticObjects.Book.FormatTableObject = new FormatTable(dataFiles.GetFormatTable());
                }
                return true;
            }
            catch (Exception ex)
            {
                StaticObjects.Logger.Error($"Missing format table. May be you do not have the correct data to use this tool.", ex);
                return false;
            }
        }


        private static bool InitTranslation(GetDataFiles dataFiles, short translationId, ref Translation? trans)
        {
            trans = null;
            if (translationId < 0) return true;

            EventsControl.FireSendMessage($"Getting translation id {translationId}");
            trans = dataFiles.GetTranslation(translationId);
            if (trans == null)
            {
                StaticObjects.Logger.Error($"Non existing translation: {translationId}");
                return false;
            }
            EventsControl.FireSendMessage($"Translation file read: {trans.Description}");
            if (trans.IsEditingTranslation)
            {
                EventsControl.FireSendMessage($"Getting editing translation {trans.Description}");
                EventsControl.FireSendMessage("Checking folders for editing translation");
                if (!Directory.Exists(StaticObjects.Parameters.EditParagraphsRepositoryFolder))
                {
                    StaticObjects.Logger.Error("There is no repositoty set for editing translation");
                    return false;
                }
                if (!GitHelper.Instance.IsValid(StaticObjects.Parameters.EditParagraphsRepositoryFolder))
                {
                    StaticObjects.Logger.Error($"Folder is not a valid respository: {StaticObjects.Parameters.EditParagraphsRepositoryFolder}");
                    return false;
                }
                EventsControl.FireSendMessage("Found a valid repository");
                // Format table must exist for editing translation
                if (!GetFormatTable(dataFiles))
                {
                    return false;
                }
            }
            return trans.CheckData();
        }

        private static bool VerifyRepository(string repository, string url, string? branch = null)
        {
            try
            {
                EventsControl.FireSendMessage($"Verifying repository: {repository}");
                if (!GitHelper.Instance.IsValid(repository))
                {
                    EventsControl.FireSendMessage("Cloning...");
                    if (!GitHelper.Instance.Clone(url, repository))
                    {
                        EventsControl.FireSendMessage("Clone failed");
                        EventsControl.FireFatalError("Could not clone translations");
                        return false;
                    }
                }
                else
                {
                    if (branch != null)
                    {
                        EventsControl.FireSendMessage("Checkout...");
                        if (!GitHelper.Instance.Checkout(repository, branch))
                        {
                            EventsControl.FireSendMessage("Checkout failed");
                            EventsControl.FireFatalError("Could not checkout TUB translations");
                            return false;
                        }
                    }

                    EventsControl.FireSendMessage("Pull...");
                    if (!GitHelper.Instance.Pull(repository))
                    {
                        EventsControl.FireSendMessage("Pull failed");
                        EventsControl.FireFatalError("Could not checkout TUB translations");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                string message = $"Could not verify repository {repository}";
                StaticObjects.Logger.Error(message, ex);
                EventsControl.FireFatalError(message);
                return false;
            }
            // Verify respository existence
        }

        public static bool InitLogger()
        {
            try
            {
                // Log for errors
                Logger.PathLog = Path.Combine(MakeProgramDataFolder(), Logger.FileName);
                StaticObjects.Logger = new Logger();
                StaticObjects.Logger.Initialize(Logger.PathLog, false);
                EventsControl.FireSendMessage($"Log started {DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}");
                return true;
            }
            catch (Exception ex)
            {
                string message = "Could not initialize logger";
                StaticObjects.Logger.Error(message, ex);
                EventsControl.FireFatalError(message);
                return false;
            }
        }

        public static bool InitParameters()
        {
            try
            {
                Parameters.PathParameters = Path.Combine(MakeProgramDataFolder(), Parameters.FileName);
                if (!File.Exists(Parameters.PathParameters))
                {
                    EventsControl.FireSendMessage($"Parameters not found, creating a new one.");
                    StaticObjects.Parameters = new Parameters();
                }
                else
                {
                    EventsControl.FireSendMessage($"Parameters found, reading from disk.");
                    StaticObjects.Parameters = Parameters.Deserialize(Parameters.PathParameters);
                }


                // Set folders and URLs used
                // This must be set in the parameters
                StaticObjects.Parameters.ApplicationDataFolder = MakeProgramDataFolder();
                StaticObjects.Parameters.IndexSearchFolders = MakeProgramDataFolder("IndexSearch");
                StaticObjects.Parameters.TubSearchFolders = MakeProgramDataFolder("TubSearch");


                // This application has a differente location for TUB Files
                StaticObjects.Parameters.TUB_Files_RepositoryFolder = MakeProgramDataFolder("TUB_Files");
                StaticObjects.Parameters.EditParagraphsRepositoryFolder = MakeProgramDataFolder("PtAlternative");

                return true;
            }
            catch (Exception ex)
            {
                string message = "Could not initialize parameters";
                StaticObjects.Logger.Error(message, ex);
                EventsControl.FireFatalError(message);
                return false;
            }
        }

        public static bool InitTranslations()
        {
            try
            {
                GetDataFiles dataFiles = new GetDataFiles(StaticObjects.Parameters);
                StaticObjects.Book = new Book();

                // Verify respository existence
                if (!VerifyRepository(StaticObjects.Parameters.TUB_Files_RepositoryFolder, StaticObjects.Parameters.TUB_Files_Url))
                {
                    return false;
                }

                // Verify respository existence
                if (!VerifyRepository(StaticObjects.Parameters.EditParagraphsRepositoryFolder, StaticObjects.Parameters.EditParagraphsUrl))
                {
                    return false;
                }


                EventsControl.FireSendMessage("Getting translations list");
                if (!InicializeTranslations(dataFiles))
                {
                    return false;
                }

                Translation trans = StaticObjects.Book.LeftTranslation;
                if (!InitTranslation(dataFiles, StaticObjects.Parameters.LanguageIDLeftTranslation, ref trans))
                {
                    StaticObjects.Book.LeftTranslation = null;
                    return false;
                }
                StaticObjects.Book.LeftTranslation = trans;

                trans = StaticObjects.Book.MiddleTranslation;
                if (!InitTranslation(dataFiles, StaticObjects.Parameters.LanguageIDMiddleTranslation, ref trans))
                {
                    StaticObjects.Book.MiddleTranslation = null;
                    return false;
                }
                StaticObjects.Book.MiddleTranslation = trans;

                trans = StaticObjects.Book.RightTranslation;
                if (!InitTranslation(dataFiles, StaticObjects.Parameters.LanguageIDRightTranslation, ref trans))
                {
                    StaticObjects.Book.RightTranslation = null;
                    return false;
                }
                StaticObjects.Book.RightTranslation = trans;
                EventsControl.FireSendMessage("Initialization finished succesfully.");
                return true;
            }
            catch (Exception ex)
            {
                string message = "Could not initialize translations 2. See log.";
                StaticObjects.Logger.Error(message, ex);
                EventsControl.FireFatalError(message);
                return false;
            }
        }
    }
}
