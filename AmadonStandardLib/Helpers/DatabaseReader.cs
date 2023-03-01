﻿using AmadonStandardLib.UbClasses;
using System;
using System.Collections.Generic;
using System.IO;

namespace AmadonStandardLib.Helpers
{
    public class DatabaseReader : Database
    {


        /// <summary>
        /// Get all papers from the zipped file
        /// </summary>
        /// <param name="translationId"></param>
        /// <param name="isZip"></param>
        /// <returns></returns>
        protected string? GetFile(short translationId, bool isZip = true)
        {
            try
            {
                string json = "";

                string translationJsonFilePath = TranslationJsonFilePath(translationId);
                if (File.Exists(translationJsonFilePath))
                {
                    json = File.ReadAllText(translationJsonFilePath);
                    return json;
                }

                string translationStartupPath = TranslationFilePath(translationId);
                if (File.Exists(translationStartupPath))
                {
                    StaticObjects.Logger.Info("File exists: " + translationStartupPath);
                    byte[] bytes = File.ReadAllBytes(translationStartupPath);
                    json = BytesToString(bytes, isZip);
                    File.WriteAllText(translationJsonFilePath, json);
                    return json;
                }
                else
                {
                    StaticObjects.Logger.Error($"Translation not found {translationId}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                StaticObjects.Logger.Error("GetFile", ex);
                return null;
            }
        }


        /// <summary>
        /// Get the translations list from a local file
        /// </summary>
        /// <returns></returns>
        public List<Translation> GetTranslations()
        {
            string path = ControlFilePath();
            string json = File.ReadAllText(path);
            return Translations.DeserializeJson(json);
        }


        /// <summary>
        /// Get a translation from a local file
        /// </summary>
        /// <param name="translationId"></param>
        /// <returns></returns>
        public virtual Translation GetTranslation(short translationId, bool initializePapers = true)
        {
            Translation translation = StaticObjects.Book.GetTranslation(translationId);
            if (translation == null)
            {
                translation = new Translation();
            }

            if (!initializePapers) return translation;

            if (translation.Papers.Count > 0)
            {
                return translation;
            }

            // FIX ME: IsEditingTranslation is hard codded here, but needs to be 
            if (translationId == 2)
            {
                Translation translatioEdit = translation as Translation;
                if (translatioEdit == null)
                {
                    StaticObjects.Book.Translations.Remove(translation);
                    translatioEdit = new Translation(translation, StaticObjects.Parameters.EditParagraphsRepositoryFolder);
                    StaticObjects.Book.Translations.Add(translatioEdit);
                }
                translatioEdit.IsEditingTranslation = true;
                //translatioEdit.Annotations = null;
                return translation;
            }
            else
            {
                string json = GetFile(translationId, true);
                translation.GetData(json);

                // Loading annotations
                //translation.Annotations = null;
                return translation;
            }


        }

        /// <summary>
        /// Get the zipped format table json and unzipp it to return
        /// </summary>
        /// <returns></returns>
        public string? GetFormatTable()
        {
            try
            {
                string formatTableZippedPath = Path.Combine(StaticObjects.Parameters.TUB_Files_RepositoryFolder, @"FormatTable.gz");

                if (!File.Exists(formatTableZippedPath))
                {
                    StaticObjects.Logger.Error($"Format Table not found {formatTableZippedPath}");
                    return null;
                }

                StaticObjects.Logger.Info("File exists: " + formatTableZippedPath);
                byte[] bytes = File.ReadAllBytes(formatTableZippedPath);
                return BytesToString(bytes);
            }
            catch (Exception ex)
            {
                StaticObjects.Logger.Error("GetFormatTable", ex);
                return null;
            }
        }


        /// <summary>
        /// Get the json string for a paper notes
        /// </summary>
        /// <returns></returns>
        public string? GetNotes(short paperNo)
        {
            try
            {
                string filePath = Path.Combine(StaticObjects.Parameters.EditParagraphsRepositoryFolder, $@"{Paragraph.FolderPath(paperNo)}\Notes.json");
                string jsonString = File.ReadAllText(filePath);
                return "";
            }
            catch (Exception ex)
            {
                StaticObjects.Logger.Error("GetFormatTable", ex);
                return null;
            }
        }


    }
}
