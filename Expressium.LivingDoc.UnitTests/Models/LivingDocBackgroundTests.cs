using Expressium.LivingDoc.Models;

namespace Expressium.LivingDoc.UnitTests.Models
{
    internal class LivingDocBackgroundTests
    {
        [Test]
        public void LivingDocBackground_Constructor_InitialisesStepsAsEmptyList()
        {
            var background = new LivingDocBackground();

            Assert.That(background.Steps, Is.Not.Null);
            Assert.That(background.Steps, Is.Empty);
        }

        [Test]
        public void LivingDocBackground_Constructor_PropertiesDefaultToNull()
        {
            var background = new LivingDocBackground();

            Assert.That(background.Id, Is.Null);
            Assert.That(background.Name, Is.Null);
            Assert.That(background.Description, Is.Null);
            Assert.That(background.Keyword, Is.Null);
        }

        [Test]
        public void LivingDocBackground_Properties_CanBeAssigned()
        {
            var background = new LivingDocBackground
            {
                Id = "bg-001",
                Name = "Common Setup",
                Description = "Steps shared across all scenarios",
                Keyword = "Background"
            };

            Assert.That(background.Id, Is.EqualTo("bg-001"));
            Assert.That(background.Name, Is.EqualTo("Common Setup"));
            Assert.That(background.Description, Is.EqualTo("Steps shared across all scenarios"));
            Assert.That(background.Keyword, Is.EqualTo("Background"));
        }

        [Test]
        public void LivingDocBackground_Steps_CanAddAndRetrieveSteps()
        {
            var background = new LivingDocBackground();
            background.Steps.Add(new LivingDocStep { Name = "I am logged in" });
            background.Steps.Add(new LivingDocStep { Name = "I am on the homepage" });

            Assert.That(background.Steps.Count, Is.EqualTo(2));
            Assert.That(background.Steps[0].Name, Is.EqualTo("I am logged in"));
            Assert.That(background.Steps[1].Name, Is.EqualTo("I am on the homepage"));
        }
    }
}
