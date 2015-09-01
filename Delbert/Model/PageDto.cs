using System.IO;
using Delbert.Infrastructure;

namespace Delbert.Model
{
    public class PageDto : NotifyPropertyChangedBase
    {
        public SectionDto ParentSection { get; set; }
        public NotebookDto ParentNotebook { get; set; }
        public FileInfo File { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
    }
}