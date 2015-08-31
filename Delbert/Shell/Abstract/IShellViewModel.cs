using Caliburn.Micro;

namespace Delbert.Shell.Abstract
{
    internal interface IShellViewModel
    {
        IScreen CurrentScreen { get; set; }
    }
}