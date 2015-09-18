using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows;
using Autofac;
using Caliburn.Micro;
using Caliburn.Micro.Autofac;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Abstract;
using Delbert.Shell;

namespace Delbert.Startup
{
    class CaliburnAutofacConfig : AutofacBootstrapper<ShellViewModel>
    {
        public CaliburnAutofacConfig()
        {
            var culture = new CultureInfo("en-GB");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            Initialize();
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            DelbertConfig.ConfigureDependencies(builder);

            builder.RegisterType<WindowManager>().As<IWindowManager>().SingleInstance();
            builder.RegisterType<MessageBus>().As<IMessageBus>().SingleInstance();
        }

        protected override void Configure()
        {
            base.Configure();

            EnforceNamespaceConvention = true;
            ViewModelBaseType = typeof(IViewModelBase);
            
            DelbertConfig.ConfigureApplication(Container);
        }
        
        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] { Assembly.GetExecutingAssembly() };
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                if (Container.IsRegistered(serviceType))
                    return Container.Resolve(serviceType);
            }
            else
            {
                if (Container.IsRegisteredWithKey(key, serviceType))
                    return Container.ResolveKeyed(key, serviceType);
            }
            throw new Exception($"Could not locate any instances of contract {key ?? serviceType.Name}.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return Container.Resolve(typeof(IEnumerable<>).MakeGenericType(service)) as IEnumerable<object>;
        }

        protected override void BuildUp(object instance)
        {
            Container.InjectProperties(instance);
        }
    }
}
