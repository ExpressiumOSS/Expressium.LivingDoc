using Expressium.LivingDoc.Models;
using Expressium.LivingDoc.Parsers;
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
            Assert.That(livingDocProjectMaster.Histories.Count, Is.EqualTo(1));
            Assert.That(livingDocProjectMaster.Histories[0].Date, Is.EqualTo(livingDocProjectSlave.Date));
            Assert.That(livingDocProjectMaster.Histories[0].Features.Passed, Does.Contain("PassedFeature"));
            Assert.That(livingDocProjectMaster.Histories[0].Features.Failed, Does.Contain("FailedFeature"));
            Assert.That(livingDocProjectMaster.Histories[0].Features.Incomplete, Does.Contain("IncompleteFeature"));
            Assert.That(livingDocProjectMaster.Histories[0].Features.Skipped, Does.Contain("SkippedFeature"));

            // Calling MergeHistory again should omit duplicates...
            livingDocProjectMaster.MergeHistory(livingDocProjectSlave);
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

            Assert.That(livingDocProject.Histories.Count, Is.EqualTo(2));
            Assert.That(livingDocProject.GetMaximumNumberOfHistoryFeatures(), Is.EqualTo(5));
            Assert.That(livingDocProject.GetMaximumNumberOfHistoryScenarios(), Is.EqualTo(4));
            Assert.That(livingDocProject.GetMaximumNumberOfHistorySteps(), Is.EqualTo(7));
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

            var historyOne = new LivingDocHistory();
            historyOne.Date = DateTime.UtcNow.AddDays(-2);
            historyOne.Features.Failed.Add("Checkout");
            historyOne.Features.Passed.Add("Login");

            var historyTwo = new LivingDocHistory();
            historyTwo.Date = DateTime.UtcNow.AddDays(-1);
            historyTwo.Features.Failed.Add("Checkout");
            historyTwo.Features.Passed.Add("Login");

            livingDocProject.Histories.Add(historyOne);
            livingDocProject.Histories.Add(historyTwo);

            var failures = livingDocProject.GetHistoryFeatureFailures();

            Assert.That(failures, Does.Contain("Checkout"));
            Assert.That(failures, Does.Not.Contain("Login"));
        }

        [Test]
        public void LivingDocProject_GetHistoryFeatureFailures_Ordered()
        {
            var livingDocProject = new LivingDocProject();

            var historyOne = new LivingDocHistory();
            historyOne.Date = DateTime.UtcNow.AddDays(-3);
            historyOne.Features.Failed.Add("FeatureA");

            var historyTwo = new LivingDocHistory();
            historyTwo.Date = DateTime.UtcNow.AddDays(-2);
            historyTwo.Features.Failed.Add("FeatureA");
            historyTwo.Features.Failed.Add("FeatureB");

            var historyThree = new LivingDocHistory();
            historyThree.Date = DateTime.UtcNow.AddDays(-1);
            historyThree.Features.Failed.Add("FeatureA");
            historyThree.Features.Failed.Add("FeatureB");

            livingDocProject.Histories.Add(historyOne);
            livingDocProject.Histories.Add(historyTwo);
            livingDocProject.Histories.Add(historyThree);

            var failures = livingDocProject.GetHistoryFeatureFailures();

            Assert.That(failures.Count, Is.EqualTo(2));
            Assert.That(failures[0], Is.EqualTo("FeatureA"));
            Assert.That(failures[1], Is.EqualTo("FeatureB"));
        }

        [Test]
        public void LivingDocProject_GetHistoryScenarioFailures_Failed_Above_Threshold()
        {
            var livingDocProject = new LivingDocProject();

            var historyOne = new LivingDocHistory();
            historyOne.Date = DateTime.UtcNow.AddDays(-2);
            historyOne.Scenarios.Failed.Add("Add Item To Basket");
            historyOne.Scenarios.Passed.Add("View Homepage");

            var historyTwo = new LivingDocHistory();
            historyTwo.Date = DateTime.UtcNow.AddDays(-1);
            historyTwo.Scenarios.Failed.Add("Add Item To Basket");
            historyTwo.Scenarios.Passed.Add("View Homepage");

            livingDocProject.Histories.Add(historyOne);
            livingDocProject.Histories.Add(historyTwo);

            var failures = livingDocProject.GetHistoryScenarioFailures();

            Assert.That(failures, Does.Contain("Add Item To Basket"));
            Assert.That(failures, Does.Not.Contain("View Homepage"));
        }

        [Test]
        public void LivingDocProject_GetHistoryStepFailures_Failed_Above_Threshold()
        {
            var livingDocProject = new LivingDocProject();

            var historyOne = new LivingDocHistory();
            historyOne.Date = DateTime.UtcNow.AddDays(-2);
            historyOne.Steps.Failed.Add("When I click the submit button");
            historyOne.Steps.Passed.Add("Given I am on the homepage");

            var historyTwo = new LivingDocHistory();
            historyTwo.Date = DateTime.UtcNow.AddDays(-1);
            historyTwo.Steps.Failed.Add("When I click the submit button");
            historyTwo.Steps.Passed.Add("Given I am on the homepage");

            livingDocProject.Histories.Add(historyOne);
            livingDocProject.Histories.Add(historyTwo);

            var failures = livingDocProject.GetHistoryStepFailures();

            Assert.That(failures, Does.Contain("When I click the submit button"));
            Assert.That(failures, Does.Not.Contain("Given I am on the homepage"));
        }

        [Test]
        public void LivingDocProject_GetHistoryFailures_With_No_Faliures()
        {
            var livingDocProject = new LivingDocProject();

            var history = new LivingDocHistory();
            history.Date = DateTime.UtcNow.AddDays(-1);

            history.Features.Skipped.Add("SlowFeature");
            history.Features.Passed.Add("StableFeature");
            history.Features.Incomplete.Add("IncompleteFeature");

            history.Scenarios.Skipped.Add("Skipped Scenario");
            history.Scenarios.Passed.Add("Stable Scenario");
            history.Scenarios.Incomplete.Add("Incomplete Scenario");

            history.Steps.Skipped.Add("Given everything is skipped");
            history.Steps.Passed.Add("Given everything is fine");
            history.Steps.Incomplete.Add("Given everything is incomplete");

            livingDocProject.Histories.Add(history);

            Assert.That(livingDocProject.GetHistoryFeatureFailures(), Is.Empty);
            Assert.That(livingDocProject.GetHistoryScenarioFailures(), Is.Empty);
            Assert.That(livingDocProject.GetHistoryStepFailures(), Is.Empty);
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
    }
}
