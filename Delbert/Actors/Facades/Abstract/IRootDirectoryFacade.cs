using System.IO;
using System.Threading.Tasks;
using Akka.Actor;

namespace Delbert.Actors.Facades.Abstract
{
    public interface IRootDirectoryFacade
    {
        void SetRootDirectory(DirectoryInfo directory);
        void SetRootDirectory(ActorSelection actorSelection, DirectoryInfo directory);
        Task<DirectoryInfo> GetRootDirectory();
        Task<DirectoryInfo> GetRootDirectory(ActorSelection actorSelection);
        void SetRootDirectoryFromCommandLineArgumentsIfExists();
        void SetRootDirectoryFromCommandLineArgumentsIfExists(ActorSelection actorSelection);
    }
}