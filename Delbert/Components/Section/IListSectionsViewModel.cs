using Delbert.Infrastructure;
using Delbert.Infrastructure.Abstract;
using Delbert.Model;

namespace Delbert.Components.Section
{
    internal interface IListSectionsViewModel : IScreenViewModel
    {
        ItemChangeAwareObservableCollection<SectionDto> Sections { get; } 
    }
}