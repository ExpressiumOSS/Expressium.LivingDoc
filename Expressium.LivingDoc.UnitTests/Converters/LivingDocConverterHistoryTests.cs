using System;
using System.IO;
using System.Linq;

namespace Expressium.LivingDoc.UnitTests.Converters
{
    internal class LivingDocConverterHistoryTests
    {
        private string tempDirectory;
        private string currentRunPath;
        private string[] historyRunPaths;

        [SetUp]
        public void SetUp()
        {
            tempDirectory = Path.Combine(Path.GetTempPath(), "LivingDocHistoryTests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDirectory);

            var samples = Path.Combine(Directory.GetCurrentDirectory(), "Samples");
            currentRunPath = Path.Combine(samples, "coffeeshop.ndjson");

            // Five runs with distinct test run dates (oldest first)...
            historyRunPaths = new[]
            {
                Path.Combine(samples, "History", "coffeeshop1.ndjson"),
                Path.Combine(samples, "History", "coffeeshop2.ndjson"),
                Path.Combine(samples, "History", "coffeeshop3.ndjson"),
                Path.Combine(samples, "History", "coffeeshop4.ndjson"),
                Path.Combine(samples, "coffeeshop.ndjson")
            };
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(tempDirectory))
                Directory.Delete(tempDirectory, true);
        }

        private string CopyHistory(string sourcePath, string relativeTargetPath, DateTime lastWriteTime)
        {
            var targetPath = Path.Combine(tempDirectory, relativeTargetPath);
            Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
            File.Copy(sourcePath, targetPath, true);
            File.SetLastWriteTime(targetPath, lastWriteTime);
            return targetPath;
        }

        [Test]
        public void LivingDocConverter_MergeHistory_UsesFourMostRecentlyWrittenFiles()
        {
            var now = DateTime.Now;
            for (int i = 0; i < historyRunPaths.Length; i++)
                CopyHistory(historyRunPaths[i], $"run{i + 1}.ndjson", now.AddMinutes(i - 10));

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(currentRunPath, "Title");
            livingDocConverter.MergeHistory(livingDocProject, Path.Combine(tempDirectory, "run.ndjson"));

            var oldestRunDate = livingDocConverter.Convert(historyRunPaths[0], null).Date;

            Assert.That(livingDocProject.History.Scenarios.Count, Is.EqualTo(4));
            Assert.That(livingDocProject.History.Scenarios.Any(h => h.Date == oldestRunDate), Is.False);
        }

        [Test]
        public void LivingDocConverter_MergeHistory_LimitIsBasedOnFileWriteTime_NotRunDate()
        {
            // The run with the oldest test run date is the most recently written file...
            var now = DateTime.Now;
            CopyHistory(historyRunPaths[0], "run1.ndjson", now);
            for (int i = 1; i < historyRunPaths.Length; i++)
                CopyHistory(historyRunPaths[i], $"run{i + 1}.ndjson", now.AddMinutes(-i));

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(currentRunPath, "Title");
            livingDocConverter.MergeHistory(livingDocProject, Path.Combine(tempDirectory, "run.ndjson"));

            var oldestRunDate = livingDocConverter.Convert(historyRunPaths[0], null).Date;
            var excludedRunDate = livingDocConverter.Convert(historyRunPaths[4], null).Date;

            Assert.That(livingDocProject.History.Scenarios.Count, Is.EqualTo(4));
            Assert.That(livingDocProject.History.Scenarios.Any(h => h.Date == oldestRunDate), Is.True);
            Assert.That(livingDocProject.History.Scenarios.Any(h => h.Date == excludedRunDate), Is.False);
        }

        [Test]
        public void LivingDocConverter_MergeHistory_IsOrderedByRunDate()
        {
            // Write times are the reverse of the run dates...
            var now = DateTime.Now;
            for (int i = 0; i < 4; i++)
                CopyHistory(historyRunPaths[i], $"run{i + 1}.ndjson", now.AddMinutes(-i));

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(currentRunPath, "Title");
            livingDocConverter.MergeHistory(livingDocProject, Path.Combine(tempDirectory, "run.ndjson"));

            var dates = livingDocProject.History.Scenarios.Select(h => h.Date).ToList();
            Assert.That(dates.Count, Is.EqualTo(4));
            Assert.That(dates, Is.Ordered.Ascending);

            var example = livingDocProject.Features[0].Scenarios[0].Examples[0];
            Assert.That(example.History.Select(h => h.Date), Is.Ordered.Ascending);
        }

        [Test]
        public void LivingDocConverter_MergeHistory_SearchesSubfolders()
        {
            var now = DateTime.Now;
            CopyHistory(historyRunPaths[0], "run1.ndjson", now.AddMinutes(-2));
            CopyHistory(historyRunPaths[1], Path.Combine("Nightly", "run2.ndjson"), now.AddMinutes(-1));

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(currentRunPath, "Title");
            livingDocConverter.MergeHistory(livingDocProject, Path.Combine(tempDirectory, "run.ndjson"));

            Assert.That(livingDocProject.History.Scenarios.Count, Is.EqualTo(2));
        }

        [Test]
        public void LivingDocConverter_MergeHistory_IgnoresFilesNotMatchingPattern()
        {
            var now = DateTime.Now;
            CopyHistory(historyRunPaths[0], "run1.ndjson", now.AddMinutes(-2));
            CopyHistory(historyRunPaths[1], "other1.ndjson", now.AddMinutes(-1));
            CopyHistory(historyRunPaths[2], "run2.json", now);

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(currentRunPath, "Title");
            livingDocConverter.MergeHistory(livingDocProject, Path.Combine(tempDirectory, "run.ndjson"));

            Assert.That(livingDocProject.History.Scenarios.Count, Is.EqualTo(1));
        }

        [Test]
        public void LivingDocConverter_MergeHistory_DuplicateRunFiles_AreMergedOnce()
        {
            var now = DateTime.Now;
            CopyHistory(historyRunPaths[0], "run1.ndjson", now.AddMinutes(-2));
            CopyHistory(historyRunPaths[0], "run1-copy.ndjson", now.AddMinutes(-1));

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(currentRunPath, "Title");
            livingDocConverter.MergeHistory(livingDocProject, Path.Combine(tempDirectory, "run.ndjson"));

            Assert.That(livingDocProject.History.Scenarios.Count, Is.EqualTo(1));
            Assert.That(livingDocProject.HasHistory(), Is.False);
        }

        [Test]
        public void LivingDocConverter_MergeHistory_IncludingCurrentRun_EnablesHealth()
        {
            // The --history CLI mode copies the current run into the history folder before merging...
            var now = DateTime.Now;
            for (int i = 1; i < historyRunPaths.Length; i++)
                CopyHistory(historyRunPaths[i], $"run{i + 1}.ndjson", now.AddMinutes(i - 10));

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Convert(currentRunPath, "Title");
            livingDocConverter.MergeHistory(livingDocProject, Path.Combine(tempDirectory, "run.ndjson"));

            Assert.That(livingDocProject.History.Scenarios.Last().Date, Is.EqualTo(livingDocProject.Date));
            Assert.That(livingDocProject.HasHistory(), Is.True);
            Assert.That(livingDocProject.Features.SelectMany(f => f.Scenarios).All(s => s.Examples.All(e => e.History.Count == 4)), Is.True);
        }
    }
}
