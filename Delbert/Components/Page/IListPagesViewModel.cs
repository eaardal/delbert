using Delbert.Infrastructure;
using Delbert.Infrastructure.Abstract;
using Delbert.Model;

namespace Delbert.Components.Page
{
    public interface IListPagesViewModel : IScreenViewModel
    {
        ItemChangeAwareObservableCollection<PageDto> Pages { get; }
        void PageSelected(PageDto page);
    }
}