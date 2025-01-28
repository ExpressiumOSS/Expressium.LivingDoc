using log4net;
using OpenQA.Selenium;

namespace Expressium.Coffeeshop.Web.API.Tests
{
    public class ContextController
    {
        public Configuration Configuration { get; set; }
        public ILog Logger { get; set; }
        public LazyWebDriver Driver { get; set; }
        public Asserts Asserts {  get; set; }
    }
}
