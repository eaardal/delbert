using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delbert.Infrastructure.Abstract;
using Delbert.Model;

namespace Delbert.Components.Section
{
    class ListNotebookSectionsViewModel : ScreenViewModel, IListNotebookSectionsViewModel
    {
        public ListNotebookSectionsViewModel(IIoC ioc) : base(ioc)
        {
            MessageBus.Subscribe<NotebookSelected>(async msg => await OnNotebookSelected(msg));
        }

        private async Task OnNotebookSelected(NotebookSelected msg)
        {
            throw new NotImplementedException();
        }
    }

    internal class NotebookSelected
    {
        public NotebookDto Notebook { get; }

        public NotebookSelected(NotebookDto notebook)
        {
            if (notebook == null) throw new ArgumentNullException(nameof(notebook));
            Notebook = notebook;
        }
    }

    internal interface IListNotebookSectionsViewModel : IScreenViewModel
    {
    }
}
