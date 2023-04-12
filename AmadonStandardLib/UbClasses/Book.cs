
using AmadonStandardLib.Classes;
using AmadonStandardLib.Helpers;
using System;
using System.Collections.Generic;

namespace AmadonStandardLib.UbClasses
{
    public class Book
    {
        protected DatabaseReader DataFiles = new DatabaseReader();


        public Translation? LeftTranslation { get; set; }

        public Translation? MiddleTranslation { get; set; }

        public Translation? RightTranslation { get; set; }

        public List<Translation>? Translations { get; set; } = null;

        public List<Translation> ObservableTranslations
        {
            get
            {
                List<Translation> list = new List<Translation>();
                list.AddRange(Translations);
                return list;
            }
        }

        public FormatTable? FormatTableObject { get; set; } = null;

        /// <summary>
        /// Inicialize the list of available translations
        /// </summary>
        /// <returns></returns>
        protected bool InicializeTranslations()
        {
            try
            {
                if (Translations == null)
                {
                    Translations = DataFiles.GetTranslations();
                }
                return true;
            }
            catch (Exception ex)
            {
                string message = $"Could not initialize available translations. See log.";
                StaticObjects.Logger.Error(message, ex);
                LibraryEventsControl.FireFatalError(message);
                return false;
            }
        }


        /// <summary>
        /// Get a translation from the list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Translation? GetTranslation(short id)
        {
            Translation trans = Translations.Find(o => o.LanguageID == id);
            if (trans == null)
            {
                string message = $"Missing translation number {id}. May be you do not have the correct data to use this tool.";
                LibraryEventsControl.FireFatalError(message);
                return null;
            }
            return trans;
        }

        /// <summary>
        /// Initialize the format table used for editing translations
        /// </summary>
        public FormatTable? GetFormatTable()
        {
            try
            {
                if (FormatTableObject == null)
                {
                    FormatTableObject = new FormatTable(DataFiles.GetFormatTable());
                }
                return FormatTableObject;
            }
            catch (Exception ex)
            {
                StaticObjects.Logger.Error($"Missing format table. May be you do not have the correct data to use this tool.", ex);
                return null;
            }
        }

        private Translation? InitializeTranslation(short id)
        {
            if (id < 0) return null;
            Translation trans = DataFiles.GetTranslation(id);
            if (trans == null || !trans.CheckData()) return null;
            return trans;
        }


        /// <summary>
        /// Inicialize book and 2 translations
        /// </summary>
        /// <returns></returns>
        public bool Inicialize()
        {
            try
            {
                if (!InicializeTranslations())
                {
                    return false;
                }
                
                LeftTranslation = InitializeTranslation(StaticObjects.Parameters.LanguageIDLeftTranslation);
                MiddleTranslation = InitializeTranslation(StaticObjects.Parameters.LanguageIDMiddleTranslation);
                RightTranslation = InitializeTranslation(StaticObjects.Parameters.LanguageIDRightTranslation);
                return true;
            }
            catch (Exception ex)
            {
                string message = $"Could not initialize translations. See log.";
                StaticObjects.Logger.Error(message, ex);
                LibraryEventsControl.FireFatalError(message);
                return false;
            }
        }



        //public virtual void StoreAnnotations(TOC_Entry entry, List<UbAnnotationsStoreData> annotations)
        //{

        //}

        public virtual void DeleteAnnotations(TOC_Entry entry)
        {

        }

        public void SetNewTranslation(Translation translation, bool isLeft = true)
        {
            try
            {
                if (isLeft)
                {
                    StaticObjects.Parameters.LanguageIDLeftTranslation = translation.LanguageID;
                    LeftTranslation = DataFiles.GetTranslation(translation.LanguageID);
                    StaticObjects.Logger.IsNull(LeftTranslation, $"Invalid of non existing translation: {translation.LanguageID}");
                    if (!LeftTranslation.CheckData())
                    {
                        return;
                    }
                }
                else
                {
                    StaticObjects.Parameters.LanguageIDRightTranslation = translation.LanguageID;
                    RightTranslation = DataFiles.GetTranslation(translation.LanguageID);
                    StaticObjects.Logger.IsNull(RightTranslation, $"Invalid of non existing translation: {translation.LanguageID}");
                    if (!RightTranslation.CheckData())
                    {
                        return;
                    }
                }
                LibraryEventsControl.FireTranslationsChanged();
            }
            catch (Exception ex)
            {
                string message = $"General error changing translation: {ex.Message}. May be you do not have the correct data to use this tool.";
                StaticObjects.Logger.Error(message, ex);
                LibraryEventsControl.FireFatalError("Data not loaded!");

            }
        }

        //public void StoreAnnotations(TOC_Entry entry, List<UbAnnotationsStoreData> annotations)
        //{
        //    //((GetDataFilesCore)DataFiles).StoreAnnotations(entry, annotations);
        //}


        private void EventsControl_AnnotationsChanges(TOC_Entry entry)
        {
            //StoreAnnotations(entry, entry.TranslationId == StaticObjects.Parameters.LanguageIDLeftTranslation ? LeftTranslation.Annotations : RightTranslation.Annotations);
        }

    }
}
