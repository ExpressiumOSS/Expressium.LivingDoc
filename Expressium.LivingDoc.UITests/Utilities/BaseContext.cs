using log4net;

namespace Expressium.LivingDoc.UITests.Utilities
{
    public class BaseContext
    {
        public Configuration Configuration { get; set; }
        public ILog Logger { get; set; }
        public WebDriverController Controller { get; set; }
        public Asserts Asserts {  get; set; }
    }
}
