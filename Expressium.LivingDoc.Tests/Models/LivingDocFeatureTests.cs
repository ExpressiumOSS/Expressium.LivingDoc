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
    }
}
