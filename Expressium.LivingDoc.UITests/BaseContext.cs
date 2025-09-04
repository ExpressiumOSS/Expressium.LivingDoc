using log4net;

namespace Expressium.LivingDoc.UITests
{
    public class BaseContext
    {
        public Configuration Configuration { get; set; }
        public ILog Logger { get; set; }
        public WebDriverManager DriverManager { get; set; }
        public Asserts Asserts {  get; set; }
    }
}
