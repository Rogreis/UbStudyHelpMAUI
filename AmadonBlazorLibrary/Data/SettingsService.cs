using AmadonStandardLib.Classes;
using AmadonStandardLib.Helpers;
using AmadonStandardLib.UbClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AmadonBlazorLibrary.Data
{
    public class SettingsService
    {
        private static JsonSerializerOptions options = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            WriteIndented = true,
            IncludeFields = true,
        };

        public static Task<string> Set(string json)
        {
            StaticObjects.Parameters= JsonSerializer.Deserialize<Parameters>(json);
            Parameters.Serialize(StaticObjects.Parameters, Parameters.PathParameters);
            var jsonString = JsonSerializer.Serialize(StaticObjects.Parameters, options);
            return Task.FromResult(jsonString);
        }

        public static Task<string> Get()
        {
            var jsonString = JsonSerializer.Serialize(StaticObjects.Parameters, options);
            return Task.FromResult(jsonString);
        }
    }
}
