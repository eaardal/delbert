using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Abstract;
using Delbert.Messages;
using Delbert.Model;

namespace Delbert.Components.Page
{
    public sealed class ListPagesViewModel : ScreenViewModel, IListPagesViewModel
    {
        public ListPagesViewModel(IIoC ioc) : base(ioc)
        {
            Pages = new ItemChangeAwareObservableCollection<PageDto>();

            MessageBus.Subscribe<SectionSelected>(async msg => await OnSectionSelected(msg));
        }

        public ItemChangeAwareObservableCollection<PageDto> Pages { get; }

        public void PageSelected(PageDto page)
        {
            if (page == null)
            {
                Log.Msg(this, l => l.Warning("Selected page was null"));
                return;
            }

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
