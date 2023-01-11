using AmadonBlazorLibrary.Data;
using AmadonBlazorLibrary.UbClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AmadonBlazorLibrary.Helpers
{
    public delegate void dlShowMessage(string message, bool isError = false, bool isFatal = false);

    public delegate void dlShowExceptionMessage(string message, Exception ex, bool isFatal = false);

    public delegate void ShowStatusMessage(string message);

    public delegate void ShowPaperNumber(short paperNo);


    public static class StaticObjects
    {
        /// <summary>
        /// Control file name for different translations versions
        /// </summary>
        public const string ControlFileName = "UbControlFile.json";

        public static string PathParameters { get; set; } = "";

        public static string PathLog { get; set; } = "";

        /// <summary>
        /// This is the object to store log
        /// </summary>
        public static Logger Logger { get; set; }

        public static Parameters Parameters { get; set; }

        public static Book Book { get; set; } = new Book();

        #region events

        public static event dlShowMessage ShowMessage = null;
        public static event dlShowExceptionMessage ShowExceptionMessage = null;

        public static void FireSendMessage(string message, bool isError = false, bool isFatal = false)
        {
            ShowMessage?.Invoke(message, isError, isFatal);
        }

        public static void FireShowExceptionMessage(string message, Exception ex, bool isFatal = false)
        {
            ShowExceptionMessage?.Invoke(message, ex, isFatal);
        }

        #endregion

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
        public static T DeserializeObject<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }


    }
}
