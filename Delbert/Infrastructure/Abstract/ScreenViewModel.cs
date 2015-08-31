using System;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Delbert.Infrastructure.Logging.Contracts;

namespace Delbert.Infrastructure.Abstract
{
    public abstract class ScreenViewModel : Screen, IScreenViewModel
    {
        public IIoC IoC { get; }
        public IWindowManager WindowManager { get; }
        public IMessageBus MessageBus { get; }
        public ILogger Logger { get; }

        protected ScreenViewModel(IIoC ioc, IWindowManager windowManager, IMessageBus messageBus, ILogger logger)
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

        protected ScreenViewModel(IIoC ioc, IWindowManager windowManager) : this(ioc, windowManager, ioc.Resolve<IMessageBus>(), ioc.Resolve<ILogger>()) { }

        protected ScreenViewModel(IIoC ioc) :this(ioc, ioc.Resolve<IWindowManager>()) { }

        public void ShowWindow<TViewModel>()
        {
            WindowManager.ShowWindow(IoC.Resolve<TViewModel>());
        }

        public void ShowWindow<TViewModel>(TViewModel viewModel)
        {
            WindowManager.ShowWindow(viewModel);
        }

        public void DoOnUiDispatcher(System.Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }

        public async Task DoOnUiDispatcherAsync(System.Action action)
        {
            await Task.Run(() => DoOnUiDispatcher(action));
        }
    }
}