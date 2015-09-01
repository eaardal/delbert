using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using Delbert.Infrastructure;

namespace Delbert.Model
{
    public class SectionDto : NotifyPropertyChangedBase
    {
        public string Name { get; set; }
        public DirectoryInfo Directory { get; set; }
        public NotebookDto ParentNotebook { get; set; }
        public ImmutableArray<PageDto> Pages { get; set; }

        public void AddPages(IEnumerable<PageDto> pages)
        {
            Pages = ImmutableArray.CreateRange(pages);
        }

        public void AddPage(PageDto page)
        {
            Pages = Pages.Add(page);
        }
    }
}