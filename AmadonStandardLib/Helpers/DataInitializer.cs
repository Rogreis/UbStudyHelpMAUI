using AmadonStandardLib.Classes;
using AmadonStandardLib.UbClasses;
using LibGit2Sharp;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using static Lucene.Net.Queries.Function.ValueSources.MultiFunction;
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
        /// Initialize the format table used for editing translations
        /// </summary>
        private static bool GetFormatTable(GetDataFiles dataFiles)
        {
            try
            {
                if (StaticObjects.Book.FormatTableObject == null)
                {
                    string? json = dataFiles.GetFormatTable();
                    if (json != null) 
                    {
                        StaticObjects.Book.FormatTableObject = new FormatTable(json);
                    }
                    else
                    {
                        StaticObjects.Logger.Error($"Missing format table. May be you do not have the correct data to use this tool.");
                        return false;
                    }
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
            LibraryEventsControl.FireSendUserAndLogMessage($"Initializing translation {translationId}");
            trans = null;
            if (translationId < 0) return true;

            LibraryEventsControl.FireSendUserAndLogMessage($"Getting translation id {translationId}");
            trans = dataFiles.GetTranslation(translationId);
            if (trans == null)
            {
                StaticObjects.Logger.Error($"Non existing translation: {translationId}");
                return false;
            }
            //LibraryEventsControl.FireSendUserAndLogMessage($"Translation file read: {trans.Description}");
            //if (trans.IsEditingTranslation)
            //{
            //    LibraryEventsControl.FireSendUserAndLogMessage($"Getting editing translation {trans.Description}");
            //    LibraryEventsControl.FireSendUserAndLogMessage("Checking folders for editing translation");
            //    if (!Directory.Exists(StaticObjects.Parameters.EditParagraphsRepositoryFolder))
            //    {
            //        StaticObjects.Logger.Error("There is no repository set for editing translation");
            //        return false;
            //    }
            //    if (!GitHelper.Instance.IsValid(StaticObjects.Parameters.EditParagraphsRepositoryFolder))
            //    {
            //        StaticObjects.Logger.Error($"Folder is not a valid respository: {StaticObjects.Parameters.EditParagraphsRepositoryFolder}");
            //        return false;
            //    }
            //    LibraryEventsControl.FireSendUserAndLogMessage("Found a valid repository");
            //    // Format table must exist for editing translation
            //    if (!GetFormatTable(dataFiles))
            //    {
            //        return false;
            //    }
            //}
            return trans.CheckData();
        }

        /// <summary>
        /// Downlaod a file from github
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private async Task DownloadFileFromGitHubAsync(string url, string filePath)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                byte[] fileContents = await response.Content.ReadAsByteArrayAsync();

                await File.WriteAllBytesAsync(filePath, fileContents);
            }
        }


        ///// <summary>
        /// Initialize the log object
        /// </summary>
        /// <returns></returns>
        public static bool InitLogger()
        {
            try
            {
                // Log for errors
                Logger.PathLog = Path.Combine(MakeProgramDataFolder(), Logger.FileName);
                StaticObjects.Logger = new Logger();
                StaticObjects.Logger.Initialize(Logger.PathLog, false);
                LibraryEventsControl.FireSendUserAndLogMessage($"Log started {DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}");
                return true;
            }
            catch (Exception ex)
            {
                string message = "Could not initialize logger";
                StaticObjects.Logger.Error(message, ex);
                LibraryEventsControl.FireSendUserAndLogMessage(message);
                return false;
            }
        }

        /// <summary>
        /// Initialize the parameters object
        /// </summary>
        /// <returns></returns>
        public static bool InitParameters()
        {
            try
            {
                Parameters.PathParameters = Path.Combine(MakeProgramDataFolder(), Parameters.FileName);
                if (!File.Exists(Parameters.PathParameters))
                {
                    LibraryEventsControl.FireSendUserAndLogMessage($"Parameters not found, creating a new one.");
                    StaticObjects.Parameters = new Parameters();
                }
                else
                {
                    LibraryEventsControl.FireSendUserAndLogMessage($"Parameters found, reading from disk.");
                    StaticObjects.Parameters = Parameters.Deserialize(Parameters.PathParameters);
                }


                // Set folders and URLs used
                // This must be set in the parameters
                StaticObjects.Parameters.ApplicationDataFolder = MakeProgramDataFolder();
                StaticObjects.Parameters.IndexSearchFolders = MakeProgramDataFolder("IndexSearch");
                StaticObjects.Parameters.TubSearchFolders = MakeProgramDataFolder("TubSearch");


                // This application has a differente location for TUB Files
                StaticObjects.Parameters.TUB_Files_RepositoryFolder = MakeProgramDataFolder("TUB_Files");
                LibraryEventsControl.FireSendUserAndLogMessage($"Parameters started {DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}");
                Parameters.Serialize(StaticObjects.Parameters, Parameters.PathParameters);

                return true;
            }
            catch (Exception ex)
            {
                string message = "Could not initialize parameters";
                LibraryEventsControl.FireSendUserAndLogMessage(message, ex);
                return false;
            }
        }

        /// <summary>
        /// Initialize the repositories object
        /// </summary>
        /// <param name="initEditRepository"></param>
        /// <returns></returns>
        public static bool VerifyDataFolder(bool recreate = false)
        {
            StaticObjects.Book = new Book();

            if (recreate)
            {
                try
                {
                    Directory.Delete(StaticObjects.Parameters.TUB_Files_RepositoryFolder, true);
                }
                catch (Exception ex)
                {
                    LibraryEventsControl.FireSendUserAndLogMessage($"Failure removing data folder not verified: {StaticObjects.Parameters.TUB_Files_RepositoryFolder}", ex);
                    return false;
                }
            }

            try
            {
                LibraryEventsControl.FireSendUserAndLogMessage($"Verifying data folder: {StaticObjects.Parameters.TUB_Files_RepositoryFolder}");
                if (!Directory.Exists(StaticObjects.Parameters.TUB_Files_RepositoryFolder))
                {
                    StaticObjects.Logger.Error("There is no repository set for editing translation");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                string message = $"Could not verify data folder {StaticObjects.Parameters.TUB_Files_RepositoryFolder}";
                StaticObjects.Logger.Error(message, ex);
                LibraryEventsControl.FireSendUserAndLogMessage(message);
                return false;
            }
        }


        /// <summary>
        /// Inicialize the list of available translations
        /// </summary>
        /// <param name="dataFiles"></param>
        /// <returns></returns>
        public static bool InitTranslationsList()
        {
            try
            {
                GetDataFiles dataFiles = new GetDataFiles(StaticObjects.Parameters);
                LibraryEventsControl.FireSendUserAndLogMessage("Initializing translations");
                if (StaticObjects.Book.Translations == null)
                {
                    StaticObjects.Book.Translations = dataFiles.GetTranslations();
                }
                LibraryEventsControl.FireSendUserAndLogMessage($"{StaticObjects.Book.Translations.Count} translation found in available translations file");
                return true;
            }
            catch (Exception ex)
            {
                string message = "Could not initialize translations.";
                LibraryEventsControl.FireSendUserAndLogMessage(message);
                return false;
            }
        }


        public static Translation? InitTranslation(Translation? previousTranslation, short newId, Diagnostic diagnostic)
        {
            try
            {
                if (newId < 0)
                {
                    diagnostic.IsError = true;
                    diagnostic.Message = "Translation id must be greater than 0";
                    return previousTranslation;
                }

                GetDataFiles dataFiles = new GetDataFiles(StaticObjects.Parameters);
                LibraryEventsControl.FireSendUserAndLogMessage($"Initializing translation: {newId}");

                Translation? trans = null;
                if (!InitTranslation(dataFiles, newId, ref trans))
                {
                    diagnostic.IsError = true;
                    diagnostic.Message = "Translation was not initialized";
                    return previousTranslation;
                }
                diagnostic.IsError= false; 
                diagnostic.Message = "";
                LibraryEventsControl.FireSendUserAndLogMessage("Translation initialized succesfully.");
                return trans;
            }
            catch (Exception ex)
            {
                diagnostic.IsError = true;
                diagnostic.Message = $"Could not initialize translation: {newId}";
                LibraryEventsControl.FireSendUserAndLogMessage(diagnostic.Message);
                return previousTranslation;
            }
        }
    }
}
