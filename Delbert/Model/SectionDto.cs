using System.Collections.Immutable;
using System.IO;
using Delbert.Infrastructure;

namespace Delbert.Model
{
    public class SectionDto : NotifyPropertyChangedBase
    {
        public string Name { get; set; }
        public DirectoryInfo Directory { get; set; }
        public NotebookDto Notebook { get; set; }
        public ImmutableArray<PageDto> Pages { get; set; }
    }
}