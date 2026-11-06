using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;
using System;
using System.IO;
using System.Linq;

namespace Expressium.LivingDoc.UnitTests.Converters
{
    /// <summary>
    /// Reflects the historical scenario view of a native LivingDoc Json file...
    ///
    ///   @TA-3001  @Fixed
    ///   Scenario: Successful Submitting a Contact Us Inquiry   12s 111ms
    ///     Given I have logged in with valid user credentials
    ///     When I complete and submit the Contact Us formular
    ///     Then I should receive an inquiry confirmation message
    ///   History
    ///     (grey)   Mon 1 Sep 2025 at 06:30:00 GMT+2
    ///     (yellow) Tue 2 Sep 2025 at 15:50:00 GMT+2
    ///     (red)    Wed 3 Sep 2025 at 19:36:40 GMT+2
    ///     (green)  Thu 4 Sep 2025 at 12:42:28 GMT+2
    /// </summary>
    internal class LivingDocConverterNativeHistoryTests
    {
        private static readonly DateTimeOffset[] HistoryDates =
        {
            new DateTimeOffset(2025, 9, 1, 6, 30, 0, TimeSpan.FromHours(2)),
            new DateTimeOffset(2025, 9, 2, 15, 50, 0, TimeSpan.FromHours(2)),
            new DateTimeOffset(2025, 9, 3, 19, 36, 40, TimeSpan.FromHours(2)),
            new DateTimeOffset(2025, 9, 4, 12, 42, 28, TimeSpan.FromHours(2))
        };

        private static readonly string[] HistoryStatuses =
        {
            LivingDocStatuses.Skipped.ToString(),
            LivingDocStatuses.Incomplete.ToString(),
            LivingDocStatuses.Failed.ToString(),
            LivingDocStatuses.Passed.ToString()
        };

        private static readonly string[] HistoryDisplayDates =
        {
            "Mon 1 Sep 2025 at 06:30:00 GMT+2",
            "Tue 2 Sep 2025 at 15:50:00 GMT+2",
            "Wed 3 Sep 2025 at 19:36:40 GMT+2",
            "Thu 4 Sep 2025 at 12:42:28 GMT+2"
        };

        private LivingDocProject project;
        private LivingDocScenario scenario;
        private LivingDocExample example;

        [SetUp]
        public void SetUp()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native-history.json");

            project = new LivingDocConverter().Import(inputFilePath);
            scenario = project.Features[0].Scenarios[0];
            example = scenario.Examples[0];
        }

        private static bool IsLocalTimeZoneGmtPlusTwo()
        {
            return HistoryDates.All(d => TimeZoneInfo.Local.GetUtcOffset(d) == TimeSpan.FromHours(2));
        }

        [Test]
        public void NativeHistory_Import_Scenario()
        {
            Assert.That(scenario.Name, Is.EqualTo("Successful Submitting a Contact Us Inquiry"));
            Assert.That(scenario.Tags, Is.EqualTo(new[] { "@TA-3001" }));
            Assert.That(scenario.Health, Is.EqualTo(LivingDocHealths.Fixed.ToString()));
            Assert.That(scenario.IsFixed(), Is.True);
            Assert.That(scenario.GetDataTags(), Is.EqualTo("@TA-3001 @Fixed"));
            Assert.That(scenario.GetStatus(), Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(example.GetDuration(), Is.EqualTo("12s 111ms"));
        }

        [Test]
        public void NativeHistory_Import_Steps()
        {
            Assert.That(example.Steps.Select(s => s.GetFullName()), Is.EqualTo(new[]
            {
                "Given I have logged in with valid user credentials",
                "When I complete and submit the Contact Us formular",
                "Then I should receive an inquiry confirmation message"
            }));

            Assert.That(example.Steps.All(s => s.IsPassed()), Is.True);
        }

        [Test]
        public void NativeHistory_Import_HistoryStatuses()
        {
            Assert.That(example.History.Count, Is.EqualTo(4));
            Assert.That(example.History.Select(h => h.Status), Is.EqualTo(HistoryStatuses));
        }

        [Test]
        public void NativeHistory_Import_HistoryDates()
        {
            Assert.That(example.History.Select(h => h.Date), Is.Ordered.Ascending);

            for (int i = 0; i < HistoryDates.Length; i++)
                Assert.That(example.History[i].Date.ToUniversalTime(), Is.EqualTo(HistoryDates[i].UtcDateTime));
        }

        [Test]
        public void NativeHistory_GetDate_MatchesLocalTime()
        {
            for (int i = 0; i < HistoryDates.Length; i++)
                Assert.That(example.History[i].GetDate(), Is.EqualTo(HistoryDates[i].LocalDateTime.FormatAsString()));
        }

        [Test]
        public void NativeHistory_GetDate_MatchesScenarioPicture()
        {
            // The displayed offset follows the local time zone, e.g. Europe/Copenhagen in September...
            Assume.That(IsLocalTimeZoneGmtPlusTwo(), "Requires a local time zone at GMT+2 in September 2025");

            Assert.That(example.History.Select(h => h.GetDate()), Is.EqualTo(HistoryDisplayDates));
        }

        [Test]
        public void NativeHistory_Health_IsRecalculatedAsFixed()
        {
            scenario.Health = null;

            project.MergeScenarioHistoryHealth();

            Assert.That(scenario.Health, Is.EqualTo(LivingDocHealths.Fixed.ToString()));
        }

        [Test]
        public void NativeHistory_Project_HasHistory()
        {
            Assert.That(project.HasHistory(), Is.True);
            Assert.That(project.HasHealth(), Is.True);
            Assert.That(project.History.Features.Count, Is.EqualTo(4));
            Assert.That(project.History.Scenarios.Count, Is.EqualTo(4));
            Assert.That(project.History.Steps.Count, Is.EqualTo(4));
            Assert.That(project.History.Scenarios.Last().Date, Is.EqualTo(project.Date));
        }

        [Test]
        public void NativeHistory_GenerateDataScenarioTags_ContainsHealthTag()
        {
            var listOfLines = new LivingDocDataObjectsGenerator(project).GenerateDataScenarioTags(scenario);

            Assert.That(listOfLines, Does.Contain("<span class='scenario-tag'>@TA-3001</span>"));
            Assert.That(listOfLines, Does.Contain("<span class='scenario-tag-health color-fixed'>@Fixed</span>"));
        }

        [Test]
        public void NativeHistory_GenerateDataScenarioName_ContainsHistoryToggle()
        {
            var listOfLines = new LivingDocDataObjectsGenerator(project).GenerateDataScenarioName(scenario, example, string.Empty);

            Assert.That(listOfLines, Does.Contain("<span class='scenario-name'>Successful Submitting a Contact Us Inquiry</span>"));
            Assert.That(listOfLines, Does.Contain("<span class='scenario-duration'>12s 111ms</span>"));
            Assert.That(listOfLines.Any(l => l.Contains("title='Toggle History'")), Is.True);
        }

        [Test]
        public void NativeHistory_GenerateDataScenarioHistory_DotsAndDates()
        {
            var listOfLines = new LivingDocDataObjectsGenerator(project).GenerateDataScenarioHistory(example);

            Assert.That(listOfLines, Does.Contain("<span class='history-keyword'>History</span>"));

            var historyLines = listOfLines.Where(l => l.StartsWith("<li><span class='status-dot")).ToList();
            Assert.That(historyLines.Count, Is.EqualTo(4));

            for (int i = 0; i < HistoryStatuses.Length; i++)
            {
                var expected = $"<li><span class='status-dot bgcolor-{HistoryStatuses[i].ToLower()}'></span> {example.History[i].GetDate()}</li>";
                Assert.That(historyLines[i], Is.EqualTo(expected));
            }
        }

        [Test]
        public void NativeHistory_GenerateDataAnalyticsTrends_Scenarios()
        {
            var listOfLines = new LivingDocDataAnalyticsGenerator(project).GenerateDataAnalyticsTrends("Scenarios");

            var passed = listOfLines.Where(l => l.StartsWith("<div class='bgcolor-passed'")).ToArray();
            var incomplete = listOfLines.Where(l => l.StartsWith("<div class='bgcolor-incomplete'")).ToArray();
            var failed = listOfLines.Where(l => l.StartsWith("<div class='bgcolor-failed'")).ToArray();

            Assert.That(passed.Length, Is.EqualTo(4));

            // Widths are relative to the largest run (10 scenarios)...
            Assert.That(passed[0], Does.Contain("title='2' style='width: 20%;"));
            Assert.That(incomplete[0], Does.Contain("title='1' style='width: 10%;"));
            Assert.That(failed[0], Does.Contain("title='2' style='width: 20%;"));

            Assert.That(passed[3], Does.Contain("title='6' style='width: 60%;"));
            Assert.That(incomplete[3], Does.Contain("title='1' style='width: 10%;"));
            Assert.That(failed[3], Does.Contain("title='3' style='width: 30%;"));

            foreach (var history in project.History.Scenarios)
                Assert.That(listOfLines, Does.Contain($"<td>{history.GetDate()}</td>"));
        }

        [Test]
        public void NativeHistory_Generate_HtmlContainsHistory()
        {
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native-history.html");
            File.Delete(outputFilePath);

            new LivingDocConverter().Generate(project, outputFilePath);

            var htmlFile = File.ReadAllText(outputFilePath);
            Assert.That(htmlFile, Does.Contain("Successful Submitting a Contact Us Inquiry"));
            Assert.That(htmlFile, Does.Contain("@Fixed"));

            foreach (var history in example.History)
                Assert.That(htmlFile, Does.Contain(history.GetDate()));
        }
    }
}
