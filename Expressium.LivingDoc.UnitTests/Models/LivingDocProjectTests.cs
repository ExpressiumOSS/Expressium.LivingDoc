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

            var livingDocProject = LivingDocSerializer.DeserializeAsJson<LivingDocProject>(inputFilePath);

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
    }
}
