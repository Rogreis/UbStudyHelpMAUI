using AmadonStandardLib.Classes;
using AmadonStandardLib.Helpers;
using AmadonStandardLib.InterchangeData;
using System.Text.Json;

namespace AmadonBlazorLibrary.Data
{
    public static class InitializationService
    {
        public static bool Looger()
        {
            return DataInitializer.InitLogger();
        }

        public static bool Parameters()
        {
            return DataInitializer.InitParameters();
        }

        public static bool Translations()
        {
            return DataInitializer.InitTranslationsList();
        }



        public static Task<bool> InitLooger()
        {
            return Task.FromResult(Looger());
        }

        public static Task<bool> InitParameters()
        {
            return Task.FromResult(Parameters());
        }

        public static Task<bool> InitRepositories(bool recreate= false)
        {
            return Task.FromResult(DataInitializer.Repositories(recreate));
        }

        public static Task<bool> InitTranslations()
        {
            return Task.FromResult(Translations());
        }

        public static Task<bool> InitTranslation(string transToInit, short id)
        {
            return Task.FromResult(DataInitializer.InitTranslation(transToInit, id));
        }


    }
}
