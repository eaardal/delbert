using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using Delbert.Infrastructure;

namespace Delbert.Model
{
    public class NotebookDto : NotifyPropertyChangedBase
    {
        private bool _isSelected;

        public NotebookDto()
        {
            Sections = ImmutableArray<SectionDto>.Empty;
        }

        public string Name { get; set; }
        public DirectoryInfo Directory { get; set; }
        public ImmutableArray<SectionDto> Sections { get; set; }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (Equals(value, _isSelected)) return;
                _isSelected = value;
                NotifyOfPropertyChanged(nameof(IsSelected));
            }
        }

        public void AddSections(IEnumerable<SectionDto> sections)
        {
            Sections = ImmutableArray.CreateRange(sections);
        }

        public void AddSection(SectionDto section)
        {
            Sections = Sections.Add(section);
        }

        public void Select()
        {
            IsSelected = true;
        }

        public void Deselect()
        {
            IsSelected = false;
        }
    }
}