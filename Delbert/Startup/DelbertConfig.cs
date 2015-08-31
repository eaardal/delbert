using System.Linq;
using System.Reflection;
using Autofac;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Abstract;
using Delbert.Infrastructure.Logging;
using Delbert.Infrastructure.Logging.Contracts;
using Delbert.Infrastructure.Logging.LogFactories;

namespace Delbert.Startup
{
    class DelbertConfig
    {
        public static void ConfigureDependencies(ContainerBuilder builder)
        {
            var thisAssembly = Assembly.GetExecutingAssembly();
            var referencedAssemblies = thisAssembly.GetReferencedAssemblies().Where(a => a.Name.StartsWith("Delbert") || a.Name.StartsWith("Eaardal"));
            var appAssemblies = referencedAssemblies.Select(Assembly.Load).Concat(new[] { thisAssembly }).ToArray();

            builder.RegisterAssemblyTypes(appAssemblies)
              .Except<Logger>()
              .Except<IoC>()
              .Except<MessageBus>()
              .Except<WindowsEventLogLogFactory>()
              .Except<TraceLogFactory>()
              .Except<ConsoleLogFactory>()
              .AsImplementedInterfaces()
              .AsSelf();

            builder.RegisterType<Logger>().AsImplementedInterfaces().AsSelf().SingleInstance();
            builder.RegisterType<IoC>().As<IIoC>().SingleInstance();
            builder.RegisterType<MessageBus>().As<IMessageBus>().SingleInstance();
        }
        
        public static void Konfigurer(IContainer container)
        {
            var ioc = container.Resolve<IIoC>();
            ioc.RegisterContainer(container);

            var messageBus = ioc.Resolve<IMessageBus>();

            var log = ioc.Resolve<ILogger>();
            
            log.InitializeLogFactories(new SerilogLogFactory(), new DebugLogFactory(), new MessagePublishingLogFactory(messageBus));
            log.Msg(typeof(DelbertConfig), l => l.Info("Logging is initialized"));
        }
    }
}
