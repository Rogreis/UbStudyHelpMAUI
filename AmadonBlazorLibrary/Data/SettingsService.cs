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
        public static Task Store()
        {
            Parameters.Serialize(StaticObjects.Parameters, Parameters.PathParameters);
            return Task.CompletedTask;
        }

        public static Task Get()
        {
            StaticObjects.Parameters= Parameters.Deserialize(Parameters.PathParameters);
            return Task.CompletedTask;
        }
    }
}
