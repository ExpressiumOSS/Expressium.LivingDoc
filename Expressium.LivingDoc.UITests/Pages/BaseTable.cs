using Expressium.LivingDoc.UITests.Controls;
using log4net;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.LivingDoc.UITests.Pages
{
    public class BaseTable : BasePage
    {
        protected readonly By baseLocator;

        public BaseTable(ILog logger, IWebDriver driver, By baseLocator) : base(logger, driver)
        {
            this.baseLocator = baseLocator;
        }

        public virtual int GetNumberOfRows()
        {
            logger.Info("GetNumberOfRows()");
            return baseLocator.GetChildElements(driver, By.XPath("./tbody/tr")).Count;
        }

        public virtual int GetNumberOfColumns()
        {
            logger.Info("GetNumberOfColumns()");
            return baseLocator.GetChildElements(driver, By.XPath("./thead/tr/th")).Count;
        }

        public virtual void ClickCell(object rowId, object columnId)
        {
            logger.Info($"ClickCell({rowId}, {columnId})");

            var element = GetCellElement(rowId, columnId, "//*");
            element.Click(driver);
        }

        public virtual void ClickCellByDataRole(string dataRole)
        {
            logger.Info($"ClickCellByDataRole({dataRole})");

            var element = baseLocator.GetChildElement(driver, By.XPath($"./tbody/tr[@data-role='{dataRole}']"));
            element.Click(driver);
        }

        public virtual void SetCellTextBox(object rowId, object columnId, string value)
        {
            logger.Info($"SetCellTextBox({rowId}, {columnId})");

            var element = GetCellElement(rowId, columnId, "//input");
            element.SetTextBox(driver, value);
        }

        public virtual string GetCellTextBox(object rowId, object columnId)
        {
            logger.Info($"GetCellTextBox({rowId}, {columnId})");

            var element = GetCellElement(rowId, columnId, "//input");
            return element.GetTextBox(driver);
        }

        public virtual void SetCellCheckBox(object rowId, object columnId, bool value)
        {
            logger.Info($"SetCellCheckBox({rowId}, {columnId})");

            var element = GetCellElement(rowId, columnId, "//input");
            element.SetCheckBox(driver, value);
        }

        public virtual bool GetCellCheckBox(object rowId, object columnId)
        {
            logger.Info($"GetCellCheckBox({rowId}, {columnId})");

            var element = GetCellElement(rowId, columnId, "//input");
            return element.GetCheckBox(driver);
        }

        public virtual string GetCellText(object rowId, object columnId)
        {
            logger.Info($"GetCellText({rowId}, {columnId})");

            var element = GetCellElement(rowId, columnId, "");
            return element.GetText(driver);
        }

        public virtual void ClickContextMenu(object rowId, object columnId, string menuEntry)
        {
            logger.Info($"ClickContextMenu({rowId}, {columnId})");

            var element = GetCellElement(rowId, columnId, "");
            element.Click(driver);

            var contextMenuLocator = By.XPath($"//li[@role='menuitem']//a[normalize-space()='{menuEntry}']");
            contextMenuLocator.Click(driver);
        }

        protected virtual IWebElement GetCellElement(object rowId, object columnId, string subXPath)
        {
            if (rowId is int rowIndex && columnId is int columnIndex)
            {
                return baseLocator.GetChildElement(driver, By.XPath($"./tbody/tr[{rowIndex}]/td[{columnIndex}]{subXPath}"));
            }
            else if (rowId is int rowIndexInt && columnId is string columnNameStr)
            {
                return GetCellElement(rowIndexInt, GetColumnIndex(columnNameStr), subXPath);
            }
            else if (rowId is string rowTextStr && columnId is int columnIndexInt)
            {
                return baseLocator.GetChildElement(driver, By.XPath($"./tbody/tr[td[contains(normalize-space(),'{rowTextStr}')]]/td[{columnIndexInt}]{subXPath}"));
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

        public virtual int GetColumnIndex(string columnName)
        {
            var columnNames = baseLocator.GetChildElements(driver, By.XPath("./thead/tr/th")).ToList();

            int index = columnNames.FindIndex((element) => element.Text == columnName);

            if (index == -1)
                throw new ApplicationException($"The column name '{columnName}' was not found...");

            return index + 1;
        }

        public virtual int GetNumberOfFolders()
        {
            logger.Info("GetNumberOfFolders()");
            return GetFolders().Count;
        }

        public virtual List<string> GetFolderDataNames()
        {
            logger.Info("GetFolderDataNames()");
            return GetFolders().Select(e => e.GetAttribute("data-name")).Where(name => name != null).ToList();
        }

        private List<IWebElement> GetFolders()
        {
            return baseLocator.GetChildElements(driver, By.CssSelector("tr[data-role='folder']")).Where(row => row.Displayed).ToList();
        }

        public virtual int GetNumberOfFeatures()
        {
            logger.Info("GetNumberOfFeatures()");
            return GetFeatures().Count;
        }

        public virtual List<string> GetFeatureDataNames()
        {
            logger.Info("GetFeatureDataNames()");

            var listOfNames = new List<string>();

            foreach (var feature in GetFeatures())
            {
                var element = feature.FindElement(By.XPath(".//td[span[contains(@class,'status-dot')]]"));
                if (element != null)
                    listOfNames.Add(element.GetText(driver));
            }

            return listOfNames;
        }

        public virtual List<string> GetFeatureNames()
        {
            logger.Info("GetFeatureNames()");

            var listOfNames = new List<string>();

            foreach (var feature in GetFeatures())
            {
                var element = feature.FindElement(By.XPath(".//td[2]"));
                if (element != null)
                    listOfNames.Add(element.GetText(driver));
            }

            return listOfNames;
        }

        private List<IWebElement> GetFeatures()
        {
            return baseLocator.GetChildElements(driver, By.CssSelector("tr[data-role='feature']")).Where(row => row.Displayed).ToList();
        }

        public virtual int GetNumberOfScenarios()
        {
            logger.Info("GetNumberOfScenarios()");
            return GetScenarios().Count;
        }

        public virtual List<string> GetScenarioDataNames()
        {
            logger.Info("GetScenarioDataNames()");

            var listOfNames = new List<string>();

            foreach (var scenario in GetScenarios())
            {
                var element = scenario.FindElement(By.XPath(".//td[span[contains(@class,'status-dot')]]"));
                if (element != null)
                    listOfNames.Add(element.GetText(driver));
            }

            return listOfNames;
        }

        public virtual List<string> GetScenarioNames()
        {
            logger.Info("GetScenarioNames()");

            var listOfNames = new List<string>();

            foreach (var scenario in GetScenarios())
            {
                var element = scenario.FindElement(By.XPath(".//td[2]"));
                if (element != null)
                    listOfNames.Add(element.GetText(driver));
            }

            return listOfNames;
        }

        private List<IWebElement> GetScenarios()
        {
            return baseLocator.GetChildElements(driver, By.CssSelector("tr[data-role='scenario']")).Where(row => row.Displayed).ToList();
        }

        public virtual int GetNumberOfSteps()
        {
            logger.Info("GetNumberOfSteps()");
            return GetSteps().Count;
        }

        public virtual List<string> GetStepDataNames()
        {
            logger.Info("GetStepDataNames()");

            var listOfNames = new List<string>();

            foreach (var step in GetSteps())
            {
                var element = step.FindElement(By.XPath(".//td[span[contains(@class,'status-dot')]]"));
                if (element != null)
                    listOfNames.Add(element.GetText(driver));
            }

            return listOfNames;
        }

        public virtual List<string> GetStepNames()
        {
            logger.Info("GetStepNames()");

            var listOfNames = new List<string>();

            foreach (var step in GetSteps())
            {
                var element = step.FindElement(By.XPath(".//td[2]"));
                if (element != null)
                    listOfNames.Add(element.GetText(driver));
            }

            return listOfNames;
        }

        private List<IWebElement> GetSteps()
        {
            return baseLocator.GetChildElements(driver, By.CssSelector("tr[data-role='step']")).Where(row => row.Displayed).ToList();
        }

        public virtual void ExpandAllFeatures()
        {
            logger.Info("ExpandAllFeatures()");
            var element = baseLocator.GetChildElement(driver, By.XPath(".//a[@title='Expand All Features']"));
            element.Click(driver);
        }

        public virtual void CollapseAllFeatures()
        {
            logger.Info("CollapseAllFeatures()");
            var element = baseLocator.GetChildElement(driver, By.XPath(".//a[@title='Collapse All Features']"));
            element.Click(driver);
        }

        public virtual void ToggleExpandFeatures()
        {
            logger.Info("ToggleExpandFeatures()");

            var features = GetFeatures();
            foreach (var feature in features)
            {
                var element = feature.GetChildElement(driver, By.XPath(".//td[@onclick='loadCollapse(this);']"));
                if (element != null)
                    element.Click(driver);
            }
        }

        public virtual void SortByColumn(string value)
        {
            logger.Info($"SortByColumn({value})");
            var element = baseLocator.GetChildElement(driver, By.XPath($".//th[contains(normalize-space(), '{value}')]"));
            element.Click(driver);
        }
    }
}
