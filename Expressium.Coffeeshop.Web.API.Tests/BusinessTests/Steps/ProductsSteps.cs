using Expressium.Coffeeshop.Web.API.Models;
using Expressium.Coffeeshop.Web.API.Pages;
using Expressium.Coffeeshop.Web.API.Tests.Factories;
using Reqnroll;

namespace Expressium.Coffeeshop.Web.API.Tests.BusinessTests.Steps
{
    [Binding]
    public class ProductsSteps : BaseSteps
    {
        public ProductsSteps(ContextController contextController) : base(contextController)
        {
        }

        [When(@"I add (.*) coffee to the shopping cart")]
        public void WhenIAddCoffeeToTheShoppingCart(string brand)
        {
            var mainMenuBar = new MainMenuBar(logger, driver);
            mainMenuBar.ClickProducts();

            var productsPage = new ProductsPage(logger, driver);
            productsPage.Grid.ClickCell(brand, 6);
        }

        [Then(@"I should have (.*) in the confirmation notification message")]
        public void ThenIShouldHavePriceInTheConfirmationNotificationMessage(string price)
        {
            var productsPage = new ProductsPage(logger, driver);
            Asserts.DoesContain(productsPage.GetNotification(), price, "Validating the ProductsPage Price notification...");
        }
    }
}
