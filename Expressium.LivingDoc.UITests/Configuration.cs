using Expressium.LivingDoc.UITests.Controls;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Expressium.LivingDoc.UITests
{
    public class Configuration
    {
        public enum Profiles
        {
            Development,
            Test,
            PreProduction,
            Production
        }

        public string Company { get; set; }
        public string Project { get; set; }
        public string Environment { get; set; }
        public string Url { get; set; }
        public bool Logging { get; set; }
        public string LoggingPath { get; set; }
        public string BrowserType { get; set; }
        public bool Maximize { get; set; }
        public bool Headless { get; set; }
        public bool WindowSize { get; set; }
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public bool Highlight { get; set; }
        public int HighlightTimeOut { get; set; }
        public bool Screenshot { get; set; }

        public static Configuration GetCurrentConfiguation()
        {
            var profile = System.Environment.GetEnvironmentVariable("PROFILE");

            if (profile == null || !Enum.IsDefined(typeof(Profiles), profile))
                profile = Profiles.Development.ToString();

            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("configuration.json").Build()
               .GetSection("Profiles").GetSection(profile).Get<Configuration>();

            if (configuration == null)
                throw new ApplicationException($"Configuration profile '{profile}' is unknown...");

            if (configuration.LoggingPath.StartsWith(".\\"))
                configuration.LoggingPath = Path.Combine(Directory.GetCurrentDirectory(), configuration.LoggingPath.Replace(".\\", ""));

            if (configuration.Url.StartsWith(".\\"))
                configuration.Url = Path.Combine(Directory.GetCurrentDirectory(), configuration.Url.Replace(".\\", ""));

            // Initialize WebElements Settings...
            WebElements.Highlight = configuration.Highlight;
            WebElements.HighlightTimeOut = configuration.HighlightTimeOut;

            return configuration;
        }
    }
}
