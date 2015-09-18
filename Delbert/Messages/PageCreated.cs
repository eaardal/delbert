using System;
using Delbert.Model;

namespace Delbert.Messages
{
    internal class PageCreated
    {
        public SectionDto SelectedSection { get; }

        public PageCreated(SectionDto selectedSection)
        {
            if (selectedSection == null) throw new ArgumentNullException(nameof(selectedSection));
            SelectedSection = selectedSection;
        }
    }
}