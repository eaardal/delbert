using Delbert.Model;

namespace Delbert.Messages
{
    internal class SectionCreated
    {
        public SectionCreated(NotebookDto newSectionParentNotebook)
        {
            NewSectionParentNotebook = newSectionParentNotebook;
        }

        public NotebookDto NewSectionParentNotebook { get; set; }
    }
}