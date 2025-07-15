using Expressium.Coffeeshop.Web.API.Pages;
using Reqnroll;

namespace Expressium.Coffeeshop.Web.API.Tests.Steps
{
    [Binding]
    public class ContactUsSteps : BaseSteps
    {
        public ContactUsSteps(ContextController contextController) : base(contextController)
        {
        }

        [When(@"I complete and submit the Contact Us formular")]
        public void WhenICreateAndSubmitTheContactUsFormular()
        {
            var mainMenuBar = new MainMenuBar(logger, driver);
            mainMenuBar.ClickContactUs();

            var contactUsPage = new ContactUsPage(logger, driver);
            contactUsPage.SetName("John Doe");
            contactUsPage.SetEmail("john.doe@microsoft.com");
            contactUsPage.SetSubject("Detailed product information");
            contactUsPage.SetMessage("Please send some more detailed product information...");
            contactUsPage.ClickSend();
        }

        [Then(@"I should receive an inquiry confirmation message")]
        public void ThenIShouldHaveANotificationOnTheContactUsPage()
        {
            var contactUsPage = new ContactUsPage(logger, driver);

            var expected = "Your contact information has been send - Thanks...";
            Asserts.EqualTo(contactUsPage.GetNotification(), expected, "Validate ContactUsPage notification message...");
        }
    }
}
