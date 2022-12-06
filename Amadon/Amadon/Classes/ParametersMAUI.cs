using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UbStandardObjects;

namespace Amadon.Classes
{
    [Serializable]
    internal class ParametersMAUI : Parameters
    {
        public bool UseDarkThemme { get; set; } = true;

        public ControlsAppearance Appearance = new ControlsAppearance();


        /// <summary>
        /// Serialize the parameters instance
        /// </summary>
        /// <param name="p"></param>
        /// <param name="pathParameters"></param>
        public static void Serialize(ParametersMAUI p, string pathParameters)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    AllowTrailingCommas = true,
                    WriteIndented = true,
                };
                var jsonString = JsonSerializer.Serialize<ParametersMAUI>(p, options);
                File.WriteAllText(pathParameters, jsonString);
            }
            catch { }
        }

        /// <summary>
        /// Deserialize the parameters instance
        /// </summary>
        /// <param name="pathParameters"></param>
        /// <returns></returns>
        public static ParametersMAUI Deserialize(string pathParameters)
        {
            try
            {
                StaticObjects.Logger.Info("»»»» Deserialize Parameters");
                var jsonString = File.ReadAllText(pathParameters);
                return StaticObjects.DeserializeObject<ParametersMAUI>(jsonString);
            }
            catch
            {
                StaticObjects.Logger.Info("»»»» Deserialize Parameters creating default");
                return new ParametersMAUI();
            }
        }

    }
}
