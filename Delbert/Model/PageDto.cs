using System.IO;

namespace Delbert.Model
{
    public class PageDto
    {
        public SectionDto ParentSection { get; set; }
        public NotebookDto ParentNotebook { get; set; }
        public FileInfo File { get; set; }
        public string Name { get; set; }
    }
}