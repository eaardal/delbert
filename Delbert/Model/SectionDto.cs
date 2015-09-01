using System.IO;
using Delbert.Infrastructure;

namespace Delbert.Model
{
    internal class SectionDto : NotifyPropertyChangedBase
    {
        public string Name { get; set; }
        public DirectoryInfo Directory { get; set; }
        public NotebookDto Notebook { get; set; }
    }
}