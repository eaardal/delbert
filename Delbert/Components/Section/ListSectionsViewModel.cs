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
    class ListSectionsViewModel : ScreenViewModel, IListSectionsViewModel
    {
        private readonly IActorSystemAdapter _actorSystem;

        public ListSectionsViewModel(IIoC ioc, IActorSystemAdapter actorSystem) : base(ioc)
        {
            if (actorSystem == null) throw new ArgumentNullException(nameof(actorSystem));
            _actorSystem = actorSystem;

            Sections = new ItemChangeAwareObservableCollection<SectionDto>();

            MessageBus.Subscribe<NotebookSelected>(async msg => await OnNotebookSelected(msg));
            MessageBus.Subscribe<SectionCreated>(async msg => await OnSectionCreated(msg));
        }

        private async Task OnSectionCreated(SectionCreated msg)
        {
            await DoOnUiDispatcherAsync(async () =>
            {
                await GetSections(msg.NewSectionParentNotebook);
            });
        }

        public ItemChangeAwareObservableCollection<SectionDto> Sections { get; }

        private async Task OnNotebookSelected(NotebookSelected msg)
        {
            try
            {
                await GetSections(msg.Notebook);
            }
            catch (Exception ex)
            {
                Log.Msg(this, l => l.Error(ex));
            }
        }

        private async Task GetSections(NotebookDto notebook)
        {
            await DoOnUiDispatcherAsync(() =>
            {
                Sections.Clear();
                notebook.Sections.ForEach(s => Sections.Add(s));
            });
        }

        public void SectionSelected(SectionDto section)
        {
            if (section == null)
            {
                Log.Msg(this, l => l.Warning("Selected section was null"));
                return;
            }

            Sections.ForEach(s => s.Deselect());

            section.Select();

            MessageBus.Publish(new SectionSelected(section));
        }
    }
}
