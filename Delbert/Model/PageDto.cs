using System.IO;

namespace Delbert.Model
{
    public class PageDto
    {
        public SectionDto Section { get; set; }
        public NotebookDto Notebook { get; set; }
        public FileInfo File { get; set; }
        public string Name { get; set; }
    }
}