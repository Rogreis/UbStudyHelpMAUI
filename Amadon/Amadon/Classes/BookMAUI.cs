using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UbStandardObjects.Objects;
using UbStandardObjects;

namespace AmadonBlazor.Classes
{
    internal class BookMAUI : Book
    {
        public BookMAUI()
        {
            EventsControl.AnnotationsChanges += EventsControl_AnnotationsChanges;

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
                EventsControl.FireTranslationsChanged();
            }
            catch (Exception ex)
            {
                string message = $"General error changing translation: {ex.Message}. May be you do not have the correct data to use this tool.";
                StaticObjects.Logger.Error(message, ex);
                EventsControl.FireFatalError("Data not loaded!");

            }
        }

        public override void StoreAnnotations(TOC_Entry entry, List<UbAnnotationsStoreData> annotations)
        {
            //((GetDataFilesCore)DataFiles).StoreAnnotations(entry, annotations);
        }

        /// <summary>
        /// Remove annotations from the translation list
        /// Do not need to store; this will be done for another event fired next
        /// </summary>
        /// <param name="entry"></param>
        public override void DeleteAnnotations(TOC_Entry entry)
        {
            //    UbAnnotationsStoreData data = LeftTranslation.Annotations.Find(a => a.Entry == entry);
            //    if (data != null)
            //    {
            //        LeftTranslation.Annotations.Remove(data);
            //        EventsControl.FireAnnotationsChanges(entry);
            //        return;
            //    }

            //    data = RightTranslation.Annotations.Find(a => a.Entry == entry);
            //    if (data != null)
            //    {
            //        RightTranslation.Annotations.Remove(data);
            //        EventsControl.FireAnnotationsChanges(entry);
            //        return;
            //    }
        }

        private void EventsControl_AnnotationsChanges(TOC_Entry entry)
        {
            //StoreAnnotations(entry, entry.TranslationId == StaticObjects.Parameters.LanguageIDLeftTranslation ? LeftTranslation.Annotations : RightTranslation.Annotations);
        }
    }
}
