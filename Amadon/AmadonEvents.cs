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
    internal delegate void RazorComponentShownDelegate(string componentId);
    internal delegate void NewTocEntryDelegate(TOC_Entry entry);

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

        public static event RazorComponentShownDelegate OnRazorComponentShown;

        /// <summary>
        /// Fired when a new table of contents item is selected
        /// </summary>
        public static event NewTocEntryDelegate OnNewTocEntry;

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

        public static void RazorComponentShown(string componentId)
        {
            OnRazorComponentShown?.Invoke(componentId);
        }

        public static void NewTocEntry(TOC_Entry entry)
        {
            if (entry != null)
                OnNewTocEntry?.Invoke(entry);
        }

    }
}
