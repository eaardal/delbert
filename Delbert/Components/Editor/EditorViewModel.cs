using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delbert.Actors.Facades.Abstract;
using Delbert.Infrastructure.Abstract;
using Delbert.Messages;
using Delbert.Model;

namespace Delbert.Components.Editor
{
    public sealed class EditorViewModel : ScreenViewModel, IEditorViewModel
    {
        private readonly IProcessFacade _process;
        private string _text;
        private PageDto _page;

        public EditorViewModel(IProcessFacade process, IIoC ioc) : base(ioc)
        {
            _process = process;
            MessageBus.Subscribe<PageSelected>(async msg => await OnPageSelected(msg));
        }

        private async Task OnPageSelected(PageSelected message)
        {
            try
            {
                await DoOnUiDispatcherAsync(() =>
                {
                    Page = message.Page;
                    Text = Page.Text;
                });
            }
            catch (Exception ex)
            {
                Log.Msg(this, l => l.Error(ex));
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if (value == _text) return;
                _text = value;
                NotifyOfPropertyChange(() => Text);
            }
        }

        public PageDto Page
        {
            get { return _page; }
            set
            {
                if (Equals(value, _page)) return;
                _page = value;
                NotifyOfPropertyChange(() => Page);
            }
        }

        public void EditorSelected()
        {
            if (Page != null)
            {
                _process.StartProcessForFile(Page.File);
            }
        }
    }
}
