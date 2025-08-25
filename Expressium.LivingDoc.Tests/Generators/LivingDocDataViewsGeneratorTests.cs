using Expressium.LivingDoc.Generators;

namespace Expressium.LivingDoc.UnitTests.Generators
{
    public class LivingDocDataOverviewGeneratorTests
    {
        [Test]
        public void LivingDocDataOverviewGenerator_GetFolderDepth_No_Locators()
        {
            var livingDocDataOverviewGenerator = new LivingDocDataOverviewGenerator();
            var folderDepth = livingDocDataOverviewGenerator.GetFolderDepth(null);
            Assert.That(folderDepth, Is.EqualTo(0));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GetFolderDepth_One_Locator()
        {
            var livingDocDataOverviewGenerator = new LivingDocDataOverviewGenerator();
            var folderDepth = livingDocDataOverviewGenerator.GetFolderDepth("Features");
            Assert.That(folderDepth, Is.EqualTo(1));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GetFolderDepth_Two_Locators()
        {
            var livingDocDataOverviewGenerator = new LivingDocDataOverviewGenerator();
            var folderDepth = livingDocDataOverviewGenerator.GetFolderDepth("Features\\Login");
            Assert.That(folderDepth, Is.EqualTo(2));
        }

        [Test]
        public void LivingDocDataOverviewGenerator_GetFolderDepth_Tree_Locators()
        {
            var livingDocDataOverviewGenerator = new LivingDocDataOverviewGenerator();
            var folderDepth = livingDocDataOverviewGenerator.GetFolderDepth("Features\\Login\\Exp");
            Assert.That(folderDepth, Is.EqualTo(3));
        }
    }
}
