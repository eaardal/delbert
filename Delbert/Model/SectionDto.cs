using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using Delbert.Infrastructure;

namespace Delbert.Model
{
    public class SectionDto : NotifyPropertyChangedBase
    {
        private bool _isSelected;

        public string Name { get; set; }
        public DirectoryInfo Directory { get; set; }
        public NotebookDto ParentNotebook { get; set; }
        public ImmutableArray<PageDto> Pages { get; set; }
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

        public void AddPages(IEnumerable<PageDto> pages)
        {
            Pages = ImmutableArray.CreateRange(pages);
        }

        public void AddPage(PageDto page)
        {
            Pages = Pages.Add(page);
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