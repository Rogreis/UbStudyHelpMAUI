using AmadonStandardLib.Helpers;
using AmadonStandardLib.InterchangeData;
using System.Text.Json;

namespace Amadon.Services
{
    internal class PersistentNotes
    {

        public static UserNotes NotesList = new UserNotes();

        private static string PathPersistentData(string dataName)
        {
            return Path.Combine(StaticObjects.Parameters.ApplicationDataFolder, $"{dataName}.json");
        }



        private static void Serialize<T>(T o, string name)
        {
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                WriteIndented = true,
            };
            string jsonString = JsonSerializer.Serialize<T>(o, options);
            File.WriteAllText(PathPersistentData(name), jsonString);
        }

        private static T DeSerialize<T>(string name) where T : new()
        {
            StaticObjects.Logger.Info($"»»»» Deserialize persistent object {name}");
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                WriteIndented = true,
            };

            string filePath = PathPersistentData(name);
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<T>(jsonString);
            }
            return new T();
        }



        /// <summary>
        /// Serialize the persistent objects
        /// </summary>
        public static void Serialize()
        {
            try
            {
                Serialize<UserNotes>(NotesList, "UserNotes");
            }
            catch (Exception ex)
            {
                StaticObjects.Logger.Error("Serialize persistent objects failure", ex);
            }
        }

        /// <summary>
        /// Deserialize persistent objects
        /// </summary>
        /// <returns></returns>
        public static bool Deserialize()
        {
            try
            {
                NotesList = DeSerialize<UserNotes>("UserNotes");
                return true;
            }
            catch (Exception ex)
            {
                StaticObjects.Logger.Error("»»»» Deserialize PersistentNotes objects failure, returning default", ex);
                return false;
            }
        }



    }
}
