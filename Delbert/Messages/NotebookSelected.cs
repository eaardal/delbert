using System;
using Delbert.Model;

namespace Delbert.Messages
{
    internal class NotebookSelected
    {
        public NotebookDto Notebook { get; }

        public NotebookSelected(NotebookDto notebook)
        {
            if (notebook == null) throw new ArgumentNullException(nameof(notebook));
            Notebook = notebook;
        }
    }
}