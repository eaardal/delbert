using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;
using Akka.Actor;
using Delbert.Model;

namespace Delbert.Actors.Facades.Abstract
{
    public interface IPageFacade
    {
        Task<ImmutableArray<PageDto>> GetPagesForSection(SectionDto section);
        Task<ImmutableArray<PageDto>> GetPagesForSection(IActorRef actor, SectionDto section);
        void CreatePage(FileInfo pageFile);
        void CreatePage(IActorRef actor, FileInfo pageFile);
    }
}