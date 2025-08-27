using Expressium.Coffeeshop.Web.API.Models;
using Expressium.Coffeeshop.Web.API.Pages;
using Expressium.Coffeeshop.Web.API.Tests.Factories;
using Reqnroll;
using System;

namespace Expressium.Coffeeshop.Web.API.Tests.Steps
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
            // Example of a failing step in a scenario with exmaple tables...
            if (brand == "La Soledad Antioquia")
                throw new ApplicationException("Just a test exception to see how it looks in LivingDoc report.");

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

        [When("I add multiple pieces of coffees to the shopping cart")]
        public void WhenIAddMultiplePiecesOfCoffeesToTheShoppingCart(DataTable dataTable)
        {
            var coffees = dataTable.CreateInstance<Coffees>();

            var mainMenuBar = new MainMenuBar(logger, driver);
            mainMenuBar.ClickProducts();

            for (int i = 0; i < coffees.Pieces; i++)
            {
                var productsPage = new ProductsPage(logger, driver);
                productsPage.Grid.ClickCell(coffees.Brand, 6);
            }
        }

        [Then("I should have a confirmation notification message")]
        public void ThenIShouldHaveAConfirmationNotificationMessage()
        {
            var productsPage = new ProductsPage(logger, driver);
            Asserts.IsNotEmpty(productsPage.GetNotification(), "Validating the ProductsPage notification...");
        }

        private class Coffees
        {
            public string Brand { get; set; }
            public int Pieces { get; set; }
        }
    }
}
