using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using Delbert.Infrastructure;

namespace Delbert.Model
{
    public class NotebookDto : NotifyPropertyChangedBase
    {
        public string Name { get; set; }
        public DirectoryInfo Directory { get; set; }
        public ImmutableArray<SectionDto> Sections { get; set; }

        public void AddSections(IEnumerable<SectionDto> sections)
        {
            Sections = ImmutableArray.CreateRange(sections);
        }

        public void AddSection(SectionDto section)
        {
            Sections = Sections.Add(section);
        }
    }
}