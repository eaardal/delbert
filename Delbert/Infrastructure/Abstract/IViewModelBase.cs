using System.Threading.Tasks;
using Caliburn.Micro;
using Delbert.Infrastructure.Logging.Contracts;

namespace Delbert.Infrastructure.Abstract
{
    public interface IViewModelBase
    {
        IIoC IoC { get; }
        IWindowManager WindowManager { get; }
        IMessageBus MessageBus { get; }
        ILogger Logger { get; }
        void ShowWindow<TViewModel>();
        void ShowWindow<TViewModel>(TViewModel viewModel);
        void DoOnUiDispatcher(System.Action action);
        Task DoOnUiDispatcherAsync(System.Action action);
    }
}