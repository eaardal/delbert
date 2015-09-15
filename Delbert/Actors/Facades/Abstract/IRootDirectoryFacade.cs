using System.IO;
using System.Threading.Tasks;
using Akka.Actor;

namespace Delbert.Actors.Facades.Abstract
{
    internal interface IRootDirectoryFacade
    {
        void SetRootDirectory(DirectoryInfo directory);
        void SetRootDirectory(ActorSelection actorSelection, DirectoryInfo directory);
        Task<DirectoryInfo> GetRootDirectory();
        Task<DirectoryInfo> GetRootDirectory(ActorSelection actorSelection);
    }
}