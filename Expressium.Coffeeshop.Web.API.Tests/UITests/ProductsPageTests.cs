using NUnit.Framework;
using Expressium.Coffeeshop.Web.API.Pages;
using System;

namespace Expressium.Coffeeshop.Web.API.Tests.UITests
{
    [Category("UITests")]
    [Description("Validate Navigation & Content of ProductsPage")]
    public class ProductsPageTests : BaseTest
    {
        private ProductsPage productsPage;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            InitializeBrowserWithLogin();

            // TODO - Implement potential missing page navigations...

            var mainMenuBar = new MainMenuBar(logger, driver);
            mainMenuBar.ClickProducts();

            productsPage = new ProductsPage(logger, driver);
        }

        [Test]
        public void Validate_Page_Title()
        {
            Asserts.EqualTo(productsPage.GetTitle(), "Products", "Validating the ProductsPage Title...");
        }

        [Test]
        public void Validate_Page_Grid_Number_Of_Rows()
        {
            Asserts.GreaterThan(productsPage.Grid.GetNumberOfRows(), -1, "Validating the ProductsPage Grid number of rows...");
        }

        [Test]
        public void Validate_Page_Grid_Number_Of_Columns()
        {
            Asserts.GreaterThan(productsPage.Grid.GetNumberOfColumns(), -1, "Validating the ProductsPage Grid number of columns...");
        }

        [Test]
        public void Validate_Page_Grid_Cell_Text_Country()
        {
            Asserts.IsNotNull(productsPage.Grid.GetCellText(1, "Country"), "Validating the ProductsPage Grid table cell Text Country...");
        }

        [Test]
        public void Validate_Page_Grid_Cell_Text_Type()
        {
            Asserts.IsNotNull(productsPage.Grid.GetCellText(1, "Type"), "Validating the ProductsPage Grid table cell Text Type...");
        }

        [Test]
        public void Validate_Page_Grid_Cell_Text_Brand()
        {
            Asserts.IsNotNull(productsPage.Grid.GetCellText(1, "Brand"), "Validating the ProductsPage Grid table cell Text Brand...");
        }

        [Test]
        public void Validate_Page_Grid_Cell_Text_Price()
        {
            Asserts.IsNotNull(productsPage.Grid.GetCellText(1, "Price"), "Validating the ProductsPage Grid table cell Text Price...");
        }
    }
}
