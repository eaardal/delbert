using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;
using Akka.Actor;
using Delbert.Model;

namespace Delbert.Actors.Facades.Abstract
{
    public interface INotebookFacade
    {
        void CreateNotebook(DirectoryInfo directory);
        void CreateNotebook(IUntypedActorContext context, DirectoryInfo directory);
        Task<ImmutableArray<NotebookDto>> GetNotebooks();
        Task<ImmutableArray<NotebookDto>> GetNotebooks(IActorRef actor);
    }
}