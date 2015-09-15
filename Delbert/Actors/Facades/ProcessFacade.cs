using System;
using System.IO;
using Akka.Actor;
using Delbert.Actors.Facades.Abstract;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Logging.Contracts;

namespace Delbert.Actors.Facades
{
    public class ProcessFacade : ActorFacade, IProcessFacade
    {
        public ProcessFacade(IActorSystemAdapter actorSystem, ILogger logger) : base(actorSystem, logger)
        {
        }

        public void StartProcessForFile(FileInfo file)
        {
            var actor = ActorSystem.ActorOf(ActorRegistry.Process);

            StartProcessForFile(actor, file);
        }

        private void StartProcessForFile(IActorRef actor, FileInfo file)
        {
            try
            {
                actor.Tell(new ProcessActor.StartProcessForFile(file));
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}