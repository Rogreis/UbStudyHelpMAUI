using AmadonStandardLib.Classes;
using AmadonStandardLib.Helpers;
using AmadonStandardLib.InterchangeData;
using AmadonStandardLib.UbClasses;
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

        public static async Task<bool> InitParameters()
        {
            Parameters();
            await SettingsService.Get();
            return true;
        }

        public static async Task<bool> StoreParameters()
        {
            await SettingsService.Store();
            return true;
        }


        public static Task<bool> VerifyDataFolder(bool recreate= false)
        {
            return Task.FromResult(DataInitializer.VerifyDataFolder(recreate));
        }

        public static Task<bool> InitTranslations()
        {
            return Task.FromResult(Translations());
        }

        public static Task<Translation> InitTranslation(Translation? previousTranslation, short id, Diagnostic diagnostic)
        {
            return Task.FromResult(DataInitializer.InitTranslation(previousTranslation, id, diagnostic));
        }


        public static async Task<Diagnostic> InitLeftTranslation(short newTranslationId)
        {
            Diagnostic diagnostic = new Diagnostic();
            if (newTranslationId < 0)
            {
                diagnostic.IsError = true;
                diagnostic.Message = "Left translation ID must be greater than 0";
                return diagnostic;
            }
            else
            {
                Translation? trans = await InitializationService.InitTranslation(StaticObjects.Book.LeftTranslation, newTranslationId, diagnostic);
                if (diagnostic.IsError)
                {
                    return diagnostic;
                }
                StaticObjects.Book.LeftTranslation = trans;
                StaticObjects.Parameters.LanguageIDLeftTranslation = trans.LanguageID;
                diagnostic.IsError = false;
                diagnostic.Message = "";
                return diagnostic;
            }
        }

        public static async Task<Diagnostic> InitMiddleTranslation(short newTranslationId)
        {
            Diagnostic diagnostic = new Diagnostic();
            if (newTranslationId < 0)
            {
                diagnostic.IsError = true;
                diagnostic.Message = "Middle translation ID must be greater than 0";
                return diagnostic;
            }
            else
            {
                Translation? trans = await InitializationService.InitTranslation(StaticObjects.Book.MiddleTranslation, newTranslationId, diagnostic);
                if (diagnostic.IsError)
                {
                    return diagnostic;
                }
                StaticObjects.Book.MiddleTranslation = trans;
                StaticObjects.Parameters.LanguageIDMiddleTranslation = trans.LanguageID;
                diagnostic.IsError = false;
                diagnostic.Message = "";
                return diagnostic;
            }
        }

        public static async Task<Diagnostic> InitRightTranslation(short newTranslationId)
        {
            Diagnostic diagnostic = new Diagnostic();
            if (newTranslationId < 0)
            {
                diagnostic.IsError = true;
                diagnostic.Message = "Right translation ID must be greater than 0";
                return diagnostic;
            }
            else
            {
                Translation? trans = await InitializationService.InitTranslation(StaticObjects.Book.RightTranslation, newTranslationId, diagnostic);
                if (diagnostic.IsError)
                {
                    return diagnostic;
                }
                StaticObjects.Book.RightTranslation = trans;
                StaticObjects.Parameters.LanguageIDRightTranslation = trans.LanguageID;
                diagnostic.IsError = false;
                diagnostic.Message = "";
                return diagnostic;
            }
        }


    }
}
