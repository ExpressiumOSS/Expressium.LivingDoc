﻿using Expressium.LivingDoc.Messages;
using Expressium.LivingDoc.Models;
using System.IO;

namespace Expressium.LivingDoc.UnitTests.Models
{
    internal class LivingDocExampleTests
    {
        [Test]
        public void LivingDocExample_GetStatus_Failed()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "stack-traces.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);
            var livingDocExample = livingDocProject.Features[0].Scenarios[0].Examples[0];

            Assert.That(livingDocExample.GetStatus(), Is.EqualTo(LivingDocStatuses.Failed.ToString()));
            Assert.That(livingDocExample.GetDuration(), Is.EqualTo("4s 000ms"));
            Assert.That(livingDocExample.HasDataTable(), Is.False);
        }

        [Test]
        public void LivingDocExample_GetStatus_Incomplete()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "pending.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);
            var livingDocExample = livingDocProject.Features[0].Scenarios[0].Examples[0];

            Assert.That(livingDocExample.GetStatus(), Is.EqualTo(LivingDocStatuses.Incomplete.ToString()));
            Assert.That(livingDocExample.GetDuration(), Is.EqualTo("4s 000ms"));
            Assert.That(livingDocExample.HasDataTable(), Is.False);
        }

        [Test]
        public void LivingDocExample_GetStatus_Passed()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "examples-tables.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);
            var livingDocExample = livingDocProject.Features[0].Scenarios[0].Examples[0];

            Assert.That(livingDocExample.GetStatus(), Is.EqualTo(LivingDocStatuses.Passed.ToString()));
            Assert.That(livingDocExample.GetDuration(), Is.EqualTo("8s 000ms"));
            Assert.That(livingDocExample.HasDataTable(), Is.True);
        }

        [Test]
        public void LivingDocExample_GetStatus_Skipped()
        {
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "empty.feature.ndjson");

            var livingDocProject = MessagesConvertor.ConvertToLivingDoc(inputFilePath);
            var livingDocExample = livingDocProject.Features[0].Scenarios[0].Examples[0];

            Assert.That(livingDocExample.GetStatus(), Is.EqualTo(LivingDocStatuses.Skipped.ToString()));
            Assert.That(livingDocExample.GetDuration(), Is.EqualTo("2s 000ms"));
            Assert.That(livingDocExample.HasDataTable(), Is.False);
        }
    }
}
