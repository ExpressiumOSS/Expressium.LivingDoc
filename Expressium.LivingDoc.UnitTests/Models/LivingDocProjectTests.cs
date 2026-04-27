using Expressium.LivingDoc.Models;
using Expressium.LivingDoc.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Expressium.LivingDoc.UnitTests.Models
{
    internal class LivingDocProjectTests
    {
        [Test]
        public void LivingDocProject_DefaultConstructor_InitializesCollections()
        {
            var livingDocProject = new LivingDocProject();

            Assert.That(livingDocProject.Features, Is.Not.Null);
            Assert.That(livingDocProject.History, Is.Not.Null);
            Assert.That(livingDocProject.Duration, Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public void LivingDocProject_Number_Of_Objects()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "native.json");

            var livingDocConverter = new LivingDocConverter();
            var livingDocProject = livingDocConverter.Import(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(6));
            Assert.That(livingDocProject.GetNumberOfRules(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(17));

            Assert.That(livingDocProject.GetNumberOfFailedFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfIncompleteFeatures(), Is.EqualTo(0));
            Assert.That(livingDocProject.GetNumberOfPassedFeatures(), Is.EqualTo(0));
            Assert.That(livingDocProject.GetNumberOfSkippedFeatures(), Is.EqualTo(0));

            Assert.That(livingDocProject.GetNumberOfFailedScenarios(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfIncompleteScenarios(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfPassedScenarios(), Is.EqualTo(3));
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
        [TestCase(0, 0, 0, 0, 0, "0s 000ms")]
        [TestCase(1, 2, 43, 7, 587, "2h 43min")]
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

            livingDocProjectMaster.MergeHistory(livingDocProjectSlave);
            Assert.That(livingDocProjectMaster.History.Features.Count, Is.EqualTo(1));
            Assert.That(livingDocProjectMaster.History.Features[0].Date, Is.EqualTo(livingDocProjectSlave.Date));
            Assert.That(livingDocProjectMaster.History.Features[0].Passed, Is.EqualTo(1));
            Assert.That(livingDocProjectMaster.History.Features[0].Failed, Is.EqualTo(1));
            Assert.That(livingDocProjectMaster.History.Features[0].Incomplete, Is.EqualTo(1));
            Assert.That(livingDocProjectMaster.History.Features[0].Skipped, Is.EqualTo(1));

            // Calling MergeHistory again should omit duplicates...
            livingDocProjectMaster.MergeHistory(livingDocProjectSlave);
            Assert.That(livingDocProjectMaster.History.Features.Count, Is.EqualTo(1));
        }

        [Test]
        public void LivingDocProject_MergeProjectHistoryResults()
        {
            var masterProject = new LivingDocProject();

            var slaveProject = new LivingDocProject();
            slaveProject.Date = DateTime.UtcNow;

            var feature = new LivingDocFeature { Name = "Login" };
            var scenario = new LivingDocScenario();
            var example = new LivingDocExample();
            example.Steps.Add(new LivingDocStep { Status = LivingDocStatuses.Passed.ToString() });
            scenario.Examples.Add(example);
            feature.Scenarios.Add(scenario);
            slaveProject.Features.Add(feature);

            masterProject.MergeProjectHistoryResults(slaveProject);

            Assert.That(masterProject.History.Features.Count, Is.EqualTo(1));
            Assert.That(masterProject.History.Scenarios.Count, Is.EqualTo(1));
            Assert.That(masterProject.History.Steps.Count, Is.EqualTo(1));

            Assert.That(masterProject.History.Features[0].Date, Is.EqualTo(slaveProject.Date));
            Assert.That(masterProject.History.Features[0].Passed, Is.EqualTo(1));

            Assert.That(masterProject.History.Scenarios[0].Date, Is.EqualTo(slaveProject.Date));
            Assert.That(masterProject.History.Scenarios[0].Passed, Is.EqualTo(1));

            Assert.That(masterProject.History.Steps[0].Date, Is.EqualTo(slaveProject.Date));
            Assert.That(masterProject.History.Steps[0].Passed, Is.EqualTo(1));
        }

        [Test]
        public void LivingDocProject_MergeExampleHistoryResults()
        {
            var masterProject = new LivingDocProject();

            var masterFeature = new LivingDocFeature { Name = "Login" };
            var masterScenario = new LivingDocScenario { Name = "Successful Login" };
            var masterExample = new LivingDocExample();
            masterExample.Steps.Add(new LivingDocStep { Status = LivingDocStatuses.Passed.ToString() });
            masterScenario.Examples.Add(masterExample);
            masterFeature.Scenarios.Add(masterScenario);
            masterProject.Features.Add(masterFeature);

            var slaveProject = new LivingDocProject();
            slaveProject.Date = DateTime.UtcNow;

            var slaveFeature = new LivingDocFeature { Name = "Login" };
            var slaveScenario = new LivingDocScenario { Name = "Successful Login" };
            var slaveExample = new LivingDocExample();
            slaveExample.Steps.Add(new LivingDocStep { Status = LivingDocStatuses.Failed.ToString() });
            slaveScenario.Examples.Add(slaveExample);
            slaveFeature.Scenarios.Add(slaveScenario);
            slaveProject.Features.Add(slaveFeature);

            masterProject.MergeExampleHistoryResults(slaveProject);

            Assert.That(masterExample.History.Count, Is.EqualTo(1));
            Assert.That(masterExample.History[0].Date, Is.EqualTo(slaveProject.Date));
            Assert.That(masterExample.History[0].Status, Is.EqualTo(LivingDocStatuses.Failed.ToString()));
        }

        [Test]
        public void LivingDocProject_MergeExampleHistoryResults_UnmatchedFeature_IsSkipped()
        {
            var masterProject = new LivingDocProject();

            var masterFeature = new LivingDocFeature { Name = "Login" };
            var masterScenario = new LivingDocScenario { Name = "Successful Login" };
            var masterExample = new LivingDocExample();
            masterExample.Steps.Add(new LivingDocStep { Status = LivingDocStatuses.Passed.ToString() });
            masterScenario.Examples.Add(masterExample);
            masterFeature.Scenarios.Add(masterScenario);
            masterProject.Features.Add(masterFeature);

            var slaveProject = new LivingDocProject();
            slaveProject.Date = DateTime.UtcNow;
            slaveProject.Features.Add(new LivingDocFeature { Name = "Products" });

            masterProject.MergeExampleHistoryResults(slaveProject);

            Assert.That(masterExample.History.Count, Is.EqualTo(0));
        }

        [Test]
        public void LivingDocProject_GetMaximumNumberOfHistoryCounts()
        {
            var livingDocProject = new LivingDocProject();

            var historyOne = new LivingDocProjectHistoryResults();
            historyOne.Date = System.DateTime.UtcNow.AddDays(-1);
            historyOne.Passed = 2;
            historyOne.Failed = 1;

            var historyOneScenarios = new LivingDocProjectHistoryResults();
            historyOneScenarios.Date = System.DateTime.UtcNow.AddDays(-1);
            historyOneScenarios.Passed = 3;
            historyOneScenarios.Incomplete = 1;

            var historyOneSteps = new LivingDocProjectHistoryResults();
            historyOneSteps.Date = System.DateTime.UtcNow.AddDays(-1);
            historyOneSteps.Passed = 4;

            var historyTwo = new LivingDocProjectHistoryResults();
            historyTwo.Date = System.DateTime.UtcNow;
            historyTwo.Passed = 5;

            var historyTwoScenarios = new LivingDocProjectHistoryResults();
            historyTwoScenarios.Date = System.DateTime.UtcNow;
            historyTwoScenarios.Passed = 2;

            var historyTwoSteps = new LivingDocProjectHistoryResults();
            historyTwoSteps.Date = System.DateTime.UtcNow;
            historyTwoSteps.Passed = 6;
            historyTwoSteps.Failed = 1;

            livingDocProject.History.Features.Add(historyOne);
            livingDocProject.History.Features.Add(historyTwo);
            livingDocProject.History.Scenarios.Add(historyOneScenarios);
            livingDocProject.History.Scenarios.Add(historyTwoScenarios);
            livingDocProject.History.Steps.Add(historyOneSteps);
            livingDocProject.History.Steps.Add(historyTwoSteps);

            Assert.That(livingDocProject.History.Features.Count, Is.EqualTo(2));
            Assert.That(livingDocProject.History.GetMaximumNumberOfHistoryFeatures(), Is.EqualTo(5));
            Assert.That(livingDocProject.History.GetMaximumNumberOfHistoryScenarios(), Is.EqualTo(4));
            Assert.That(livingDocProject.History.GetMaximumNumberOfHistorySteps(), Is.EqualTo(7));
        }

        [Test]
        public void LivingDocProject_GetNumberOfStatuses()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "coffeeshop.feature.ndjson");

            var messagesParser = new MessagesParser();
            var livingDocProject = messagesParser.ConvertToLivingDoc(inputFilePath);

            Assert.That(livingDocProject.GetNumberOfFeatures(), Is.EqualTo(4));
            Assert.That(livingDocProject.GetNumberOfPassedFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfIncompleteFeatures(), Is.EqualTo(1));
            Assert.That(livingDocProject.GetNumberOfFailedFeatures(), Is.EqualTo(2));
            Assert.That(livingDocProject.GetNumberOfSkippedFeatures(), Is.EqualTo(0));

            Assert.That(livingDocProject.GetNumberOfScenarios(), Is.EqualTo(11));
            Assert.That(livingDocProject.GetNumberOfPassedScenarios(), Is.EqualTo(6));
            Assert.That(livingDocProject.GetNumberOfIncompleteScenarios(), Is.EqualTo(2));
            Assert.That(livingDocProject.GetNumberOfFailedScenarios(), Is.EqualTo(2));
            Assert.That(livingDocProject.GetNumberOfSkippedScenarios(), Is.EqualTo(1));

            Assert.That(livingDocProject.GetNumberOfSteps(), Is.EqualTo(30));
            Assert.That(livingDocProject.GetNumberOfPassedSteps(), Is.EqualTo(21));
            Assert.That(livingDocProject.GetNumberOfIncompleteSteps(), Is.EqualTo(2));
            Assert.That(livingDocProject.GetNumberOfFailedSteps(), Is.EqualTo(2));
            Assert.That(livingDocProject.GetNumberOfSkippedSteps(), Is.EqualTo(5));
        }

        [Test]
        public void LivingDocProject_Merge_Accumulate_Duration()
        {
            var masterProject = new LivingDocProject();
            masterProject.Duration = new TimeSpan(0, 0, 0, 10, 0);
            masterProject.Features.Add(new LivingDocFeature { Name = "Login" });

            var slaveProject = new LivingDocProject();
            slaveProject.Duration = new TimeSpan(0, 0, 0, 5, 500);
            slaveProject.Features.Add(new LivingDocFeature { Name = "Login" });
            slaveProject.Features.Add(new LivingDocFeature { Name = "Products" });

            masterProject.Merge(slaveProject);

            Assert.That(masterProject.GetNumberOfFeatures(), Is.EqualTo(2));
            Assert.That(masterProject.Features[0].Name, Is.EqualTo("Login"));
            Assert.That(masterProject.Features[1].Name, Is.EqualTo("Products"));
            Assert.That(masterProject.Duration, Is.EqualTo(new TimeSpan(0, 0, 0, 15, 500)));
        }

        [Test]
        public void LivingDocProject_Merge_Empty_Project_Accumulate_Duration()
        {
            var masterProject = new LivingDocProject();
            masterProject.Duration = new TimeSpan(0, 0, 0, 7, 0);
            masterProject.Features.Add(new LivingDocFeature { Name = "Login" });

            var slaveProject = new LivingDocProject();
            slaveProject.Duration = new TimeSpan(0, 0, 0, 3, 0);

            masterProject.Merge(slaveProject);

            Assert.That(masterProject.GetNumberOfFeatures(), Is.EqualTo(1));
            Assert.That(masterProject.Duration, Is.EqualTo(new TimeSpan(0, 0, 0, 10, 0)));
        }

        [Test]
        public void LivingDocProject_Merge_DeepClone_Independence()
        {
            var masterProject = new LivingDocProject();

            var slaveProject = new LivingDocProject();
            var slaveFeature = new LivingDocFeature { Name = "Orders" };
            slaveProject.Features.Add(slaveFeature);

            masterProject.Merge(slaveProject);

            slaveFeature.Name = "MUTATED";

            Assert.That(masterProject.Features[0].Name, Is.EqualTo("Orders"));
        }

        [Test]
        public void LivingDocProject_GetHistoryFeatureFailures_Threshold()
        {
            var livingDocProject = new LivingDocProject();

            var historyOne = new LivingDocProjectHistoryResults();
            historyOne.Date = DateTime.UtcNow.AddDays(-2);
            historyOne.Failed = 1;
            historyOne.Passed = 1;

            var historyTwo = new LivingDocProjectHistoryResults();
            historyTwo.Date = DateTime.UtcNow.AddDays(-1);
            historyTwo.Failed = 1;
            historyTwo.Passed = 1;

            livingDocProject.History.Features.Add(historyOne);
            livingDocProject.History.Features.Add(historyTwo);

            Assert.That(livingDocProject.History.Features.Count, Is.EqualTo(2));
            Assert.That(livingDocProject.History.Features.Sum(h => h.Failed), Is.EqualTo(2));
            Assert.That(livingDocProject.History.Features.Sum(h => h.Passed), Is.EqualTo(2));
        }

        [Test]
        public void LivingDocProject_GetHistoryFeatureFailures_Ordered()
        {
            var livingDocProject = new LivingDocProject();

            var historyOne = new LivingDocProjectHistoryResults();
            historyOne.Date = DateTime.UtcNow.AddDays(-3);
            historyOne.Failed = 1;

            var historyTwo = new LivingDocProjectHistoryResults();
            historyTwo.Date = DateTime.UtcNow.AddDays(-2);
            historyTwo.Failed = 2;

            var historyThree = new LivingDocProjectHistoryResults();
            historyThree.Date = DateTime.UtcNow.AddDays(-1);
            historyThree.Failed = 2;

            livingDocProject.History.Features.Add(historyOne);
            livingDocProject.History.Features.Add(historyTwo);
            livingDocProject.History.Features.Add(historyThree);

            Assert.That(livingDocProject.History.Features.Count, Is.EqualTo(3));
            Assert.That(livingDocProject.History.Features[0].Failed, Is.EqualTo(1));
            Assert.That(livingDocProject.History.Features[1].Failed, Is.EqualTo(2));
            Assert.That(livingDocProject.History.Features[2].Failed, Is.EqualTo(2));
        }

        [Test]
        public void LivingDocProject_GetHistoryScenarioFailures_Failed_Above_Threshold()
        {
            var livingDocProject = new LivingDocProject();

            var historyOne = new LivingDocProjectHistoryResults();
            historyOne.Date = DateTime.UtcNow.AddDays(-2);
            historyOne.Failed = 1;
            historyOne.Passed = 1;

            var historyTwo = new LivingDocProjectHistoryResults();
            historyTwo.Date = DateTime.UtcNow.AddDays(-1);
            historyTwo.Failed = 1;
            historyTwo.Passed = 1;

            livingDocProject.History.Scenarios.Add(historyOne);
            livingDocProject.History.Scenarios.Add(historyTwo);

            Assert.That(livingDocProject.History.Scenarios.Count, Is.EqualTo(2));
            Assert.That(livingDocProject.History.Scenarios.Sum(h => h.Failed), Is.EqualTo(2));
            Assert.That(livingDocProject.History.Scenarios.Sum(h => h.Passed), Is.EqualTo(2));
        }

        [Test]
        public void LivingDocProject_GetHistoryStepFailures_Failed_Above_Threshold()
        {
            var livingDocProject = new LivingDocProject();

            var historyOne = new LivingDocProjectHistoryResults();
            historyOne.Date = DateTime.UtcNow.AddDays(-2);
            historyOne.Failed = 1;
            historyOne.Passed = 1;

            var historyTwo = new LivingDocProjectHistoryResults();
            historyTwo.Date = DateTime.UtcNow.AddDays(-1);
            historyTwo.Failed = 1;
            historyTwo.Passed = 1;

            livingDocProject.History.Steps.Add(historyOne);
            livingDocProject.History.Steps.Add(historyTwo);

            Assert.That(livingDocProject.History.Steps.Count, Is.EqualTo(2));
            Assert.That(livingDocProject.History.Steps.Sum(h => h.Failed), Is.EqualTo(2));
            Assert.That(livingDocProject.History.Steps.Sum(h => h.Passed), Is.EqualTo(2));
        }

        [Test]
        public void LivingDocProject_GetHistoryFailures_With_No_Failures()
        {
            var livingDocProject = new LivingDocProject();

            var featuresHistory = new LivingDocProjectHistoryResults();
            featuresHistory.Date = DateTime.UtcNow.AddDays(-1);
            featuresHistory.Skipped = 1;
            featuresHistory.Passed = 1;
            featuresHistory.Incomplete = 1;

            var scenariosHistory = new LivingDocProjectHistoryResults();
            scenariosHistory.Date = DateTime.UtcNow.AddDays(-1);
            scenariosHistory.Skipped = 1;
            scenariosHistory.Passed = 1;
            scenariosHistory.Incomplete = 1;

            var stepsHistory = new LivingDocProjectHistoryResults();
            stepsHistory.Date = DateTime.UtcNow.AddDays(-1);
            stepsHistory.Skipped = 1;
            stepsHistory.Passed = 1;
            stepsHistory.Incomplete = 1;

            livingDocProject.History.Features.Add(featuresHistory);
            livingDocProject.History.Scenarios.Add(scenariosHistory);
            livingDocProject.History.Steps.Add(stepsHistory);

            Assert.That(livingDocProject.History.Features.Sum(h => h.Failed), Is.EqualTo(0));
            Assert.That(livingDocProject.History.Scenarios.Sum(h => h.Failed), Is.EqualTo(0));
            Assert.That(livingDocProject.History.Steps.Sum(h => h.Failed), Is.EqualTo(0));
        }

        [Test]
        public void LivingDocProject_GetApplicationName_ReturnsExpectedValue()
        {
            var livingDocProject = new LivingDocProject();

            Assert.That(livingDocProject.GetApplicationName(), Is.EqualTo("Expressium LivingDoc"));
        }

        [Test]
        public void LivingDocProject_GetApplicationVersion_ReturnsNonNullString()
        {
            var livingDocProject = new LivingDocProject();

            Assert.That(livingDocProject.GetApplicationVersion(), Is.Not.Null);
        }

        // Non-patterns
        [TestCase(null, null, null, "Passed", null)]
        [TestCase(null, null, null, "Incomplete", null)]
        [TestCase(null, null, null, "Failed", null)]
        [TestCase(null, null, null, "Skipped", null)]

        // Fixed patterns
        [TestCase(null, null, "Skipped", "Passed", "Fixed")]
        [TestCase(null, null, "Incomplete", "Passed", "Fixed")]
        [TestCase(null, null, "Failed", "Passed", "Fixed")]
        [TestCase(null, "Failed", "Failed", "Passed", "Fixed")]
        [TestCase("Failed", "Failed", "Failed", "Passed", "Fixed")]
        [TestCase("Failed", "Skipped", "Failed", "Passed", "Fixed")]
        [TestCase("Incomplete", "Skipped", "Failed", "Passed", "Fixed")]

        // Regressed patterns
        [TestCase(null, null, "Passed", "Failed", "Regressed")]
        [TestCase(null, "Passed", "Passed", "Failed", "Regressed")]
        [TestCase("Passed", "Passed", "Passed", "Failed", "Regressed")]
        [TestCase("Passed", "Skipped", "Passed", "Failed", "Regressed")]
        [TestCase("Skipped", "Incomplete", "Passed", "Failed", "Regressed")]

        // New Broken patterns
        [TestCase(null, null, "Skipped", "Failed", "Broken")]
        [TestCase(null, null, "Incomplete", "Failed", "Broken")]
        [TestCase(null, "Skipped", "Skipped", "Failed", "Broken")]
        [TestCase(null, "Incomplete", "Incomplete", "Failed", "Broken")]
        [TestCase("Passed", "Failed", "Incomplete", "Failed", "Flaky")]
        [TestCase("Passed", "Failed", "Skipped", "Failed", "Flaky")]

        // Still broken patterns
        [TestCase(null, null, "Failed", "Failed", "Broken")]
        [TestCase(null, "Failed", "Failed", "Failed", "Broken")]
        [TestCase("Failed", "Failed", "Failed", "Failed", "Broken")]
        [TestCase("Skipped", "Skipped", "Skipped", "Failed", "Broken")]

        // Flaky patterns
        [TestCase(null, "Failed", "Passed", "Failed", "Flaky")]
        [TestCase("Passed", "Failed", "Passed", "Failed", "Flaky")]
        [TestCase("Skipped", "Failed", "Passed", "Failed", "Flaky")]
        [TestCase("Incomplete", "Failed", "Passed", "Failed", "Flaky")]
        [TestCase(null, "Passed", "Failed", "Failed", "Flaky")]
        [TestCase("Passed", "Passed", "Failed", "Failed", "Flaky")]

        // Edge Flaky patterns
        [TestCase("Failed", "Skipped", "Passed", "Failed", "Flaky")]
        [TestCase("Failed", "Incomplete", "Passed", "Failed", "Flaky")]
        [TestCase("Failed", "Passed", "Skipped", "Failed", "Flaky")]
        [TestCase("Failed", "Passed", "Incomplete", "Failed", "Flaky")]

        public void LivingDocProject_MergeScenarioHistoryHealth(string prior, string oldest, string previous, string newest, string health)
        {
            var project = new LivingDocProject();

            var feature = new LivingDocFeature { Name = "Payments" };
            var scenario = new LivingDocScenario { Name = "Scenario One" };
            var example = new LivingDocExample();

            if (!string.IsNullOrEmpty(prior))
                example.History.Add(new LivingDocExampleHistoryResults { Date = DateTime.UtcNow.AddDays(-3), Status = prior });

            if (!string.IsNullOrEmpty(oldest))
                example.History.Add(new LivingDocExampleHistoryResults { Date = DateTime.UtcNow.AddDays(-2), Status = oldest });

            if (!string.IsNullOrEmpty(previous))
                example.History.Add(new LivingDocExampleHistoryResults { Date = DateTime.UtcNow.AddDays(-1), Status = previous });

            if (!string.IsNullOrEmpty(newest))
                example.History.Add(new LivingDocExampleHistoryResults { Date = DateTime.UtcNow, Status = newest });

            scenario.Examples.Add(example);
            feature.Scenarios.Add(scenario);
            project.Features.Add(feature);

            project.MergeScenarioHistoryHealth();

            Assert.That(scenario.Health, Is.EqualTo(health));
        }
    }
}
