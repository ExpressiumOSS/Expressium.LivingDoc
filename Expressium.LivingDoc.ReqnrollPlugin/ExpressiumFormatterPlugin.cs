using Expressium.LivingDoc.ReqnrollPlugin;
using Reqnroll.Formatters;
using Reqnroll.Formatters.RuntimeSupport;
using Reqnroll.Plugins;
using Reqnroll.UnitTestProvider;

[assembly: RuntimePlugin(typeof(ExpressiumFormatterPlugin))]

namespace Expressium.LivingDoc.ReqnrollPlugin
{
    public class ExpressiumFormatterPlugin : IRuntimePlugin
    {
        public void Initialize(RuntimePluginEvents runtimePluginEvents, RuntimePluginParameters runtimePluginParameters, UnitTestProviderConfiguration unitTestProviderConfiguration)
        {
            runtimePluginEvents.RegisterGlobalDependencies += (_, args) =>
            {
                args.ObjectContainer.RegisterTypeAs<ExpressiumFormatter, ICucumberMessageFormatter>("expressium");
            };

            runtimePluginEvents.CustomizeGlobalDependencies += (_, args) =>
            {
                args.ObjectContainer.RegisterTypeAs<TraceListenerFormatterLog, IFormatterLog>();

            };
        }
    }
}
