using System.Collections.Immutable;
using System.Threading.Tasks;
using Akka.Actor;
using Delbert.Model;

namespace Delbert.Actors.Facades.Abstract
{
    internal interface ISectionFacade
    {
        Task<ImmutableArray<SectionDto>> GetSectionsForNotebook(NotebookDto notebook);
        Task<ImmutableArray<SectionDto>> GetSectionsForNotebook(IActorRef actor, NotebookDto notebook);
    }
}