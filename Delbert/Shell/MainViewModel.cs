using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delbert.Infrastructure.Abstract;
using Delbert.Shell.Abstract;

namespace Delbert.Shell
{
    sealed class MainViewModel : ViewModelBase, IMainViewModel
    {
        public MainViewModel(IIoC ioc) : base(ioc)
        {
        }
    }
}
