using System.IO;
using System.Threading.Tasks;
using Akka.Actor;

namespace Delbert.Actors.Facades.Abstract
{
    public interface ICommandLineArgsParserFacade
    {
        Task<DirectoryInfo> GetRootDirectoryFromCommandLine();
        Task<DirectoryInfo> GetRootDirectoryFromCommandLine(IActorRef context);
    }
}