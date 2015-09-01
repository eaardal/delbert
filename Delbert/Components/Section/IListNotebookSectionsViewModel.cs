using Delbert.Infrastructure;
using Delbert.Infrastructure.Abstract;
using Delbert.Model;

namespace Delbert.Components.Section
{
    internal interface IListNotebookSectionsViewModel : IScreenViewModel
    {
        ItemChangeAwareObservableCollection<SectionDto> NotebookSections { get; } 
    }
}