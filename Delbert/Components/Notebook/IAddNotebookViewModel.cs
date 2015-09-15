using Delbert.Infrastructure.Abstract;

namespace Delbert.Components.Notebook
{
    public interface IAddNotebookViewModel : IScreenViewModel
    {
        string NewNotebookName { get; set; }
        void CreateNew();
    }
}