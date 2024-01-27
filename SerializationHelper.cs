using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project1_1
{
    using System.IO;
    using System.Xml;
    using Newtonsoft.Json;

    static class SerializationHelper
    {
        public static T Deserialize<T>(string filePath) where T : new()
        {
            try
            {
                if (!File.Exists(filePath)) return new T(); 
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<T>(json) ?? new T();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при десериализации файла '{filePath}': {ex.Message}");
                return new T();
            }
        }

        public static void Serialize<T>(T data, string filePath)
        {
            try
            {
                string json = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сериализации в файл '{filePath}': {ex.Message}");
            }
        }
    }
}
