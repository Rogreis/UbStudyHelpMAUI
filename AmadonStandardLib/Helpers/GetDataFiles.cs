using AmadonStandardLib.Classes;
using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AmadonStandardLib.Helpers
{
    public class GetDataFiles
    {
        // Hard coded function returns
        public const string FileNotFound = "»»FNF";
        public const string ErrorGettingFile = "»»ERR";


        #region Helper functions
        /// <summary>
        /// Copy streams
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        protected static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        /// <summary>
        /// Unzip a Gzipped translation file
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        protected static string BytesToString(byte[] bytes, bool isZip = true)
        {
            if (!isZip)
            {
                return Encoding.UTF8.GetString(bytes);
            }
            else
            {
                using (var msi = new MemoryStream(bytes))
                using (var mso = new MemoryStream())
                {
                    using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                    {
                        //gs.CopyTo(mso);
                        CopyTo(gs, mso);
                    }

                    return Encoding.UTF8.GetString(mso.ToArray());
                }
            }
        }

        #endregion

        #region Get Files

        /// <summary>
        /// Returns the app data folder compatible with all targets: Android, iOS, macOS, and Windows
        /// <see href="https://docs.microsoft.com/en-us/xamarin/essentials/"/>
        /// Once we have the correct platform-specific directory paths, 
        /// we can use the System.IO.Path methods to manipulate and work with paths, such as Path.Combine, Path.GetFileName, Path.GetDirectoryName, and others.
        /// </summary>
        /// <returns></returns>
        public static string GetDataFolder()
        {
            return FileSystem.AppDataDirectory;
        }

        public static async Task<bool> DownloadBinaryFile(string url, string pathDestination)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    byte[] fileContents = await response.Content.ReadAsByteArrayAsync();

                    await File.WriteAllBytesAsync(pathDestination, fileContents);
                    return true;
                }
            }
            catch (Exception ex)
            {
                LibraryEventsControl.FireSendUserAndLogMessage($"Could not download translation, error: {ex.Message}", ex);
                return false;
            }
        }

        /// <summary>
        /// Checks if a file exists on local repository
        /// </summary>
        /// <param name="pathLocalFile"></param>
        /// <returns></returns>
        public static bool LocalFileExists(string pathLocalFile)
        { 
            return File.Exists(pathLocalFile); 
        }

        /// <summary>
        /// Returns a string from a local zipped file
        /// </summary>
        /// <param name="pathFile"></param>
        /// <returns></returns>
        public static Task<string> GetStringFromZippedFile(string pathFile)
        {
            try
            {
                if (!File.Exists(pathFile))
                {
                    StaticObjects.Logger.Warn($"File not found on local repository: {pathFile}");
                    return Task.FromResult<string>(FileNotFound);
                }
                byte[] bytes = File.ReadAllBytes(pathFile);
                return Task.FromResult<string>(BytesToString(bytes));
            }
            catch (Exception ex)
            {
                StaticObjects.Logger.Error("Getting zipped file string from local repository", ex);
                return Task.FromResult<string>(ErrorGettingFile);
            }
        }

        public static Task<string> GetStringFromLocalFile(string pathLocalFile)
        {
            try
            {
                if (!File.Exists(pathLocalFile))
                {
                    StaticObjects.Logger.Warn($"File not found on local repository: {pathLocalFile}");
                    return Task.FromResult<string>(FileNotFound);
                }
                string ret= File.ReadAllText(pathLocalFile);
                return Task.FromResult<string>(ret);
            }
            catch (Exception ex)
            {
                StaticObjects.Logger.Error("Getting text file string from local repository", ex);
                return Task.FromResult<string>(ErrorGettingFile);
            }
        }

        #endregion

        ///// <summary>
        ///// Get all papers from the zipped file
        ///// </summary>
        ///// <param name="translationId"></param>
        ///// <param name="isZip"></param>
        ///// <returns></returns>
        //protected static string? GetFile(short translationId, bool isZip = true)
        //{
        //    try
        //    {
        //        string json = "";

        //        string translationJsonFilePath = TranslationJsonFilePath(translationId);
        //        if (File.Exists(translationJsonFilePath))
        //        {
        //            json = File.ReadAllText(translationJsonFilePath);
        //            return json;
        //        }

        //        string translationStartupPath = TranslationFilePath(translationId);
        //        if (File.Exists(translationStartupPath))
        //        {
        //            StaticObjects.Logger.Info("File exists: " + translationStartupPath);
        //            byte[] bytes = File.ReadAllBytes(translationStartupPath);
        //            json = BytesToString(bytes, isZip);
        //            File.WriteAllText(translationJsonFilePath, json);
        //            return json;
        //        }
        //        else
        //        {
        //            StaticObjects.Logger.Error($"Translation not found {translationId}");
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        StaticObjects.Logger.Error("GetFile", ex);
        //        return null;
        //    }
        //}



        ///// <summary>
        ///// Get a translation from a local file
        ///// </summary>
        ///// <param name="translationId"></param>
        ///// <returns></returns>
        //public static Translation? GetTranslation(short translationId, bool initializePapers = true)
        //{
        //    Translation? translation = StaticObjects.Book.GetTranslation(translationId);
        //    if (translation == null)
        //    {
        //        translation = new Translation();
        //    }

        //    if (!initializePapers) return translation;

        //    if (translation.Papers.Count > 0)
        //    {
        //        if (!translation.CheckData())
        //        {
        //            return null;
        //        }

        //        return translation;
        //    }

        //    // FIX ME: IsEditingTranslation is hard codded here, but needs to come from repository
        //    if (!translation.IsEditingTranslation)
        //    {
        //        string? json = GetFile(translationId, true);
        //        translation.GetPapersData(json);

        //        // Loading annotations
        //        //translation.Annotations = null;
        //    }
        //    return translation;
        //}

    }
}
