using Expressium.LivingDoc.Messages;
using Expressium.LivingDoc.Models;
using System.Collections.Generic;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Models
{
    internal class LivingDocFeatureTests
    {
        [Test]
        public void LivingDocFeature_GetStatus_Failed()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "stack-traces.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);
            var livingDocFeature = livingDocProject.Features[0];

            Assert.That(livingDocFeature.GetTags(), Is.EqualTo(""));
            Assert.That(livingDocFeature.GetStatus(), Is.EqualTo(LivingDocStatuses.Failed.ToString()));
            Assert.That(livingDocFeature.GetDuration(), Is.EqualTo("4s 000ms"));
        }

        [Test]
        public void LivingDocFeature_GetStatus_Incomplete()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "pending.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);
            var livingDocFeature = livingDocProject.Features[0];

            Assert.That(livingDocFeature.GetTags(), Is.EqualTo(""));
            Assert.That(livingDocFeature.GetStatus(), Is.EqualTo(LivingDocStatuses.Incomplete.ToString()));
            Assert.That(livingDocFeature.GetDuration(), Is.EqualTo("30s 000ms"));
        }

        [Test]
        public void LivingDocFeature_GetStatus_Passed()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "examples-tables.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);
            var livingDocFeature = livingDocProject.Features[0];

            Assert.That(livingDocFeature.GetTags(), Is.EqualTo(""));
            Assert.That(livingDocFeature.GetStatus(), Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(livingDocFeature.GetDuration(), Is.EqualTo("3min 36s"));
        }

        [Test]
        public void LivingDocFeature_GetStatus_Skipped()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "empty.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);
            var livingDocFeature = livingDocProject.Features[0];

            Assert.That(livingDocFeature.GetTags(), Is.EqualTo(""));
            Assert.That(livingDocFeature.GetStatus(), Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
            Assert.That(livingDocFeature.GetDuration(), Is.EqualTo("2s 000ms"));
        }

        [Test]
        public void LivingDocFeature_GetFolders_No_Locators()
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
            var folderDepth = livingDocProject.GetFolderDepth();

            Assert.That(folderDepth, Is.EqualTo(0));
            Assert.That(listOfFolders.Count, Is.EqualTo(1));
            Assert.That(listOfFolders[0], Is.EqualTo(null));
        }

        [Test]
        public void LivingDocFeature_GetFolders_One_Locator()
        {
            var livingDocProject = new LivingDocProject
            {
                Features = new List<LivingDocFeature>
                {
                    new LivingDocFeature { Name = "Login", Uri = "Features/Login/Login.feature" },
                }
            };

            var listOfFolders = livingDocProject.GetFolders();
            var folderDepth = livingDocProject.GetFolderDepth();

            Assert.That(folderDepth, Is.EqualTo(2));
            Assert.That(listOfFolders.Count, Is.EqualTo(3));
            Assert.That(listOfFolders[0], Is.EqualTo("Features"));
            Assert.That(listOfFolders[1], Is.EqualTo("Features\\Login"));
            Assert.That(listOfFolders[2], Is.EqualTo(null));
        }

        [Test]
        public void LivingDocFeature_GetFolders_Two_Locators()
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
            var folderDepth = livingDocProject.GetFolderDepth();

            Assert.That(folderDepth, Is.EqualTo(2));
            Assert.That(listOfFolders.Count, Is.EqualTo(4));
            Assert.That(listOfFolders[0], Is.EqualTo("Features"));
            Assert.That(listOfFolders[1], Is.EqualTo("Features\\Login"));
            Assert.That(listOfFolders[2], Is.EqualTo("Features\\Products"));
            Assert.That(listOfFolders[3], Is.EqualTo(null));
        }

        [Test]
        public void LivingDocFeature_GetFolders_Multiple_Locators()
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
            var folderDepth = livingDocProject.GetFolderDepth();

            Assert.That(folderDepth, Is.EqualTo(3));
            Assert.That(listOfFolders.Count, Is.EqualTo(5));
            Assert.That(listOfFolders[0], Is.EqualTo("Features"));
            Assert.That(listOfFolders[1], Is.EqualTo("Features\\Login\\Exp"));
            Assert.That(listOfFolders[2], Is.EqualTo("Features\\Login\\Rtgs"));
            Assert.That(listOfFolders[3], Is.EqualTo("Features\\Products"));
            Assert.That(listOfFolders[4], Is.EqualTo(null));
        }
    }
}
