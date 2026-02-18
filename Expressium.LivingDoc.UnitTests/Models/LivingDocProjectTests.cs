using Expressium.LivingDoc.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Models
{
    internal class LivingDocProjectTests
    {
        [Test]
        public void LivingDocProject_Number_Of_Objects()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native.json");

            var livingDocConverter = new LivingDocNativeConverter();
            var livingDocProject = livingDocConverter.Convert(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(5));
            Assert.That(livingDocProject.GetNumberOfRules(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfExamples(), Is.EqualTo(6));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(17));

            Assert.That(livingDocProject.GetNumberOfFailedFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfIncompleteFeatures(), Is.EqualTo(0));
            Assert.That(livingDocProject.GetNumberOfPassedFeatures(), Is.EqualTo(0));
            Assert.That(livingDocProject.GetNumberOfSkippedFeatures(), Is.EqualTo(0));

            Assert.That(livingDocProject.GetNumberOfFailedScenarios(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfIncompleteScenarios(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfPassedScenarios(), Is.EqualTo(2));
            Assert.That(livingDocProject.GetNumberOfSkippedScenarios(), Is.EqualTo(1));

            Assert.That(livingDocProject.GetNumberOfFailedSteps(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfIncompleteSteps(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfPassedSteps(), Is.EqualTo(12));
            Assert.That(livingDocProject.GetNumberOfSkippedSteps(), Is.EqualTo(3));

            Assert.That(livingDocProject.GetDate(), Is.Not.Null);

            Assert.That(livingDocProject.GetDuration(), Is.EqualTo("57s 203ms"));
        }

        [Test]
        public void LivingDocProject_GetFolders_No_Locators()
        {
            var livingDocProject = new LivingDocProject
            {
                Features = new List<LivingDocFeature>
                {
                    new LivingDocFeature { Name = "Login" },
                    new LivingDocFeature { Name = "Products" }
                }
            };

            var listOfFolders = livingDocProject.GetFolders();

            Assert.That(listOfFolders.Count, Is.EqualTo(1));
            Assert.That(listOfFolders[0], Is.EqualTo(null));
        }

        [Test]
        public void LivingDocProject_GetFolders_One_Locator()
        {
            var livingDocProject = new LivingDocProject
            {
                Features = new List<LivingDocFeature>
                {
                    new LivingDocFeature { Name = "Login", Uri = "Features/Login/Login.feature" },
                }
            };

            var listOfFolders = livingDocProject.GetFolders();

            Assert.That(listOfFolders.Count, Is.EqualTo(3));
            Assert.That(listOfFolders[0], Is.EqualTo("Features"));
            Assert.That(listOfFolders[1], Is.EqualTo("Features\\Login"));
            Assert.That(listOfFolders[2], Is.EqualTo(null));
        }

        [Test]
        public void LivingDocProject_GetFolders_Two_Locators()
        {
            var livingDocProject = new LivingDocProject
            {
                Features = new List<LivingDocFeature>
                {
                    new LivingDocFeature { Name = "Login", Uri = "Features/Login/Login.feature" },
                    new LivingDocFeature { Name = "Products", Uri = "Features/Products/Products.feature" }
                }
            };

            var listOfFolders = livingDocProject.GetFolders();

            Assert.That(listOfFolders.Count, Is.EqualTo(4));
            Assert.That(listOfFolders[0], Is.EqualTo("Features"));
            Assert.That(listOfFolders[1], Is.EqualTo("Features\\Login"));
            Assert.That(listOfFolders[2], Is.EqualTo("Features\\Products"));
            Assert.That(listOfFolders[3], Is.EqualTo(null));
        }

        [Test]
        public void LivingDocProject_GetFolders_Multiple_Locators()
        {
            var livingDocProject = new LivingDocProject
            {
                Features = new List<LivingDocFeature>
                {
                    new LivingDocFeature { Name = "Login EXP", Uri = "Features/Login/Exp/LoginExp.feature" },
                    new LivingDocFeature { Name = "Login RTGS", Uri = "Features/Login/Rtgs/LoginRtgs.feature" },
                    new LivingDocFeature { Name = "Products EXP", Uri = "Features/Products/ProductsExp.feature" },
                    new LivingDocFeature { Name = "Products RTGS", Uri = "Features/Products/ProductsRtgs.feature" },
                    new LivingDocFeature { Name = "Accounts", Uri = "Features/Accounts.feature" },
                    new LivingDocFeature { Name = "Payments", Uri = "Payments.feature" },
                    new LivingDocFeature { Name = "Reports" }
                }
            };

            var listOfFolders = livingDocProject.GetFolders();

            Assert.That(listOfFolders.Count, Is.EqualTo(6));
            Assert.That(listOfFolders[0], Is.EqualTo("Features"));
            Assert.That(listOfFolders[1], Is.EqualTo("Features\\Login"));
            Assert.That(listOfFolders[2], Is.EqualTo("Features\\Login\\Exp"));
            Assert.That(listOfFolders[3], Is.EqualTo("Features\\Login\\Rtgs"));
            Assert.That(listOfFolders[4], Is.EqualTo("Features\\Products"));
            Assert.That(listOfFolders[5], Is.EqualTo(null));
        }

        [TestCase(0, 2, 43, 7, 587, "2h 43min")]
        [TestCase(0, 0, 43, 7, 587, "43min 7s")]
        [TestCase(0, 0, 0, 7, 587, "7s 587ms")]
        [TestCase(0, 0, 0, 0, 587, "0s 587ms")]
        public void LivingDocProject_GetDuration(int days, int hours, int minutes, int seconds, int milliseconds, string result)
        {
            var livingDocProject = new LivingDocProject
            {
                Duration = new TimeSpan(days, hours, minutes, seconds, milliseconds)
            };

            Assert.That(livingDocProject.GetDuration(), Is.EqualTo(result));
        }

        [Test]
        public void LivingDocProject_MergeHistory()
        {
            var livingDocProjectMaster = new LivingDocProject();

            var livingDocProjectSlave = new LivingDocProject();
            livingDocProjectSlave.Date = System.DateTime.UtcNow;

            // Passed feature
            var passedFeature = new LivingDocFeature { Name = "PassedFeature" };
            var passedScenario = new LivingDocScenario();
            var passedExample = new LivingDocExample();
            passedExample.Steps.Add(new LivingDocStep { Status = LivingDocStatuses.Passed.ToString() });
            passedScenario.Examples.Add(passedExample);
            passedFeature.Scenarios.Add(passedScenario);
            livingDocProjectSlave.Features.Add(passedFeature);

            // Failed feature
            var failedFeature = new LivingDocFeature { Name = "FailedFeature" };
            var failedScenario = new LivingDocScenario();
            var failedExample = new LivingDocExample();
            failedExample.Steps.Add(new LivingDocStep { Status = LivingDocStatuses.Failed.ToString() });
            failedScenario.Examples.Add(failedExample);
            failedFeature.Scenarios.Add(failedScenario);
            livingDocProjectSlave.Features.Add(failedFeature);

            // Incomplete feature
            var incompleteFeature = new LivingDocFeature { Name = "IncompleteFeature" };
            var incompleteScenario = new LivingDocScenario();
            var incompleteExample = new LivingDocExample();
            incompleteExample.Steps.Add(new LivingDocStep { Status = LivingDocStatuses.Incomplete.ToString() });
            incompleteScenario.Examples.Add(incompleteExample);
            incompleteFeature.Scenarios.Add(incompleteScenario);
            livingDocProjectSlave.Features.Add(incompleteFeature);

            // Skipped feature
            var skippedFeature = new LivingDocFeature { Name = "SkippedFeature" };
            livingDocProjectSlave.Features.Add(skippedFeature);

            livingDocProjectMaster.MergeHistory(livingDocProjectSlave, "http://history-url");
            Assert.That(livingDocProjectMaster.Histories.Count, Is.EqualTo(1));
            Assert.That(livingDocProjectMaster.Histories[0].Date, Is.EqualTo(livingDocProjectSlave.Date));
            Assert.That(livingDocProjectMaster.Histories[0].Url, Is.EqualTo("http://history-url"));
            Assert.That(livingDocProjectMaster.Histories[0].Features.Passed, Does.Contain("PassedFeature"));
            Assert.That(livingDocProjectMaster.Histories[0].Features.Failed, Does.Contain("FailedFeature"));
            Assert.That(livingDocProjectMaster.Histories[0].Features.Incomplete, Does.Contain("IncompleteFeature"));
            Assert.That(livingDocProjectMaster.Histories[0].Features.Skipped, Does.Contain("SkippedFeature"));

            // Calling MergeHistory again should omit duplicates...
            livingDocProjectMaster.MergeHistory(livingDocProjectSlave, "http://history-url");
            Assert.That(livingDocProjectMaster.Histories.Count, Is.EqualTo(1));
        }

        [Test]
        public void LivingDocProject_GetMaximumNumberOfHistoryCounts()
        {
            var livingDocProject = new LivingDocProject();

            var historyOne = new LivingDocHistory();
            historyOne.Date = System.DateTime.UtcNow.AddDays(-1);
            historyOne.Features.Passed.AddRange(new[] { "F1", "F2" });
            historyOne.Features.Failed.Add("F3");
            historyOne.Scenarios.Passed.AddRange(new[] { "S1", "S2", "S3" });
            historyOne.Scenarios.Incomplete.Add("S4");
            historyOne.Steps.Passed.AddRange(new[] { "Step1", "Step2", "Step3", "Step4" });

            var historyTwo = new LivingDocHistory();
            historyTwo.Date = System.DateTime.UtcNow;
            historyTwo.Features.Passed.AddRange(new[] { "F4", "F5", "F6", "F7", "F8" });
            historyTwo.Scenarios.Passed.AddRange(new[] { "S5", "S6" });
            historyTwo.Steps.Passed.AddRange(new[] { "Step5", "Step6", "Step7", "Step8", "Step9", "Step10" });
            historyTwo.Steps.Failed.Add("Step11");

            livingDocProject.Histories.Add(historyOne);
            livingDocProject.Histories.Add(historyTwo);

            Assert.That(livingDocProject.GetMaximumNumberOfHistoryFeatures(), Is.EqualTo(5));
            Assert.That(livingDocProject.GetMaximumNumberOfHistoryScenarios(), Is.EqualTo(4));
            Assert.That(livingDocProject.GetMaximumNumberOfHistorySteps(), Is.EqualTo(7));
        }
    }
}
