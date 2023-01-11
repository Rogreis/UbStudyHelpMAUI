using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadonBlazorLibrary.Helpers
{
    public class Database
    {
        protected const string TubFilesFolder = "TUB_Files";

        protected const string ControlFileName = "AvailableTranslations.json";

        protected const string indexFileName = "Index.zip";

        protected const string translationAnnotationsFileName = "TranslationAnnotations";

        protected const string paragraphAnnotationsFileName = "TranslationParagraphAnnotations";

        //private string currentAnnotationsFileNAme = "Annotations";

        /// <summary>
        /// Folder for existing files (translations)
        /// </summary>
        protected string ApplicationFolderTubFiles = "";


        #region File path creation
        /// <summary>
        /// Generates the control file full path
        /// </summary>
        /// <returns></returns>
        protected string ControlFilePath()
        {
            return Path.Combine(StaticObjects.Parameters.TUB_Files_RepositoryFolder, ControlFileName);
        }

        /// <summary>
        /// Generates the translation full path
        /// </summary>
        /// <param name="translationId"></param>
        /// <returns></returns>
        protected string TranslationFilePath(short translationId)
        {
            return Path.Combine(StaticObjects.Parameters.TUB_Files_RepositoryFolder, $"TR{translationId:000}.gz");
        }


        /// <summary>
        /// Generates the translation full path
        /// </summary>
        /// <param name="translationId"></param>
        /// <returns></returns>
        protected string TranslationJsonFilePath(short translationId)
        {
            return Path.Combine(StaticObjects.Parameters.TUB_Files_RepositoryFolder, $"TR{translationId:000}.json");
        }

        /// <summary>
        /// Generates the translation full path
        /// </summary>
        /// <param name="translationId"></param>
        /// <returns></returns>
        protected string TranslationAnnotationsJsonFilePath(short translationId)
        {
            return Path.Combine(StaticObjects.Parameters.TUB_Files_RepositoryFolder, $"{translationAnnotationsFileName}_{translationId:000}.json");
        }

        protected string ParagraphAnnotationsJsonFilePath(short translationId)
        {
            return Path.Combine(StaticObjects.Parameters.TUB_Files_RepositoryFolder, $"{paragraphAnnotationsFileName}_{translationId:000}.json");
        }

        #endregion

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
        protected string BytesToString(byte[] bytes, bool isZip = true)
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


    }
}
