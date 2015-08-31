using Caliburn.Micro;

namespace Delbert.Shell
{
    internal interface IShellViewModel
    {
        IScreen CurrentScreen { get; set; }
    }
}