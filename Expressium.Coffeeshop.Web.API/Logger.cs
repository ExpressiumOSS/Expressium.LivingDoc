using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;

namespace Expressium.Coffeeshop.Web.API
{
    public class Logger
    {
        public static ILog Initialize(string name, string filePath = null)
        {
            var repository = LoggerManager.CreateRepository(name);

            var patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date{HH:mm:ss} %level %type # %message%newline";
            patternLayout.ActivateOptions();

            var consoleAppender = new ConsoleAppender();
            consoleAppender.Layout = patternLayout;
            consoleAppender.Threshold = Level.All;
            consoleAppender.ActivateOptions();

            if (string.IsNullOrWhiteSpace(filePath))
            {
                BasicConfigurator.Configure(repository, consoleAppender);
            }
            else
            {
                var rollingFileAppender = new RollingFileAppender();
                rollingFileAppender.AppendToFile = false;
                rollingFileAppender.File = filePath;
                rollingFileAppender.Layout = patternLayout;
                rollingFileAppender.Threshold = Level.All;
                rollingFileAppender.MaxSizeRollBackups = 0;
                rollingFileAppender.MaximumFileSize = "1GB";
                rollingFileAppender.ActivateOptions();

                BasicConfigurator.Configure(repository, rollingFileAppender, consoleAppender);
            }

            return LogManager.GetLogger(name, name);
        }
    }
}
