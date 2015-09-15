using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;
using Akka.Actor;
using Delbert.Model;

namespace Delbert.Actors.Facades.Abstract
{
    public interface ISectionFacade
    {
        void CreateSection(DirectoryInfo directory);
        Task<ImmutableArray<SectionDto>> GetSectionsForNotebook(NotebookDto notebook);
        Task<ImmutableArray<SectionDto>> GetSectionsForNotebook(IActorRef actor, NotebookDto notebook);
    }
}