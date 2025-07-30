using Expressium.LivingDoc.Generators;

namespace Expressium.LivingDoc.UnitTests.Generators
{
    public class LivingDocDataViewsGeneratorTests
    {
        [Test]
        public void LivingDocFeature_GetFolderDepth_No_Locators()
        {
            var livingDocDataViewsGenerator = new LivingDocDataViewsGenerator();
            var folderDepth = livingDocDataViewsGenerator.GetFolderDepth(null);
            Assert.That(folderDepth, Is.EqualTo(0));
        }

        [Test]
        public void LivingDocFeature_GetFolderDepth_One_Locator()
        {
            var livingDocDataViewsGenerator = new LivingDocDataViewsGenerator();
            var folderDepth = livingDocDataViewsGenerator.GetFolderDepth("Features");
            Assert.That(folderDepth, Is.EqualTo(1));
        }

        [Test]
        public void LivingDocFeature_GetFolderDepth_Two_Locators()
        {
            var livingDocDataViewsGenerator = new LivingDocDataViewsGenerator();
            var folderDepth = livingDocDataViewsGenerator.GetFolderDepth("Features\\Login");
            Assert.That(folderDepth, Is.EqualTo(2));
        }

        [Test]
        public void LivingDocFeature_GetFolderDepth_Tree_Locators()
        {
            var livingDocDataViewsGenerator = new LivingDocDataViewsGenerator();
            var folderDepth = livingDocDataViewsGenerator.GetFolderDepth("Features\\Login\\Exp");
            Assert.That(folderDepth, Is.EqualTo(3));
        }
    }
}
