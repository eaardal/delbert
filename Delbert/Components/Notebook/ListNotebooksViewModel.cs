using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using Akka.Actor;
using Delbert.Actors;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Abstract;
using Delbert.Messages;
using Delbert.Model;

namespace Delbert.Components.Notebook
{
    sealed class ListNotebooksViewModel : ScreenViewModel, IListNotebooksViewModel
    {
        private readonly IActorSystemAdapter _actorSystem;

        public ListNotebooksViewModel(IIoC ioc, IActorSystemAdapter actorSystem) : base(ioc)
        {
            if (actorSystem == null) throw new ArgumentNullException(nameof(actorSystem));
            _actorSystem = actorSystem;

            Notebooks = new ItemChangeAwareObservableCollection<NotebookDto>();

            MessageBus.Subscribe<RootDirectoryChanged>(async msg => await OnNewRootDirectory(msg));
        }

        public ItemChangeAwareObservableCollection<NotebookDto> Notebooks { get; set; }

        private async Task OnNewRootDirectory(RootDirectoryChanged message)
        {
            try
            {
                var notebookActor = _actorSystem.ActorOf(ActorRegistry.Notebook);

                var query = await notebookActor.Query(new NotebookActor.GetNotebooks());

                query
                    .WhenResultIs<NotebookActor.GetNotebooksResult>(async result =>
                    {
                        await DoOnUiDispatcherAsync(() =>
                        {
                            Notebooks.Clear();
                            result.Notebooks.ForEach(n => Notebooks.Add(n));
                        });
                    })
                    .LogFailure();


            }
            catch (Exception ex)
            {
                Log.Msg(this, l => l.Error(ex));
            }
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
