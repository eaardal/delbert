using System;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Delbert.Infrastructure.Logging.Contracts;

namespace Delbert.Infrastructure.Abstrakt
{
    public abstract class ViewModelBase : Conductor<IScreen>, IViewModelBase
    {
        protected IIoC IoC { get; }
        protected IWindowManager WindowManager { get; }
        protected IMessageBus MessageBus { get; }
        protected ILogger Logger { get; }

        protected ViewModelBase(IIoC ioc, IWindowManager windowManager, IMessageBus messageBus, ILogger logger)
        {
            if (ioc == null) throw new ArgumentNullException(nameof(ioc));
            if (windowManager == null) throw new ArgumentNullException(nameof(windowManager));
            if (messageBus == null) throw new ArgumentNullException(nameof(messageBus));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            MessageBus = messageBus;
            IoC = ioc;
            WindowManager = windowManager;
            Logger = logger;
        }

        protected ViewModelBase(IIoC ioc, IWindowManager windowManager) : this(ioc, windowManager, ioc.Resolve<IMessageBus>(), ioc.Resolve<ILogger>()) { }

        protected ViewModelBase(IIoC ioc) :this(ioc, ioc.Resolve<IWindowManager>()) { }

        protected void ShowWindow<TViewModel>()
        {
            WindowManager.ShowWindow(IoC.Resolve<TViewModel>());
        }

        protected void ShowWindow<TViewModel>(TViewModel viewModel)
        {
            WindowManager.ShowWindow(viewModel);
        }

        protected void DoOnUiDispatcher(System.Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }

        protected async Task DoOnUiDispatcherAsync(System.Action action)
        {
            await Task.Run(() => DoOnUiDispatcher(action));
        }
    }
}
