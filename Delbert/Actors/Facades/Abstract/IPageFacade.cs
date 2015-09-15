using System.Collections.Immutable;
using System.Threading.Tasks;
using Akka.Actor;
using Delbert.Model;

namespace Delbert.Actors.Facades.Abstract
{
    public interface IPageFacade
    {
        Task<ImmutableArray<PageDto>> GetPagesForSection(SectionDto section);
        Task<ImmutableArray<PageDto>> GetPagesForSection(IActorRef actor, SectionDto section);
    }
}