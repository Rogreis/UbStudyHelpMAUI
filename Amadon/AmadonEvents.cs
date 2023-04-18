using Amadon.Controls;
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

    internal static class AmadonEvents
    {
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

    }
}
