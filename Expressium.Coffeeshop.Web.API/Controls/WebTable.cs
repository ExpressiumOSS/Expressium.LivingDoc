using OpenQA.Selenium;
using System;

namespace Expressium.Coffeeshop.Web.API.Controls
{
    public class WebTable : WebControl
    {
        public WebTable(IWebDriver driver, By locator) : base(driver, locator)
        {
        }

        public WebTable(IWebDriver driver, By locator, By childLocator) : base(driver, locator, childLocator)
        {
        }

        public virtual int GetNumberOfRows()
        {
            return GetElement().GetChildElements(driver, By.XPath("./tbody/tr")).Count;
        }

        public virtual int GetNumberOfColumns()
        {
            return GetElement().GetChildElements(driver, By.XPath("./thead/tr/th")).Count;
        }

        public virtual void ClickCell(object rowId, object columnId)
        {
            var cellLocator = GetCellElement(rowId, columnId, "//*");
            var control = new WebButton(driver, locator, cellLocator);
            control.Click();
        }

        public virtual void SetCellTextBox(object rowId, object columnId, string value)
        {
            var cellLocator = GetCellElement(rowId, columnId, "//input");
            var control = new WebTextBox(driver, locator, cellLocator);
            control.SetText(value);
        }

        public virtual string GetCellTextBox(object rowId, object columnId)
        {
            var cellLocator = GetCellElement(rowId, columnId, "//input");
            var control = new WebTextBox(driver, locator, cellLocator);
            return control.GetText();
        }

        public virtual void SetCellCheckBox(object rowId, object columnId, bool value)
        {
            var cellLocator = GetCellElement(rowId, columnId, "//input");
            var control = new WebCheckBox(driver, locator, cellLocator);
            control.SetChecked(value);
        }

        public virtual bool GetCellCheckBox(object rowId, object columnId)
        {
            var cellLocator = GetCellElement(rowId, columnId, "//input");
            var control = new WebCheckBox(driver, locator, cellLocator);
            return control.GetChecked();
        }

        public virtual void SetCellRadioButton(object rowId, object columnId, bool value)
        {
            var cellLocator = GetCellElement(rowId, columnId, "//input");
            var control = new WebRadioButton(driver, locator, cellLocator);
            control.SetSelected(value);
        }

        public virtual bool GetCellRadioButton(object rowId, object columnId)
        {
            var cellLocator = GetCellElement(rowId, columnId, "//input");
            var control = new WebRadioButton(driver, locator, cellLocator);
            return control.GetSelected();
        }

        public virtual string GetCellText(object rowId, object columnId)
        {
            var cellLocator = GetCellElement(rowId, columnId, null);
            var control = new WebText(driver, locator, cellLocator);
            return control.GetText();
        }

        public virtual string GetCellText(string cellText)
        {
            var cellLocator = By.XPath($"./tbody/tr/td[contains(normalize-space(),'{cellText}')]");
            var control = new WebText(driver, locator, cellLocator);
            return control.GetText();
        }

        public virtual void ClickContextMenu(object rowId, object columnId, string menuEntry)
        {
            var cellLocator = GetCellElement(rowId, columnId, null);
            var control = new WebControl(driver, locator, cellLocator);
            control.WaitForElementIsEnabled();
            control.HighlightElementAsAction();
            control.HighlightElementClear();
            control.ClickElementContextMenu();

            var contextMenuLocator = By.XPath($"//li//a[@role='menuitem' and normalize-space()='{menuEntry}']");
            var contextMenuControl = new WebButton(driver, contextMenuLocator);
            contextMenuControl.Click();
        }

        protected virtual By GetCellElement(object rowId, object columnId, string subXPath)
        {
            if (rowId is int rowIndex && columnId is int columnIndex)
            {
                return By.XPath($"./tbody/tr[{rowIndex}]/td[{columnIndex}]{subXPath}");
            }
            else if (rowId is int rowIndexInt && columnId is string columnNameStr)
            {
                return GetCellElement(rowIndexInt, GetColumnIndex(columnNameStr), subXPath);
            }
            else if (rowId is string rowTextStr && columnId is int columnIndexInt)
            {
                return By.XPath($"./tbody/tr[td[contains(normalize-space(),'{rowTextStr}')]]/td[{columnIndexInt}]{subXPath}");
            }
            else if (rowId is string rowTextStrAlt && columnId is string columnNameStrAlt)
            {
                return GetCellElement(rowTextStrAlt, GetColumnIndex(columnNameStrAlt), subXPath);
            }
            else
            {
                throw new ArgumentException("Invalid argument types for GetCellElement...");
            }
        }

        protected virtual int GetColumnIndex(string columnName)
        {
            var cellLocator = By.XPath("./thead/tr/th");
            var columnElements = locator.GetChildElements(driver, cellLocator);

            int index = columnElements.FindIndex((element) => element.Text == columnName);

            if (index == -1)
                throw new ApplicationException($"The column name '{columnName}' was not found...");

            return index + 1;
        }
    }
}
