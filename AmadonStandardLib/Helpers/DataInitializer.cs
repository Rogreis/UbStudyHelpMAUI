﻿using AmadonStandardLib.Classes;
using AmadonStandardLib.UbClasses;
using System;
using System.IO;
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
            EventsControl.FireSendUserAndLogMessage($"Initializing translation {translationId}");
            trans = null;
            if (translationId < 0) return true;

            EventsControl.FireSendUserAndLogMessage($"Getting translation id {translationId}");
            trans = dataFiles.GetTranslation(translationId);
            if (trans == null)
            {
                StaticObjects.Logger.Error($"Non existing translation: {translationId}");
                return false;
            }
            EventsControl.FireSendUserAndLogMessage($"Translation file read: {trans.Description}");
            if (trans.IsEditingTranslation)
            {
                EventsControl.FireSendUserAndLogMessage($"Getting editing translation {trans.Description}");
                EventsControl.FireSendUserAndLogMessage("Checking folders for editing translation");
                if (!Directory.Exists(StaticObjects.Parameters.EditParagraphsRepositoryFolder))
                {
                    StaticObjects.Logger.Error("There is no repository set for editing translation");
                    return false;
                }
                if (!GitHelper.Instance.IsValid(StaticObjects.Parameters.EditParagraphsRepositoryFolder))
                {
                    StaticObjects.Logger.Error($"Folder is not a valid respository: {StaticObjects.Parameters.EditParagraphsRepositoryFolder}");
                    return false;
                }
                EventsControl.FireSendUserAndLogMessage("Found a valid repository");
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
                EventsControl.FireSendUserAndLogMessage($"Verifying repository: {repository}");
                if (!GitHelper.Instance.IsValid(repository))
                {
                    EventsControl.FireSendUserAndLogMessage("Getting translations from server... (it can take longer)");
                    if (!GitHelper.Instance.Clone(url, repository))
                    {
                        EventsControl.FireSendUserAndLogMessage("Getting translations failed");
                        return false;
                    }
                }
                else
                {
                    if (branch != null)
                    {
                        EventsControl.FireSendUserAndLogMessage("Updating translations text... (it can take longer)");
                        if (!GitHelper.Instance.Checkout(repository, branch))
                        {
                            EventsControl.FireSendUserAndLogMessage("Updating translations text failed");
                            return false;
                        }
                    }

                    EventsControl.FireSendUserAndLogMessage("Updating translations text... (it can take longer)");
                    if (!GitHelper.Instance.Pull(repository))
                    {
                        EventsControl.FireSendUserAndLogMessage("Updating translations text failed");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                string message = $"Could not verify repository {repository}";
                StaticObjects.Logger.Error(message, ex);
                EventsControl.FireSendUserAndLogMessage(message);
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
                EventsControl.FireSendUserAndLogMessage($"Log started {DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}");
                return true;
            }
            catch (Exception ex)
            {
                string message = "Could not initialize logger";
                StaticObjects.Logger.Error(message, ex);
                EventsControl.FireSendUserAndLogMessage(message);
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
                    EventsControl.FireSendUserAndLogMessage($"Parameters not found, creating a new one.");
                    StaticObjects.Parameters = new Parameters();
                }
                else
                {
                    EventsControl.FireSendUserAndLogMessage($"Parameters found, reading from disk.");
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
                EventsControl.FireSendUserAndLogMessage($"Parameters started {DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}");

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

        public static bool Repositories(bool initEditRepository = false)
        {
            GetDataFiles dataFiles = new GetDataFiles(StaticObjects.Parameters);
            StaticObjects.Book = new Book();
            // Verify respository existence
            if (!VerifyRepository(StaticObjects.Parameters.TUB_Files_RepositoryFolder, StaticObjects.Parameters.TUB_Files_Url))
            {
                EventsControl.FireSendUserAndLogMessage($"Repository not verified: {StaticObjects.Parameters.TUB_Files_RepositoryFolder}");
                return false;
            }


            // Edit repositpry is only checked when editing translation is used
            EventsControl.FireSendUserAndLogMessage($"Is editing enabled: {StaticObjects.Parameters.IsEditingEnabled}");
            if (StaticObjects.Parameters.IsEditingEnabled)
            {
                if (!VerifyRepository(StaticObjects.Parameters.EditParagraphsRepositoryFolder, StaticObjects.Parameters.EditParagraphsUrl))
                {
                    EventsControl.FireSendUserAndLogMessage($"Repository not verified: {StaticObjects.Parameters.EditParagraphsRepositoryFolder}");
                    return false;
                }
            }
            return true;
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
                EventsControl.FireSendUserAndLogMessage("Initializing translations");
                if (StaticObjects.Book.Translations == null)
                {
                    StaticObjects.Book.Translations = dataFiles.GetTranslations();
                }
                EventsControl.FireSendUserAndLogMessage($"{StaticObjects.Book.Translations.Count} translation found in available translations file");
                return true;
            }
            catch (Exception ex)
            {
                string message = "Could not initialize translations.";
                EventsControl.FireSendUserAndLogMessage(message);
                return false;
            }
        }


        public static bool InitTranslation(string transToInit, short id)
        {
            try
            {
                if (id < 0)
                {
                    return true;
                }

                //Translation trans = TranslationToShow == "Left" ? StaticObjects.Book.LeftTranslation : (TranslationToShow == "Right" ? StaticObjects.Book.RightTranslation : StaticObjects.Book.MiddleTranslation);


                GetDataFiles dataFiles = new GetDataFiles(StaticObjects.Parameters);
                EventsControl.FireSendUserAndLogMessage($"Initializing translation: {transToInit} - {id}");

                Translation trans = null;
                if (!InitTranslation(dataFiles, StaticObjects.Parameters.LanguageIDLeftTranslation, ref trans))
                {
                    StaticObjects.Book.LeftTranslation = null;
                    return false;
                }
                switch(transToInit)
                {
                    case "Left":
                        StaticObjects.Book.LeftTranslation = trans;
                        break;
                    case "Right":
                        StaticObjects.Book.RightTranslation = trans;
                        break;
                    case "Middle":
                        StaticObjects.Book.MiddleTranslation = trans;
                        break;
                }

                EventsControl.FireSendUserAndLogMessage("Translation initialized succesfully.");
                return true;
            }
            catch (Exception ex)
            {
                string message = $"Could not initialize translation: {transToInit} - {id}";
                EventsControl.FireSendUserAndLogMessage(message);
                return false;
            }
        }
    }
}
