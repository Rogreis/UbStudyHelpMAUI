using AmadonStandardLib.Classes;
using AmadonStandardLib.Helpers;
using System.Text.Json;

namespace Amadon.Services
{
    internal static class PersistentData
    {
        private static string PathPersistentData(string dataName)
        {
            return Path.Combine(StaticObjects.Parameters.ApplicationDataFolder, $"{dataName}.json");
        }

        public static IList<ItemForToc> ExpandedNodesList= new List<ItemForToc>();


        //public static bool SetItem<T>(string storageName, string key, T data, CancellationToken cancellationToken = default)
        //{
        //    if (string.IsNullOrWhiteSpace(key) || data == null)
        //        return false;
        //    string pathLocalStorage = DataInitializer.GetLocalStorageFolder(storageName);
        //    string json = File.ReadAllText(pathLocalStorage);
        //    Dictionary<string,object> dicData = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
        //    if (dicData == null) dicData = new Dictionary<string, object>();
        //    dicData.Add(key, data);
        //    json = JsonSerializer.Serialize<Dictionary<string, object>>(dicData);
        //    File.WriteAllText(pathLocalStorage, json);
        //    return true;
        //}

        //public static T? GetItem<T>(string storageName, string key)
        //{
        //    if (string.IsNullOrWhiteSpace(key))
        //        return default(T);

        //    string pathLocalStorage = DataInitializer.GetLocalStorageFolder(storageName);
        //    string json = File.ReadAllText(pathLocalStorage);
        //    Dictionary<string, object> dicData = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
        //    if (dicData == null || !dicData.ContainsKey(key)) return default(T);
        //    dicData.TryGetValue(key, out object obj);
        //    if (obj == null) return default(T);
        //    return (T)obj;
        //}

        /// <summary>
        /// Serialize the persistent objects
        /// </summary>
        public static void Serialize()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    AllowTrailingCommas = true,
                    WriteIndented = true,
                };
                if (StaticObjects.Parameters != null)
                {
                    var jsonString = JsonSerializer.Serialize<IList<ItemForToc>>(ExpandedNodesList, options);
                    File.WriteAllText(PathPersistentData("ExpandedNodesList"), jsonString);
                }
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
                StaticObjects.Logger.Info("»»»» Deserialize persistent objects");
                string filePath= PathPersistentData("ExpandedNodesList");
                if (File.Exists(filePath)) 
                {
                    var jsonString = File.ReadAllText(filePath);
                    ExpandedNodesList = JsonSerializer.Deserialize<IList<ItemForToc>>(jsonString);
                }
                return true;
            }
            catch (Exception ex)
            {
                ExpandedNodesList= new List<ItemForToc>();
                StaticObjects.Logger.Error("»»»» Deserialize persistent objects failure, returning default", ex);
                return false;
            }
        }



    }
}
