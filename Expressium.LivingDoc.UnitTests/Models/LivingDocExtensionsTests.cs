using Expressium.LivingDoc.Models;
using System;

namespace Expressium.LivingDoc.UnitTests.Models
{
    internal class LivingDocExtensionsTests
    {
        [Test]
        public void LivingDocExtensions_DeepClone_ReturnsNewInstanceWithSameValues()
        {
            var original = new LivingDocFeature
            {
                Name = "Login",
                Description = "Login feature",
                Keyword = "Feature"
            };

            var clone = LivingDocExtensions.DeepClone(original);

            Assert.That(clone, Is.Not.SameAs(original));
            Assert.That(clone.Name, Is.EqualTo(original.Name));
            Assert.That(clone.Description, Is.EqualTo(original.Description));
            Assert.That(clone.Keyword, Is.EqualTo(original.Keyword));
        }

        [Test]
        public void LivingDocExtensions_DeepClone_Original_Independence()
        {
            var original = new LivingDocFeature { Name = "Login" };

            var clone = LivingDocExtensions.DeepClone(original);
            clone.Name = "MUTATED";

            Assert.That(original.Name, Is.EqualTo("Login"));
        }

        [Test]
        public void LivingDocExtensions_DeepClone_Clone_Independence()
        {
            var original = new LivingDocFeature { Name = "Login" };

            var clone = LivingDocExtensions.DeepClone(original);
            original.Name = "MUTATED";

            Assert.That(clone.Name, Is.EqualTo("Login"));
        }

        [Test]
        public void LivingDocExtensions_DeepClone_Nested()
        {
            var original = new LivingDocFeature { Name = "Login" };
            original.Tags.Add("@smoke");
            original.Scenarios.Add(new LivingDocScenario { Name = "Valid Login" });

            var clone = LivingDocExtensions.DeepClone(original);

            Assert.That(clone.Tags, Is.Not.SameAs(original.Tags));
            Assert.That(clone.Tags, Does.Contain("@smoke"));
            Assert.That(clone.Scenarios, Is.Not.SameAs(original.Scenarios));
            Assert.That(clone.Scenarios[0].Name, Is.EqualTo("Valid Login"));
        }

        [Test]
        public void LivingDocExtensions_DeepClone_Nested_Original_Independence()
        {
            var original = new LivingDocFeature { Name = "Login" };
            original.Tags.Add("@smoke");

            var clone = LivingDocExtensions.DeepClone(original);
            clone.Tags.Add("@regression");

            Assert.That(original.Tags.Count, Is.EqualTo(1));
            Assert.That(clone.Tags.Count, Is.EqualTo(2));
        }

        [Test]
        public void LivingDocExtensions_DeepClone_ThrowsArgumentNullExceptionForNullInput()
        {
            LivingDocFeature nullFeature = null;

            Assert.Throws<ArgumentNullException>(() => LivingDocExtensions.DeepClone(nullFeature));
        }
    }
}
