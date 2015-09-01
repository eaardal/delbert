using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delbert.Actors;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Abstract;
using Delbert.Messages;
using Delbert.Model;

namespace Delbert.Components.Section
{
    class ListNotebookSectionsViewModel : ScreenViewModel, IListNotebookSectionsViewModel
    {
        private readonly IActorSystemAdapter _actorSystem;

        public ListNotebookSectionsViewModel(IIoC ioc, IActorSystemAdapter actorSystem) : base(ioc)
        {
            if (actorSystem == null) throw new ArgumentNullException(nameof(actorSystem));
            _actorSystem = actorSystem;

            NotebookSections = new ItemChangeAwareObservableCollection<SectionDto>();

            MessageBus.Subscribe<NotebookSelected>(async msg => await OnNotebookSelected(msg));
        }

        public ItemChangeAwareObservableCollection<SectionDto> NotebookSections { get; }

        private async Task OnNotebookSelected(NotebookSelected msg)
        {
            try
            {
                var sectionActor = _actorSystem.ActorOf(ActorRegistry.NotebookSection);

                var result =
                    await
                        sectionActor.AskWithResultOf<NotebookSectionActor.GetSectionsForNotebookResult>(
                            new NotebookSectionActor.GetSectionsForNotebook(msg.Notebook));

                await DoOnUiDispatcherAsync(() =>
                {
                    NotebookSections.Clear();
                    result.Sections.ForEach(s => NotebookSections.Add(s));
                });

            }
            catch (Exception ex)
            {
                Log.Msg(this, l => l.Error(ex));
            }
        }
    }
}
