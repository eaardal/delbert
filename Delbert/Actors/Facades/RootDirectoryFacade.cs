using System;
using System.IO;
using System.Threading.Tasks;
using Akka.Actor;
using Delbert.Actors.Facades.Abstract;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Logging.Contracts;

namespace Delbert.Actors.Facades
{
    class RootDirectoryFacade : ActorFacade, IRootDirectoryFacade
    {
        public RootDirectoryFacade(IActorSystemAdapter actorSystem, ILogger logger) : base(actorSystem, logger)
        {

        }

        public void SetRootDirectory(DirectoryInfo directory)
        {
            var actor = ActorSystem.ActorSelection(ActorRegistry.RootDirectory);

            SetRootDirectory(actor, directory);
        }
        
        public async Task<DirectoryInfo> GetRootDirectory()
        {
            var actor = ActorSystem.ActorSelection(ActorRegistry.RootDirectory);

            return await GetRootDirectory(actor);
        }
        
        public async Task<DirectoryInfo> GetRootDirectory(ActorSelection actor)
        {
            var answer = await actor.Ask(new RootDirectoryActor.GetRootDirectory());

            if (answer is RootDirectoryActor.GetRootDirectoryResult)
            {
                var result = answer as RootDirectoryActor.GetRootDirectoryResult;

                return result.Success ? result.CurrentRootDirectory : null;
            }

            LogFailure(answer);

            return null;
        }

        public void SetRootDirectoryFromCommandLineArgumentsIfExists()
        {
            var rootDirectoryActor = ActorSystem.ActorSelection(ActorRegistry.RootDirectory);

            SetRootDirectoryFromCommandLineArgumentsIfExists(rootDirectoryActor);
        }

        public void SetRootDirectoryFromCommandLineArgumentsIfExists(ActorSelection actorSelection)
        {
            actorSelection.Tell(new RootDirectoryActor.SetRootDirectoryFromCommandLineArgumentsIfExists());
        }

        public void SetRootDirectory(ActorSelection actor, DirectoryInfo directory)
        {
            actor.Tell(new RootDirectoryActor.SetRootDirectory(directory));
        }
    }
}