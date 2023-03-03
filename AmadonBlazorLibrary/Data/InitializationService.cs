using AmadonStandardLib.Helpers;
using AmadonStandardLib.InterchangeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Schema;

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

        public static bool Translation()
        {
            return DataInitializer.InitTranslations();
        }

        public static Task<InitResult> InitAll(InitResult init)
        {
            if (init.TranslationsOnly) 
            {
                init.TranslationsOk = Translation();
            }
            else
            {
                init.LoggerOk = Looger();
                init.ParameterOk = Parameters();
                init.TranslationsOk = Translation();
            }
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                WriteIndented = true,
            };

            var jsonString = JsonSerializer.Serialize(init, options);
            return Task.FromResult(init);
        }


    }
}
