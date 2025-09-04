using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Expressium.LivingDoc.UITests.Controls
{
    public static class WebControls
    {
        public static void Click(this By locator, IWebDriver driver)
        {
            var element = locator.GetEnabledElement(driver);
            element.HighlightAction(driver);
            element.HighlightClear(driver);
            element.Click();
        }

        public static void Click(this IWebElement element, IWebDriver driver)
        {
            element.WaitForElementIsEnabled(driver);
            element.HighlightAction(driver);
            element.HighlightClear(driver);
            element.Click();
        }

        public static void ClickLink(this By locator, IWebDriver driver)
        {
            locator.Click(driver);
        }

        public static void ClickLink(this IWebElement element, IWebDriver driver)
        {
            element.Click(driver);
        }

        public static void ClickButton(this By locator, IWebDriver driver)
        {
            locator.Click(driver);
        }

        public static void ClickButton(this IWebElement element, IWebDriver driver)
        {
            element.Click(driver);
        }

        public static void SetTextBox(this By locator, IWebDriver driver, string value)
        {
            if (value == null)
                return;

            var element = locator.GetEnabledElement(driver);
            element.HighlightAction(driver);
            element.Clear();
            element.SendKeys(value);
            element.HighlightClear(driver);
        }

        public static void SetTextBox(this IWebElement element, IWebDriver driver, string value)
        {
            if (value == null)
                return;

            element.WaitForElementIsEnabled(driver);
            element.HighlightAction(driver);
            element.Clear();
            element.SendKeys(value);
            element.HighlightClear(driver);
        }

        public static string GetTextBox(this By locator, IWebDriver driver)
        {
            var element = locator.GetVisibleElement(driver);
            element.HighlightValidation(driver);

            return element.GetDomProperty("value").Trim();
        }

        public static string GetTextBox(this IWebElement element, IWebDriver driver)
        {
            element.WaitForElementIsVisible(driver);
            element.HighlightValidation(driver);

            return element.GetDomProperty("value").Trim();
        }

        public static void SetCheckBox(this By locator, IWebDriver driver, bool value)
        {
            var element = locator.GetEnabledElement(driver);
            if (!element.Selected && value || element.Selected && !value)
            {
                element.HighlightAction(driver);
                element.Click();
                element.HighlightClear(driver);
            }
        }

        public static void SetCheckBox(this IWebElement element, IWebDriver driver, bool value)
        {
            element.WaitForElementIsEnabled(driver);
            if (!element.Selected && value || element.Selected && !value)
            {
                element.HighlightAction(driver);
                element.Click();
                element.HighlightClear(driver);
            }
        }

        public static bool GetCheckBox(this By locator, IWebDriver driver)
        {
            var element = locator.GetVisibleElement(driver);
            element.HighlightValidation(driver);

            return element.Selected;
        }

        public static bool GetCheckBox(this IWebElement element, IWebDriver driver)
        {
            element.WaitForElementIsVisible(driver);
            element.HighlightValidation(driver);

            return element.Selected;
        }

        public static void SetRadioButton(this By locator, IWebDriver driver, bool value)
        {
            var element = locator.GetEnabledElement(driver);
            if (!element.Selected && value)
            {
                element.HighlightAction(driver);
                element.Click();
                element.HighlightClear(driver);
            }
        }

        public static void SetRadioButton(this IWebElement element, IWebDriver driver, bool value)
        {
            element.WaitForElementIsEnabled(driver);
            if (!element.Selected && value)
            {
                element.HighlightAction(driver);
                element.Click();
                element.HighlightClear(driver);
            }
        }

        public static bool GetRadioButton(this By locator, IWebDriver driver)
        {
            var element = locator.GetVisibleElement(driver);
            element.HighlightValidation(driver);

            return element.Selected;
        }

        public static bool GetRadioButton(this IWebElement element, IWebDriver driver)
        {
            element.WaitForElementIsVisible(driver);
            element.HighlightValidation(driver);

            return element.Selected;
        }

        public static void SetComboBox(this By locator, IWebDriver driver, string value)
        {
            locator.SetComboBoxByText(driver, value);
        }

        public static void SetComboBox(this IWebElement element, IWebDriver driver, string value)
        {
            element.SetComboBoxByText(driver, value);
        }

        public static void SetComboBoxByText(this By locator, IWebDriver driver, string value)
        {
            if (value == null)
                return;

            var element = locator.GetEnabledElement(driver);
            element.HighlightAction(driver);

            var selectElement = new SelectElement(element);
            selectElement.WaitForSelectOptionIsPopulated(driver, value);
            selectElement.SelectByText(value);

            element.HighlightClear(driver);
        }

        public static void SetComboBoxByText(this IWebElement element, IWebDriver driver, string value)
        {
            if (value == null)
                return;

            element.WaitForElementIsEnabled(driver);
            element.HighlightAction(driver);

            var selectElement = new SelectElement(element);
            selectElement.WaitForSelectOptionIsPopulated(driver, value);
            selectElement.SelectByText(value);

            element.HighlightClear(driver);
        }

        public static void SetComboBoxByIndex(this By locator, IWebDriver driver, int value)
        {
            var element = locator.GetEnabledElement(driver);
            element.HighlightAction(driver);

            var selectElement = new SelectElement(element);
            selectElement.SelectByIndex(value);

            element.HighlightClear(driver);
        }

        public static void SetComboBoxByIndex(this IWebElement element, IWebDriver driver, int value)
        {
            element.WaitForElementIsEnabled(driver);
            element.HighlightAction(driver);

            var selectElement = new SelectElement(element);
            selectElement.SelectByIndex(value);

            element.HighlightClear(driver);
        }

        public static void SetComboBoxByValue(this By locator, IWebDriver driver, string value)
        {
            if (value == null)
                return;

            var element = locator.GetEnabledElement(driver);
            element.HighlightAction(driver);

            var selectElement = new SelectElement(element);
            selectElement.SelectByValue(value);

            element.HighlightClear(driver);
        }

        public static void SetComboBoxByValue(this IWebElement element, IWebDriver driver, string value)
        {
            if (value == null)
                return;

            element.WaitForElementIsEnabled(driver);
            element.HighlightAction(driver);

            var selectElement = new SelectElement(element);
            selectElement.SelectByValue(value);

            element.HighlightClear(driver);
        }

        public static string GetComboBox(this By locator, IWebDriver driver)
        {
            var element = locator.GetVisibleElement(driver);
            element.HighlightValidation(driver);

            var selectElement = new SelectElement(element);
            if (selectElement.SelectedOption != null)
                return selectElement.SelectedOption.Text.Trim();

            return null;
        }

        public static string GetComboBox(this IWebElement element, IWebDriver driver)
        {
            element.WaitForElementIsVisible(driver);
            element.HighlightValidation(driver);

            var selectElement = new SelectElement(element);
            if (selectElement.SelectedOption != null)
                return selectElement.SelectedOption.Text.Trim();

            return null;
        }

        public static void SetListBox(this By locator, IWebDriver driver, string value)
        {
            locator.SetComboBox(driver, value);
        }

        public static void SetListBox(this IWebElement element, IWebDriver driver, string value)
        {
            element.SetComboBoxByText(driver, value);
        }

        public static string GetListBox(this By locator, IWebDriver driver)
        {
            return locator.GetComboBox(driver);
        }

        public static string GetListBox(this IWebElement element, IWebDriver driver)
        {
            return element.GetComboBox(driver);
        }

        public static string GetText(this By locator, IWebDriver driver)
        {
            var element = locator.GetVisibleElement(driver);
            element.HighlightValidation(driver);

            return element.Text.Trim();
        }

        public static string GetText(this IWebElement element, IWebDriver driver)
        {
            element.WaitForElementIsVisible(driver);
            element.HighlightValidation(driver);

            return element.Text.Trim();
        }
    }
}
