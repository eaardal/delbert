using System;
using System.IO;
using System.Threading.Tasks;
using Akka.Actor;
using Delbert.Actors.Facades.Abstract;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Logging.Contracts;

namespace Delbert.Actors.Facades
{
    public class CommandLineArgsParserFacade : ActorFacade, ICommandLineArgsParserFacade
    {
        public CommandLineArgsParserFacade(IActorSystemAdapter actorSystem, ILogger logger) : base(actorSystem, logger)
        {

        }

        public async Task<DirectoryInfo> GetRootDirectoryFromCommandLine()
        {
            var actor = ActorSystem.ActorOf(ActorRegistry.CommandLineArgsParser);

            return await GetRootDirectoryFromCommandLine(actor);
        }
        
        public async Task<DirectoryInfo> GetRootDirectoryFromCommandLine(IActorRef actor)
        {
            var answer = await actor.Ask(new CommandLineArgsParserActor.GetRootDirectoryFromCommandLineArgs());

            if (answer is CommandLineArgsParserActor.GetRootDirectoryFromCommandLineArgsResult)
            {
                var result = answer as CommandLineArgsParserActor.GetRootDirectoryFromCommandLineArgsResult;

                return result.Success ? result.Directory : null;
            }

            LogFailure(answer);

            return null;
        } 
    }
}
