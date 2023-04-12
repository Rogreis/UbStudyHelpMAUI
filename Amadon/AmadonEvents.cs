using Amadon.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amadon
{
    internal delegate void TopNavCommandDelegate(string controlName);
    internal delegate void SystemInitializedDelegate(bool success);
    internal delegate void TranslationsListInitializedDelegate();

    internal static class AmadonEvents
    {
        /// <summary>
        /// Informs the system that a top navigation command has been issued.
        /// </summary>
        public static event TopNavCommandDelegate OnTopNavCommand;

        /// <summary>
        /// Informs the system that initiazilation has finished 
        /// </summary>
        public static event SystemInitializedDelegate OnSystemInitialized;

        /// <summary>
        /// Informs the system that the list of translations has been initialized
        /// </summary>
        public static event TranslationsListInitializedDelegate OnTranslationsListInitialized;

        public static void TopNavCommand(string controlName)
        {
            OnTopNavCommand?.Invoke(controlName);
        }

        public static void SystemInitialized(bool success)
        {
            OnSystemInitialized?.Invoke(success);
        }

        public static void TranslationsListInitialized()
        {
            OnTranslationsListInitialized?.Invoke();
        }

    }
}
