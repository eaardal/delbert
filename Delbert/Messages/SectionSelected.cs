using Delbert.Model;

namespace Delbert.Messages
{
    internal class SectionSelected
    {
        public SectionDto Section { get; }

        public SectionSelected(SectionDto section)
        {
            Section = section;
        }
    }
}