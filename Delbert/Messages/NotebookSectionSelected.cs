using Delbert.Model;

namespace Delbert.Messages
{
    internal class NotebookSectionSelected
    {
        public SectionDto Section { get; }

        public NotebookSectionSelected(SectionDto section)
        {
            Section = section;
        }
    }
}