using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using Delbert.Actors.Facades.Abstract;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Abstract;
using Delbert.Messages;
using Delbert.Model;

namespace Delbert.Components.Page
{
    public sealed class ListPagesViewModel : ScreenViewModel, IListPagesViewModel
    {
        private readonly ISectionFacade _section;
        private readonly IPageFacade _page;

        public ListPagesViewModel(ISectionFacade section, IPageFacade page, IIoC ioc) : base(ioc)
        {
            if (section == null) throw new ArgumentNullException(nameof(section));
            if (page == null) throw new ArgumentNullException(nameof(page));
            _section = section;
            _page = page;

            Pages = new ItemChangeAwareObservableCollection<PageDto>();

            MessageBus.Subscribe<SectionSelected>(async msg => await OnSectionSelected(msg));
            MessageBus.Subscribe<PageCreated>(async msg => await OnPageCreated(msg));
        }

        private async Task OnPageCreated(PageCreated message)
        {
            var pages = await _page.GetPagesForSection(message.SelectedSection);
            
            await DoOnUiDispatcherAsync(() =>
            {
                Pages.Clear();
                pages.ForEach(p => Pages.Add(p));
            });
        }

        public ItemChangeAwareObservableCollection<PageDto> Pages { get; }

        public void PageSelected(PageDto page)
        {
            if (page == null)
            {
                Log.Msg(this, l => l.Warning("Selected page was null"));
                return;
            }

            Pages.ForEach(p => p.Deselect());

            page.Select();

            MessageBus.Publish(new PageSelected(page));
        }

        private async Task OnSectionSelected(SectionSelected message)
        {
            try
            {
                await DoOnUiDispatcherAsync(() =>
                {
                    Pages.Clear();
                    message.Section.Pages.ForEach(page => Pages.Add(page));
                });
            }
            catch (Exception ex)
            {
                Log.Msg(this, l => l.Error(ex));
            }
        }
    }
}
