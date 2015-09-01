using Delbert.Model;

namespace Delbert.Messages
{
    public class PageSelected
    {
        public PageDto Page { get; }

        public PageSelected(PageDto page)
        {
            Page = page;
        }
    }
}