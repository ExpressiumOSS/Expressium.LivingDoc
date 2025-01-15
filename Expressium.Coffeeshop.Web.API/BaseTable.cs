using Expressium.Coffeeshop.Web.API.Controls;
using log4net;
using OpenQA.Selenium;

namespace Expressium.Coffeeshop.Web.API
{
    public class BaseTable : BasePage
    {
        protected WebTable baseControl;

        public BaseTable(ILog logger, IWebDriver driver, By baseLocator) : base(logger, driver)
        {
            this.logger = logger;
            this.driver = driver;

            baseControl = new WebTable(driver, baseLocator);
        }

        public virtual int GetNumberOfRows()
        {
            logger.InfoFormat("GetNumberOfRows()");
            return baseControl.GetNumberOfRows();
        }

        public virtual int GetNumberOfColumns()
        {
            logger.InfoFormat("GetNumberOfColumns()");
            return baseControl.GetNumberOfColumns();
        }

        public virtual void ClickCell(object rowId, object columnId)
        {
            logger.InfoFormat("ClickCell({0}, {1})", rowId, columnId);
            baseControl.ClickCell(rowId, columnId);
        }

        public virtual void SetCellTextBox(object rowId, object columnId, string value)
        {
            logger.InfoFormat("SetCellTextBox({0}, {1})", rowId, columnId);
            baseControl.SetCellTextBox(rowId, columnId, value);
        }

        public virtual string GetCellTextBox(object rowId, object columnId)
        {
            logger.InfoFormat("GetCellTextBox({0}, {1})", rowId, columnId);
            return baseControl.GetCellTextBox(rowId, columnId);
        }

        public virtual void SetCellCheckBox(object rowId, object columnId, bool value)
        {
            logger.InfoFormat("SetCellCheckBox({0}, {1})", rowId, columnId);
            baseControl.SetCellCheckBox(rowId, columnId, value);
        }

        public virtual bool GetCellCheckBox(object rowId, object columnId)
        {
            logger.InfoFormat("GetCellCheckBox({0}, {1})", rowId, columnId);
            return baseControl.GetCellCheckBox(rowId, columnId);
        }

        public virtual void SetCellRadioButton(object rowId, object columnId, bool value)
        {
            logger.InfoFormat("SetCellRadioButton({0}, {1})", rowId, columnId);
            baseControl.SetCellRadioButton(rowId, columnId, value);
        }

        public virtual bool GetCellRadioButton(object rowId, object columnId)
        {
            logger.InfoFormat("GetCellRadioButton({0}, {1})", rowId, columnId);
            return baseControl.GetCellRadioButton(rowId, columnId);
        }

        public virtual string GetCellText(object rowId, object columnId)
        {
            logger.InfoFormat("GetCellText({0}, {1})", rowId, columnId);
            return baseControl.GetCellText(rowId, columnId);
        }

        public virtual string GetCellText(string cellText)
        {
            logger.InfoFormat("GetCellText({0})", cellText);
            return baseControl.GetCellText(cellText);
        }

        public virtual void ClickContextMenu(object rowId, object columnId, string menuEntry)
        {
            logger.InfoFormat("ClickContextMenu({0}, {1})", rowId, columnId);
            baseControl.ClickContextMenu(rowId, columnId, menuEntry);
        }
    }
}
