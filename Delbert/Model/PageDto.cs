using System.IO;
using Delbert.Infrastructure;

namespace Delbert.Model
{
    public class PageDto : NotifyPropertyChangedBase
    {
        private bool _isSelected;

        public SectionDto ParentSection { get; set; }
        public NotebookDto ParentNotebook { get; set; }
        public FileInfo File { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
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