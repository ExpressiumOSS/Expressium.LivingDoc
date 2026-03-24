using System;
using System.Text.Json;

namespace Expressium.LivingDoc.Models
{
    public static class LivingDocExtensions
    {
        public static T DeepClone<T>(this T obj)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            var json = JsonSerializer.Serialize(obj);
            return JsonSerializer.Deserialize<T>(json)!;
        }
    }
}
