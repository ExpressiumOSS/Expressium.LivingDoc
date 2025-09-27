using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Expressium.LivingDoc.Models
{
    public static class LivingDocSerializer
    {
        public static T DeserializeAsJson<T>(string filePath)
        {
            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;

            var jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(jsonString, options);
        }

        public static void SerializeAsJson<T>(string filePath, T obj)
        {
            var jsonString = JsonSerializer.Serialize(obj, new JsonSerializerOptions() { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
            File.WriteAllText(filePath, jsonString);
        }

        public static T DeepClone<T>(this T obj)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            var json = JsonSerializer.Serialize(obj);
            return JsonSerializer.Deserialize<T>(json)!;
        }
    }
}
