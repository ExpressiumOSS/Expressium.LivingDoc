using Io.Cucumber.Messages.Types;
using System;
using System.Text.Json;

namespace Expressium.LivingDoc.Parsers
{
    /// <summary>
    /// When using System.Text.Json to serialize a Cucumber Message Envelope, the following serialization options are used.
    /// Consumers of Cucumber.Messages should use these options, or their serialization library's equivalent options.
    /// These options should work with System.Text.Json v6 or above.
    /// </summary>
    internal class MessagesSerializer
    {
        private static readonly Lazy<JsonSerializerOptions> _jsonOptions = new(() =>
        {
            var options = new JsonSerializerOptions();
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.Converters.Add(new CucumberMessageEnumConverter<AttachmentContentEncoding>());
            options.Converters.Add(new CucumberMessageEnumConverter<PickleStepType>());
            options.Converters.Add(new CucumberMessageEnumConverter<SourceMediaType>());
            options.Converters.Add(new CucumberMessageEnumConverter<StepDefinitionPatternType>());
            options.Converters.Add(new CucumberMessageEnumConverter<StepKeywordType>());
            options.Converters.Add(new CucumberMessageEnumConverter<TestStepResultStatus>());
            options.Converters.Add(new CucumberMessageEnumConverter<HookType>());
            options.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            return options;
        });

        internal static Envelope Deserialize(string json)
        {
            return Deserialize<Envelope>(json);
        }

        internal static T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, JsonOptions);
        }

        private static JsonSerializerOptions JsonOptions
        {
            get
            {
                return _jsonOptions.Value;
            }
        }
    }
}
