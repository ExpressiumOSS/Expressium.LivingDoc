using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;

namespace Expressium.LivingDoc.UnitTests.Generators
{
    internal class LivingDocDataUtilitiesGeneratorTests
    {
        // GetStatusSymbol

        [Test]
        public void LivingDocDataUtilitiesGenerator_GetStatusSymbol_Passed_ReturnsExpectedClass()
        {
            var result = LivingDocDataUtilitiesGenerator.GetStatusSymbol(LivingDocStatuses.Passed.ToString().ToLower());

            Assert.That(result, Is.EqualTo("bi bi-check-circle-fill"));
        }

        [Test]
        public void LivingDocDataUtilitiesGenerator_GetStatusSymbol_Failed_ReturnsExpectedClass()
        {
            var result = LivingDocDataUtilitiesGenerator.GetStatusSymbol(LivingDocStatuses.Failed.ToString().ToLower());

            Assert.That(result, Is.EqualTo("bi bi-x-circle-fill"));
        }

        [Test]
        public void LivingDocDataUtilitiesGenerator_GetStatusSymbol_Incomplete_ReturnsExpectedClass()
        {
            var result = LivingDocDataUtilitiesGenerator.GetStatusSymbol(LivingDocStatuses.Incomplete.ToString().ToLower());

            Assert.That(result, Is.EqualTo("bi bi-exclamation-circle-fill"));
        }

        [Test]
        public void LivingDocDataUtilitiesGenerator_GetStatusSymbol_Skipped_ReturnsExpectedClass()
        {
            var result = LivingDocDataUtilitiesGenerator.GetStatusSymbol(LivingDocStatuses.Skipped.ToString().ToLower());

            Assert.That(result, Is.EqualTo("bi bi-dash-circle-fill"));
        }

        [Test]
        public void LivingDocDataUtilitiesGenerator_GetStatusSymbol_Unknown_ReturnsFallbackClass()
        {
            var result = LivingDocDataUtilitiesGenerator.GetStatusSymbol(LivingDocStatuses.Unknown.ToString().ToLower());

            Assert.That(result, Is.EqualTo("bi bi-question-circle-fill"));
        }

        [Test]
        public void LivingDocDataUtilitiesGenerator_GetStatusSymbol_UnrecognisedValue_ReturnsFallbackClass()
        {
            var result = LivingDocDataUtilitiesGenerator.GetStatusSymbol("something-unexpected");

            Assert.That(result, Is.EqualTo("bi bi-question-circle-fill"));
        }

        [Test]
        public void LivingDocDataUtilitiesGenerator_GetStatusSymbol_EmptyString_ReturnsFallbackClass()
        {
            var result = LivingDocDataUtilitiesGenerator.GetStatusSymbol(string.Empty);

            Assert.That(result, Is.EqualTo("bi bi-question-circle-fill"));
        }

        [Test]
        public void LivingDocDataUtilitiesGenerator_GetHealtSymbol_New_ReturnsExpectedClass()
        {
            var result = LivingDocDataUtilitiesGenerator.GetHealtSymbol(LivingDocHealths.New.ToString());

            Assert.That(result, Is.EqualTo("bi bi-cloud-plus"));
        }

        [Test]
        public void LivingDocDataUtilitiesGenerator_GetHealtSymbol_Broken_ReturnsExpectedClass()
        {
            var result = LivingDocDataUtilitiesGenerator.GetHealtSymbol(LivingDocHealths.Broken.ToString());

            Assert.That(result, Is.EqualTo("bi bi-cloud-rain-heavy"));
        }

        [Test]
        public void LivingDocDataUtilitiesGenerator_GetHealtSymbol_Regressed_ReturnsExpectedClass()
        {
            var result = LivingDocDataUtilitiesGenerator.GetHealtSymbol(LivingDocHealths.Regressed.ToString());

            Assert.That(result, Is.EqualTo("bi bi-cloud-rain"));
        }

        [Test]
        public void LivingDocDataUtilitiesGenerator_GetHealtSymbol_Flaky_ReturnsExpectedClass()
        {
            var result = LivingDocDataUtilitiesGenerator.GetHealtSymbol(LivingDocHealths.Flaky.ToString());

            Assert.That(result, Is.EqualTo("bi bi-clouds"));
        }

        [Test]
        public void LivingDocDataUtilitiesGenerator_GetHealtSymbol_Fixed_ReturnsExpectedClass()
        {
            var result = LivingDocDataUtilitiesGenerator.GetHealtSymbol(LivingDocHealths.Fixed.ToString());

            Assert.That(result, Is.EqualTo("bi bi-cloud-sun"));
        }

        [Test]
        public void LivingDocDataUtilitiesGenerator_GetHealtSymbol_UnrecognisedValue_ReturnsEmptyString()
        {
            var result = LivingDocDataUtilitiesGenerator.GetHealtSymbol("something-unexpected");

            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void LivingDocDataUtilitiesGenerator_GetHealtSymbol_EmptyString_ReturnsEmptyString()
        {
            var result = LivingDocDataUtilitiesGenerator.GetHealtSymbol(string.Empty);

            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void LivingDocDataUtilitiesGenerator_GetHealtSymbol_Null_ReturnsEmptyString()
        {
            var result = LivingDocDataUtilitiesGenerator.GetHealtSymbol(null);

            Assert.That(result, Is.EqualTo(string.Empty));
        }
    }
}
