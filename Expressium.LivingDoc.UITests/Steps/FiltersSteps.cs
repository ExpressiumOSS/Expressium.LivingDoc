using Expressium.LivingDoc.UITests.Pages;
using log4net.Filter;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDoc.UITests.Steps
{
    [Binding]
    public class FiltersSteps : BaseSteps
    {
        public FiltersSteps(BaseContext baseContext) : base(baseContext)
        {
        }

        [When("I clear all filters in the Filter Bar")]
        public void GivenIHaveClearedAllFiltersInTheFilterBar()
        {
            var filterBar = new FilterBar(logger, driver);
            filterBar.ClickClear();
        }

        [When("I filter by following keywords in the Filter Bar")]
        public void WhenIFilterByFollowingKeywordsInTheFilterBar(DataTable dataTable)
        {
            var filters = dataTable.CreateSet<Filters>();

            var filterBar = new FilterBar(logger, driver);
            foreach (var filter in filters)
                filterBar.SetFilterByKeywords(filter.Keywords);
        }

        [When("I enable all status prefilters in the Filter Bar")]
        public void WhenIEnableAllStatusPrefiltersInTheFilterBar()
        {
            var filterBar = new FilterBar(logger, driver);
            filterBar.ClickPassed();
            filterBar.ClickIncomplete();
            filterBar.ClickFailed();
            filterBar.ClickSkipped();
        }

        [When("I enable the status prefilter Passed in the Filter Bar")]
        public void WhenIEnableTheStatusPreFilterPassedInTheFilterBar()
        {
            var filterBar = new FilterBar(logger, driver);
            filterBar.ClickPassed();
        }

        [When("I enable the status prefilter Incomplete in the Filter Bar")]
        public void WhenIEnableTheStatusPreFilterIncompleteInTheFilterBar()
        {
            var filterBar = new FilterBar(logger, driver);
            filterBar.ClickIncomplete();
        }

        [When("I enable the status prefilter Failed in the Filter Bar")]
        public void WhenIEnableTheStatusPreFilterFailedInTheFilterBar()
        {
            var filterBar = new FilterBar(logger, driver);
            filterBar.ClickFailed();
        }

        [When("I enable the status prefilter Skipped in the Filter Bar")]
        public void WhenIEnableTheStatusPreFilterSkippedInTheFilterBar()
        {
            var filterBar = new FilterBar(logger, driver);
            filterBar.ClickSkipped();
        }

        [Then("I should have a predefined keyword filter in the Filter Bar")]
        public void ThenIShouldHaveAPredefinedKeywordFilterInTheFilterBar()
        {
            var filterBar = new FilterBar(logger, driver);
            Asserts.IsFalse(string.IsNullOrEmpty(filterBar.GetFilterByKeywords()), "Validating the Keyword filter is predefined...");
        }

        [Then("I should have an emtpy keyword filter in the Filter Bar")]
        public void ThenIShouldHaveAnEmtpyKeywordFilterInTheFilterBar()
        {
            var filterBar = new FilterBar(logger, driver);
            Asserts.IsTrue(string.IsNullOrEmpty(filterBar.GetFilterByKeywords()), "Validating the Keyword filter is empty...");
        }

        [Then("I should have all status prefilters enabled in the Filter Bar")]
        public void ThenIShouldHaveAllStatusPrefiltersEnabledInTheFilterBar()
        {
            var filterBar = new FilterBar(logger, driver);

            Asserts.IsTrue(filterBar.IsPassedActive(), "Validating the Passed filter is enabled...");
            Asserts.IsTrue(filterBar.IsIncompleteActive(), "Validating the Incomplete filter is enabled...");
            Asserts.IsTrue(filterBar.IsFailedActive(), "Validating the Failed filter is enabled...");
            Asserts.IsTrue(filterBar.IsSkippedActive(), "Validating the Skipped filter is enabled...");
        }

        [Then("I should have all status prefilters disabled in the Filter Bar")]
        public void ThenIShouldHaveAllStatusPrefiltersDisabledInTheFilterBar()
        {
            var filterBar = new FilterBar(logger, driver);

            Asserts.IsFalse(filterBar.IsPassedActive(), "Validating the Passed filter is disabled...");
            Asserts.IsFalse(filterBar.IsIncompleteActive(), "Validating the Incomplete filter is disabled...");
            Asserts.IsFalse(filterBar.IsFailedActive(), "Validating the Failed filter is disabled...");
            Asserts.IsFalse(filterBar.IsSkippedActive(), "Validating the Skipped filter is disabled...");
        }

        private class Filters
        {
            public string Keywords { get; set; }
        }
    }
}
