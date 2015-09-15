using System;
using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;
using Akka.Actor;
using Delbert.Actors.Facades.Abstract;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Logging.Contracts;
using Delbert.Model;

namespace Delbert.Actors.Facades
{
    class NotebookFacade : ActorFacade, INotebookFacade
    {
        public NotebookFacade(IActorSystemAdapter actorSystem, ILogger logger) : base(actorSystem, logger)
        {

        }

        public void CreateNotebook(DirectoryInfo directory)
        {
            var actor = ActorSystem.ActorOf(ActorRegistry.Notebook);

            CreateNotebook(actor, directory);
        }

        public void CreateNotebook(IUntypedActorContext context, DirectoryInfo directory)
        {
            var actor = context.ActorOf(ActorRegistry.Notebook);

            CreateNotebook(actor, directory);
        }
        
        private void CreateNotebook(IActorRef actor, DirectoryInfo directory)
        {
            actor.Tell(new NotebookActor.CreateNotebook(directory));
        }

        public async Task<ImmutableArray<NotebookDto>> GetNotebooks()
        {
            var actor = ActorSystem.ActorOf(ActorRegistry.Notebook);

            return await GetNotebooks(actor);
        }
        
        public async Task<ImmutableArray<NotebookDto>> GetNotebooks(IActorRef actor)
        {
            var answer = await actor.Ask(new NotebookActor.GetNotebooks());

            if (answer is NotebookActor.GetNotebooksResult)
            {
                var result = answer as NotebookActor.GetNotebooksResult;

                return result.Notebooks;
            }

            LogFailure(answer);

            return ImmutableArray<NotebookDto>.Empty;
        }
    }
}