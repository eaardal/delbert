using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using Akka.Actor;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Abstract;
using Delbert.Messages;
using Delbert.Model;
using Delbert.Services;

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

            MessageBus.Subscribe<NewRootDirectorySet>(async msg => await OnNewRootDirectory(msg));
        }

        public ItemChangeAwareObservableCollection<NotebookDto> Notebooks { get; set; }

        private async Task OnNewRootDirectory(NewRootDirectorySet message)
        {
            try
            {
                var notebook = _actorSystem.ActorOf(ActorRegistry.Notebook);

                var result = await notebook.AskWithResultOf<NotebookActor.GetNotebooksResult>(new NotebookActor.GetNotebooks());

                result.Notebooks.ForEach(n => Notebooks.Add(n));
            }
            catch (Exception ex)
            {
                Log.Msg(this, l => l.Error(ex));
            }
        }
    }
}
