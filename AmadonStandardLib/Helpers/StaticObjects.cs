using AmadonStandardLib.Classes;
using AmadonStandardLib.UbClasses;
using System.Text.Json;

namespace AmadonStandardLib.Helpers
{
    public delegate void dlShowMessage(string message, bool isError = false, bool isFatal = false);

    public delegate void ShowStatusMessage(string message);

    public delegate void ShowPaperNumber(short paperNo);


    public static class StaticObjects
    {

        /// <summary>
        /// This is the object to store log
        /// </summary>
        public static Logger? Logger { get; set; }

        public static Parameters? Parameters { get; set; }

        public static Book Book { get; set; } = new Book();

        /// <summary>
        /// Serialize an object to string using json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize<T>(T obj)
        {
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                WriteIndented = true,
            };
            return JsonSerializer.Serialize<T>(obj, options);
        }

        /// <summary>
        /// Deserialize an object from a json string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string json) => JsonSerializer.Deserialize<T>(json);


    }
}
