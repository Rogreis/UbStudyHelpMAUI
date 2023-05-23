using Amadon.Controls;
using AmadonStandardLib.UbClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amadon
{
    internal delegate void TopNavCommandDelegate(string controlName);
    internal delegate void TranslationsListInitializedDelegate();
    internal delegate void InitializationSuccesfullyDelegate();
    internal delegate void TranslationTocChangedDelegate();
    internal delegate void NewTocEntryDelegate(TOC_Entry entry);
    internal delegate void TranslationsToShowChangedDelegate();
    internal delegate void HighlightExpressionDelegate(string expression);
    internal delegate void NewSubjectIndexEntryDelegate(TOC_Entry entry);

    internal static class AmadonEvents
    {
        public const string ControlSettings = "settings";
        public const string ControlSearch = "search";
        public const string ControlHelp = "help";
        public const string ControlTrack = "track";
        public const string ControlIndex = "index";
        public const string ControlToc = "toc";

        /// <summary>
        /// Informs the system that a top navigation command has been issued.
        /// </summary>
        public static event TopNavCommandDelegate OnTopNavCommand;

        /// <summary>
        /// Informs the system that the list of translations has been initialized
        /// </summary>
        public static event TranslationsListInitializedDelegate OnTranslationsListInitialized;

        /// <summary>
        /// Fired when the initilization has finished succesfully
        /// </summary>
        public static event InitializationSuccesfullyDelegate OnInitializationSuccesfully;

        /// <summary>
        /// Fired when the translation to be used for TOC has changed
        /// </summary>
        public static event TranslationTocChangedDelegate OnTranslationTocChanged;

        /// <summary>
        /// Fired when the set of translation to have their text shown has changed
        /// </summary>
        public static event TranslationsToShowChangedDelegate OnTranslationsToShowChanged;

        /// <summary>
        /// Fired when a new table of contents item is selected
        /// </summary>
        public static event NewTocEntryDelegate OnNewTocEntry;

        /// <summary>
        /// Fired when a new table of contents item is selected
        /// </summary>
        public static event NewSubjectIndexEntryDelegate OnNewSubjectIndexEntry;

        /// <summary>
        /// Used to ask for a hightlight action (same as a search) for an expression in the current text page
        /// </summary>
        public static event HighlightExpressionDelegate OnHighlightExpression;



        // ============================================================================================================= 
        // Functions to fire the above events

        public static void TopNavCommand(string controlName)
        {
            OnTopNavCommand?.Invoke(controlName);
        }

        public static void TranslationsListInitialized()
        {
            OnTranslationsListInitialized?.Invoke();
        }

        public static void InitializationSuccesfully()
        {
            OnInitializationSuccesfully?.Invoke();
        }

        public static void TranslationTocChanged()
        {
            OnTranslationTocChanged?.Invoke();
        }

        public static void NewTocEntry(TOC_Entry entry)
        {
            if (entry != null)
                OnNewTocEntry?.Invoke(entry);
        }

        public static void NewSubjectIndexEntry(TOC_Entry entry)
        {
            if (entry != null)
                OnNewSubjectIndexEntry?.Invoke(entry);
        }

        public static void TranslationsToShowChanged()
        {
            OnTranslationsToShowChanged?.Invoke();
        }

        public static void HighlightExpression(string expression)
        {
            OnHighlightExpression?.Invoke(expression);
        }

    }
}
