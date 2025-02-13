using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Expressium.CucumberMessagesX
{
    public static class CucumberUtilities
    {
        public static string CapitalizeWords(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return Regex.Replace(value, @"(^\w)|(\s\w)", m => m.Value.ToUpper());
        }

        public static T DeserializeAsJson<T>(string filePath)
        {
            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;

            var jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(jsonString, options);
        }

        public static void SerializeAsJson<T>(string filePath, T objectRepository)
        {
            var jsonString = JsonSerializer.Serialize(objectRepository, new JsonSerializerOptions() { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
            File.WriteAllText(filePath, jsonString);
        }
    }
}
