using Expressium.LivingDoc.Generators;

namespace Expressium.LivingDoc.UnitTests.Generators
{
    public class LivingDocDataOverviewGeneratorTests
    {
        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase("\\Root", "Root")]
        [TestCase("\\Root\\", "")]
        [TestCase("\\Root\\SubFolder", "SubFolder")]
        [TestCase("\\Root\\Folder\\SubFolder", "SubFolder")]
        public void LivingDocDataOverviewGenerator_GenerateOverviewHeaderFolder(string value, string expected)
        {
            var result = LivingDocDataOverviewGenerator.GetFolderName(value);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GetFolderDepth_No_Locators()
        {
            var folderDepth = LivingDocDataOverviewGenerator.GetFolderDepth(null);
            Assert.That(folderDepth, Is.EqualTo(0));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GetFolderDepth_One_Locator()
        {
            var folderDepth = LivingDocDataOverviewGenerator.GetFolderDepth("Features");
            Assert.That(folderDepth, Is.EqualTo(1));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GetFolderDepth_Two_Locators()
        {
            var folderDepth = LivingDocDataOverviewGenerator.GetFolderDepth("Features\\Login");
            Assert.That(folderDepth, Is.EqualTo(2));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GetFolderDepth_Tree_Locators()
        {
            var folderDepth = LivingDocDataOverviewGenerator.GetFolderDepth("Features\\Login\\Exp");
            Assert.That(folderDepth, Is.EqualTo(3));
        }
    }
}
