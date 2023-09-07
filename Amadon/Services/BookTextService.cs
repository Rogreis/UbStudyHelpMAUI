using AmadonStandardLib.Classes;
using AmadonStandardLib.Helpers;
using AmadonStandardLib.UbClasses;
using Paragraph = AmadonStandardLib.UbClasses.Paragraph;

namespace Amadon.Services
{
    internal class BookTextService
    {

        #region Public Variables

        public static TextShowOption ColumnsOption = TextShowOption.LeftRight;
        public static string ColumnSize = "50%";
        public static List<string> ParagraphsAnchor = new List<string>();

        #endregion
        /// <summary>
        /// Get the paragraphs list from a translations
        /// </summary>
        /// <param name="t"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        private static List<Paragraph> GetParagraphs(Translation t, TOC_Entry entry)
        {
            List<Paragraph> list = t?.Paper(entry.Paper).Paragraphs;
            list.ForEach(p => {
                p.TranslationId = t.LanguageID;
            });
            return list;
        }

        /// <summary>
        /// Calculate the number of columns to show
        /// </summary>
        /// <returns></returns>
        private static TextShowOption CalculateShowOption()
        {
            StaticObjects.Parameters.ShowRight = StaticObjects.Parameters.ShowRight && StaticObjects.Parameters.TranslationsToShowId.Count > 0 && StaticObjects.Parameters.LanguageIDRightTranslation >= 0;
            StaticObjects.Parameters.ShowMiddle = StaticObjects.Parameters.ShowMiddle && StaticObjects.Parameters.TranslationsToShowId.Count > 1 && StaticObjects.Parameters.LanguageIDMiddleTranslation >= 0;

            if (StaticObjects.Parameters.ShowRight && StaticObjects.Parameters.ShowMiddle)
            {
                return TextShowOption.LeftMiddleRight;
            }
            if (!StaticObjects.Parameters.ShowRight && StaticObjects.Parameters.ShowMiddle)
            {
                return TextShowOption.LeftMiddle;
            }
            if (StaticObjects.Parameters.ShowRight && !StaticObjects.Parameters.ShowMiddle)
            {
                return TextShowOption.LeftRight;
            }
            return TextShowOption.LeftOnly;
        }



        /// <summary>
        /// Service api
        /// </summary>
        /// <returns>Json string for the object PaperText</returns>
        public static Task<PaperBookContent> GetHtml()
        {
            ColumnsOption = CalculateShowOption();
            int columnsNumber = 2;
            switch(ColumnsOption)
            {
                case TextShowOption.LeftOnly:
                    columnsNumber = 1;
                    break;
                case TextShowOption.LeftMiddleRight:
                    columnsNumber = 3;
                    break;
            }


            PaperBookContent bookTextContent = new PaperBookContent(columnsNumber);
            bookTextContent.Entry = StaticObjects.Parameters.Entry;

            // Get the paragraphs texts folowing what was required by user (StaticObjects.Parameters.TextShowOption)
            List<Paragraph>? leftParagraphs = null;
            List<Paragraph>? rightParagraphs = null;
            List<Paragraph>? middleParagraphs = null;

            // Left is always shown
            leftParagraphs = GetParagraphs(StaticObjects.Book.LeftTranslation, bookTextContent.Entry);


            ColumnsOption = CalculateShowOption();

            // Calculate the text to show option
            switch (ColumnsOption)
            {
                case TextShowOption.LeftOnly:
                    ColumnSize = "100%";
                    bookTextContent.AddTitle(StaticObjects.Book.LeftTranslation);
                    break;
                case TextShowOption.LeftRight:
                    ColumnSize = "50%";
                    bookTextContent.AddTitle(StaticObjects.Book.LeftTranslation);
                    bookTextContent.AddTitle(StaticObjects.Book.RightTranslation);
                    rightParagraphs = GetParagraphs(StaticObjects.Book.RightTranslation, bookTextContent.Entry);
                    break;
                case TextShowOption.LeftMiddle:
                    ColumnSize = "50%";
                    bookTextContent.AddTitle(StaticObjects.Book.LeftTranslation);
                    bookTextContent.AddTitle(StaticObjects.Book.MiddleTranslation);
                    middleParagraphs = GetParagraphs(StaticObjects.Book.MiddleTranslation, bookTextContent.Entry);
                    break;
                case TextShowOption.LeftMiddleRight:
                    ColumnSize = "33%";
                    bookTextContent.AddTitle(StaticObjects.Book.LeftTranslation);
                    bookTextContent.AddTitle(StaticObjects.Book.MiddleTranslation);
                    bookTextContent.AddTitle(StaticObjects.Book.RightTranslation);
                    rightParagraphs = GetParagraphs(StaticObjects.Book.RightTranslation, bookTextContent.Entry);
                    middleParagraphs = GetParagraphs(StaticObjects.Book.MiddleTranslation, bookTextContent.Entry);
                    break;
            }

            bookTextContent.AddParagraphs(leftParagraphs, middleParagraphs, rightParagraphs);
            ParagraphsAnchor = bookTextContent.ParagraphsAnchor;

            return Task.FromResult(bookTextContent);
        }

    }
}
