using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using Akka.Actor;
using Delbert.Actors;
using Delbert.Actors.Facades.Abstract;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Abstract;
using Delbert.Messages;
using Delbert.Model;

namespace Delbert.Components.Notebook
{
    sealed class ListNotebooksViewModel : ScreenViewModel, IListNotebooksViewModel
    {
        private readonly INotebookFacade _notebook;
        
        public ListNotebooksViewModel(INotebookFacade notebook, IIoC ioc) : base(ioc)
        {
            if (notebook == null) throw new ArgumentNullException(nameof(notebook));
            _notebook = notebook;
            
            Notebooks = new ItemChangeAwareObservableCollection<NotebookDto>();

            MessageBus.Subscribe<RootDirectoryChanged>(async msg => await OnNewRootDirectory(msg));
            MessageBus.Subscribe<NotebookCreated>(async msg => await OnNotebookCreated(msg));
            MessageBus.Subscribe<SectionCreated>(async msg => await OnSectionCreated(msg));
        }

        private async Task OnSectionCreated(SectionCreated msg)
        {
            var notebooks = await _notebook.GetNotebooks();

            var selectedNotebook = Notebooks.Single(n => n.IsSelected);

            await DoOnUiDispatcherAsync(() =>
            {
                Notebooks.Clear();
                notebooks.ForEach(n =>
                {
                    if (n.Id == selectedNotebook.Id)
                    {
                        n.Select();
                    }
                    Notebooks.Add(n);
                });
            });
        }

        private async Task OnNotebookCreated(NotebookCreated msg)
        {
            await GetNotebooks();
        }

        public ItemChangeAwareObservableCollection<NotebookDto> Notebooks { get; set; }

        protected override async void OnActivate()
        {
            await GetNotebooks();

            base.OnActivate();
        }

        private async Task OnNewRootDirectory(RootDirectoryChanged message)
        {
            await GetNotebooks();
        }

        private async Task GetNotebooks()
        {
            var notebooks = await _notebook.GetNotebooks();

            await DoOnUiDispatcherAsync(() =>
            {
                Notebooks.Clear();
                notebooks.ForEach(n => Notebooks.Add(n));
            });
        }

        public void NotebookSelected(NotebookDto notebook)
        {
            if (notebook == null)
            {
                Log.Msg(this, l => l.Warning("Selected notebook was null"));
                return;
            }

            Notebooks.ForEach(n => n.Deselect());

            notebook.Select();

            MessageBus.Publish(new NotebookSelected(notebook));
        }
    }
}
