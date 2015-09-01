using Delbert.Model;

namespace Delbert.Messages
{
    internal class NotebookSelected
    {
        public NotebookSelected(NotebookDto notebook)
        {
            Notebook = notebook;
        }

        public NotebookDto Notebook { get; }
    }
}